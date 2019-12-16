using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

using PragmaScript;


using static LanguageHelper;


class HoverHandler : IHoverHandler
{
    HoverCapability capability;
    ILanguageServer router;
    BufferManager buffers;

    public HoverHandler(ILanguageServer router, BufferManager buffers)
    {
        this.router = router;
        this.buffers = buffers;
    }

    private readonly DocumentSelector _documentSelector = new DocumentSelector(
        new DocumentFilter()
        {
            Pattern = "**/*.prag"
        }
    );

    public TextDocumentRegistrationOptions GetRegistrationOptions()
    {
        var result = new TextDocumentChangeRegistrationOptions
        {
            DocumentSelector = _documentSelector,
            SyncKind = TextDocumentSyncKind.Full,
        };
        return result;
    }

    public async Task<Hover> Handle(HoverParams request, CancellationToken cancellationToken)
    {
        var documentPath = UriHelper.GetFileSystemPath(request.TextDocument.Uri);
        if (string.IsNullOrEmpty(documentPath))
        {
            return new Hover();
        }
        var (rootScope, tc) = buffers.GetScope(documentPath);
        if (rootScope == null)
        {
            return new Hover();
        }
        var scope = GetCurrentScope(rootScope, (int)request.Position.Character, (int)request.Position.Line, documentPath);

        var pos = (int)request.Position.Character;
        var line = (int)request.Position.Line;
        var node = FindNode(scope, pos, line);
        if (node == null)
        {
            return new Hover();
        }
        if (node is AST.VariableReference vr)
        {
            var ov = node.scope.GetVar(vr.variableName, node.token);
            FrontendType nt;
            nt = tc.GetNodeType(node);
            if (nt is FrontendSumType st)
            {
                nt = st.types[vr.overloadedIdx];
            }
            return new Hover
            {
                Contents = new MarkedStringsOrMarkupContent(new MarkedString($"{nt}"))
            };
        }



        return new Hover();
    }
    public void SetCapability(HoverCapability capability)
    {
        this.capability = capability;
    }
}


class CompletionHandler : ICompletionHandler
{
    CompletionCapability capability;
    ILanguageServer router;
    BufferManager buffers;

    private readonly DocumentSelector _documentSelector = new DocumentSelector(
        new DocumentFilter()
        {
            Pattern = "**/*.prag"
        }
    );

    public CompletionHandler(ILanguageServer router, BufferManager buffers)
    {
        this.router = router;
        this.buffers = buffers;
    }

    public CompletionRegistrationOptions GetRegistrationOptions()
    {
        return new CompletionRegistrationOptions()
        {
            DocumentSelector = _documentSelector,
            ResolveProvider = false,
            TriggerCharacters = new List<string> { ".", ":", "@", " ", ">" },
        };
    }


    CompletionItem Keyword(string name)
    {
        return new CompletionItem
        {
            Kind = CompletionItemKind.Keyword,
            Label = name
        };
    }


    [System.Flags]
    enum AddToScopeFlags
    {
        None = 0,
        Variables = 2,
        Types = 4,
        Modules = 8,
        Recursive = 16,
        AddSpaceToType = 32,
    }

    void AddToScope(Scope scope, AddToScopeFlags flags, List<CompletionItem> completions, TypeChecker tc)
    {
        int scopeDepth = 0;
        while (true)
        {
            if (flags.HasFlag(AddToScopeFlags.Types))
            {
                foreach (var (name, td) in scope.types)
                {
                    CompletionItemKind kind;
                    var node = td.node;
                    FrontendType ft;
                    if (node != null)
                    {
                        ft = tc.GetNodeType(node);
                    }
                    else
                    {
                        ft = td.type;
                    }
                    var detail = ft.ToString();
                    switch (ft)
                    {
                        case FrontendEnumType fet:
                            kind = CompletionItemKind.Enum;
                            break;
                        case FrontendStructType fst:
                            kind = CompletionItemKind.Struct;
                            break;
                        default:
                            kind = CompletionItemKind.Unit;
                            break;
                    }
                    var item = new CompletionItem
                    {
                        Label = name,
                        SortText = $"{scopeDepth:D3}_{name}",
                        Kind = kind,
                        Detail = detail,
                        InsertText = flags.HasFlag(AddToScopeFlags.AddSpaceToType) ? $" {name}" : null,
                    };
                    completions.Add(item);
                }
            }
            if (flags.HasFlag(AddToScopeFlags.Variables))
            {
                foreach (var (name, vd) in scope.variables)
                {
                    CompletionItemKind kind;
                    var node = vd.First.node;
                    FrontendType ft;
                    if (node == null)
                    {
                        ft = vd.First.type;
                    }
                    else
                    {
                        ft = tc.GetNodeType(node);
                    }
                    var detail = ft.ToString();
                    switch (ft)
                    {
                        case FrontendFunctionType _:
                            kind = CompletionItemKind.Function;
                            break;
                        case FrontendEnumType _:
                            kind = CompletionItemKind.EnumMember;
                            break;
                        default:
                            kind = CompletionItemKind.Variable;
                            if (vd.First.isConstant)
                            {
                                kind = CompletionItemKind.Constant;
                            }
                            break;
                    }
                    if (vd.IsOverloaded)
                    {
                        detail = $"{detail} (+{vd.variables.Count - 1} overloads)";
                    }
                    var item = new CompletionItem
                    {
                        Label = name,
                        SortText = $"{scopeDepth:D3}_{name}",
                        Kind = kind,
                        Detail = detail
                    };
                    completions.Add(item);
                }
            }
            if (scopeDepth == 0 && flags.HasFlag(AddToScopeFlags.Modules))
            {
                foreach (var (name, mod) in scope.modules)
                {
                    var item = new CompletionItem
                    {
                        Label = name,
                        SortText = $"{scopeDepth:D3}_{name}",
                        Kind = CompletionItemKind.Module,
                        Detail = mod.GetPath(),
                        InsertText = $"{name}::",
                        Command = new Command { Name = "editor.action.triggerSuggest" },
                    };
                    completions.Add(item);
                }
            }
            if (flags.HasFlag(AddToScopeFlags.Recursive) && scope.parent != null)
            {
                scope = scope.parent;
                scopeDepth++;
            }
            else
            {
                break;
            }
        }
    }

    public List<string> GetNamespacePath(string line, int charIdx)
    {
        var result = new List<string>();
        return result;
    }

    public async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        var documentPath = UriHelper.GetFileSystemPath(request.TextDocument.Uri);
        if (string.IsNullOrEmpty(documentPath))
        {
            return new CompletionList();
        }
        var (rootScope, tc) = buffers.GetScope(documentPath);
        if (rootScope == null)
        {
            return new CompletionList();
        }

        var scope = GetCurrentScope(rootScope, (int)request.Position.Character, (int)request.Position.Line, documentPath);

        var completions = new List<CompletionItem>();
        if (request.Context.TriggerKind == CompletionTriggerKind.TriggerCharacter)
        {
            var text = buffers.GetBuffer(documentPath);
            var charIdx = (int)request.Position.Character;
            var lineIdx = (int)request.Position.Line;
            var lines = text.Split(System.Environment.NewLine);
            var line = default(string);
            if (lineIdx >= 0 && lineIdx < lines.Length)
            {
                line = lines[lineIdx];
            }
            if (line == null)
            {
                return new CompletionList(completions);
            }

            if (request.Context.TriggerCharacter == " ")
            {
                charIdx--;
                if (line.ElementAtOrDefault(charIdx) == ' ' && line.ElementAtOrDefault(charIdx - 1) == ':')
                {
                    AddToScope(scope, AddToScopeFlags.Types | AddToScopeFlags.Recursive, completions, tc);
                    return new CompletionList(completions);
                }
                if (line.ElementAtOrDefault(charIdx) == ' ' && line.ElementAtOrDefault(charIdx - 1) == '>' && line.ElementAtOrDefault(charIdx - 2) == '=')
                {
                    AddToScope(scope, AddToScopeFlags.Types | AddToScopeFlags.Recursive, completions, tc);
                    return new CompletionList(completions);
                }
            }
            if (request.Context.TriggerCharacter == ">")
            {
                charIdx--;
                if (line.ElementAtOrDefault(charIdx) == '>' && line.ElementAtOrDefault(charIdx - 1) == '=')
                {
                    AddToScope(scope, AddToScopeFlags.Types | AddToScopeFlags.Recursive | AddToScopeFlags.AddSpaceToType, completions, tc);
                    return new CompletionList(completions);
                }
            }
            if (request.Context.TriggerCharacter == "@")
            {
                AddToScope(scope, AddToScopeFlags.Types | AddToScopeFlags.Recursive, completions, tc);
                return new CompletionList(completions);
            }
            if (request.Context.TriggerCharacter == ":")
            {
                charIdx--;
                if (line.ElementAtOrDefault(charIdx) == ':' && line.ElementAtOrDefault(charIdx - 1) != ':')
                {
                    AddToScope(scope, AddToScopeFlags.Types | AddToScopeFlags.Recursive | AddToScopeFlags.AddSpaceToType, completions, tc);
                    return new CompletionList(completions);
                }
                if (line.ElementAtOrDefault(charIdx) != ':' || line.ElementAtOrDefault(charIdx - 1) != ':')
                {
                    return new CompletionList(completions);
                }

                charIdx = charIdx - 2;
                List<string> query = new List<string>();
                while (true)
                {
                    var current = "";
                    while (charIdx >= 0 && Token.isIdentifierChar(line[charIdx]))
                    {
                        current = line[charIdx] + current;
                        charIdx--;
                    }
                    if (string.IsNullOrWhiteSpace(current))
                    {
                        break;
                    }
                    else
                    {
                        query.Add(current);
                    }
                    while (charIdx >= 0 && char.IsWhiteSpace(line[charIdx]))
                    {
                        charIdx--;
                    }
                    if (charIdx <= 1 || line[charIdx] != ':' || line[charIdx - 1] != ':')
                    {
                        break;
                    }
                    charIdx -= 2;

                }
                if (query.Count > 0)
                {
                    var mod = scope.GetModule(query);
                    if (mod != null)
                    {
                        AddToScope(mod.scope, AddToScopeFlags.Types | AddToScopeFlags.Variables, completions, tc);
                    }
                }
                return new CompletionList(completions);
            }

            if (request.Context.TriggerCharacter == ".")
            {
                charIdx = System.Math.Clamp(charIdx, 0, line.Length - 1);
                while (charIdx >= 0 && line[charIdx] != '.')
                {
                    charIdx--;
                }
                if (charIdx >= 0 && charIdx < line.Length)
                {
                    if (line[charIdx] == '.')
                    {
                        charIdx--;
                    }
                }
                List<string> query = new List<string>();
                while (true)
                {
                    var current = "";
                    while (charIdx >= 0 && Token.isIdentifierChar(line[charIdx]))
                    {
                        current = line[charIdx] + current;
                        charIdx--;
                    }
                    if (string.IsNullOrWhiteSpace(current))
                    {
                        break;
                    }
                    else
                    {
                        query.Add(current);
                    }
                    while (charIdx >= 0 && char.IsWhiteSpace(line[charIdx]))
                    {
                        charIdx--;
                    }
                    if (charIdx <= 0 || line[charIdx] != '.')
                    {
                        break;
                    }
                    charIdx--;
                }
                List<string> modPath = new List<string>();
                if (line.ElementAtOrDefault(charIdx) == ':' && line.ElementAtOrDefault(charIdx - 1) == ':')
                {
                    charIdx -= 2;

                    while (true)
                    {
                        var current = "";
                        while (charIdx >= 0 && Token.isIdentifierChar(line[charIdx]))
                        {
                            current = line[charIdx] + current;
                            charIdx--;
                        }
                        if (string.IsNullOrWhiteSpace(current))
                        {
                            break;
                        }
                        else
                        {
                            modPath.Add(current);
                        }
                        while (charIdx >= 0 && char.IsWhiteSpace(line[charIdx]))
                        {
                            charIdx--;
                        }
                        if (charIdx <= 1 || line[charIdx] != ':' || line[charIdx - 1] != ':')
                        {
                            break;
                        }
                        charIdx -= 2;
                    }
                }


                if (query.Count > 0)
                {
                    FrontendStructType st = null;
                    for (int i = query.Count - 1; i >= 0; --i)
                    {
                        var current = query[i];
                        if (i == query.Count - 1)
                        {
                            if (modPath.Count > 0)
                            {
                                scope = scope.GetModule(modPath)?.scope;
                                if (scope == null)
                                {
                                    break;
                                }
                            }
                            var v = scope.GetVar(current, Token.Undefined)?.First;
                            if (v == null)
                            {
                                st = null;
                                break;
                            }
                            FrontendType vt;
                            if (v.node == null)
                            {
                                vt = v.type;
                            }
                            else
                            {
                                vt = tc.GetNodeType(v.node);
                            }
                            st = vt as FrontendStructType;

                            if (st == null && vt is FrontendPointerType pt)
                            {
                                st = pt.elementType as FrontendStructType;
                            }
                        }
                        else
                        {
                            var f = st.fields.Where(f => f.name == current).FirstOrDefault();
                            if (f == null)
                            {
                                st = null;
                                break;
                            }
                            st = f.type as FrontendStructType;
                            if (st == null && f.type is FrontendPointerType pt)
                            {
                                st = pt.elementType as FrontendStructType;
                            }
                        }
                        if (st == null)
                        {
                            break;
                        }
                    }
                    if (st != null)
                    {
                        foreach (var f in st.fields)
                        {
                            var item = new CompletionItem
                            {
                                Kind = CompletionItemKind.Field,
                                Label = f.name,
                                Detail = f.type.ToString()
                            };
                            completions.Add(item);
                        }
                    }
                }
            }
        }

        if (request.Context.TriggerKind == CompletionTriggerKind.Invoked || request.Context.TriggerKind == CompletionTriggerKind.TriggerForIncompleteCompletions)
        {
            completions.Add(Keyword("let"));
            completions.Add(Keyword("var"));
            completions.Add(Keyword("struct"));
            completions.Add(Keyword("enum"));
            completions.Add(Keyword("fun"));
            completions.Add(Keyword("return"));
            completions.Add(Keyword("true"));
            completions.Add(Keyword("false"));
            completions.Add(Keyword("if"));
            completions.Add(Keyword("elif"));
            completions.Add(Keyword("else"));
            completions.Add(Keyword("for"));
            completions.Add(Keyword("while"));
            completions.Add(Keyword("break"));
            completions.Add(Keyword("continue"));
            completions.Add(Keyword("size_of"));
            completions.Add(Keyword("extern"));
            completions.Add(Keyword("import"));
            completions.Add(Keyword("mod"));
            completions.Add(Keyword("with"));
            AddToScope(scope, AddToScopeFlags.Variables | AddToScopeFlags.Modules | AddToScopeFlags.Recursive, completions, tc);
        }
        return new CompletionList(completions);
    }

    public void SetCapability(CompletionCapability capability)
    {
        this.capability = capability;
    }
}