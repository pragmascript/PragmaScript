using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

using PragmaScript;

using static LanguageHelper;


class SignatureHelpHandler : ISignatureHelpHandler
{
    SignatureHelpCapability capability;
    ILanguageServer router;
    BufferManager buffers;

    private readonly DocumentSelector _documentSelector = new DocumentSelector(
        new DocumentFilter()
        {
            Language = "pragma",
            Scheme = "file",
            Pattern = "**/*.prag"
        }
    );

    public SignatureHelpHandler(ILanguageServer router, BufferManager buffers)
    {
        this.router = router;
        this.buffers = buffers;
    }

    public async Task<SignatureHelp> Handle(SignatureHelpParams request, CancellationToken cancellationToken)
    {
        var documentPath = DocumentUri.GetFileSystemPath(request.TextDocument.Uri);
        if (string.IsNullOrEmpty(documentPath))
        {
            return new SignatureHelp();
        }
        var (rootScope, tc) = buffers.GetScope(documentPath);
        if (rootScope == null)
        {
            return new SignatureHelp();
        }

        var scope = GetCurrentScope(rootScope, (int)request.Position.Character, (int)request.Position.Line, documentPath);

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
            return new SignatureHelp();
        }

        charIdx = System.Math.Clamp(charIdx, 0, line.Length - 1);
        int activeParam = 0;

        while (charIdx >= 0 && line[charIdx] != '(')
        {
            if (line[charIdx] == ',')
            {
                activeParam++;
            }
            charIdx--;
        }
        if (charIdx >= 0 && charIdx < line.Length)
        {
            if (line[charIdx] == '(')
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
            Scope.OverloadedVariableDefinition vd = null;
            FrontendStructType st = null;
            for (int i = query.Count - 1; i >= 0; --i)
            {
                var current = query[i];
                FrontendType vt;
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
                    if (i == 0)
                    {
                        vd = scope.GetVar(current, Token.Undefined);
                    }
                    else
                    {
                        var v = scope.GetVar(current, Token.Undefined)?.First;
                        if (v == null)
                        {
                            st = null;
                            break;
                        }
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
                        if (st == null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        vd = scope.GetVar(current, Token.Undefined);
                    }
                    else
                    {
                        var f = st.fields.Where(f => f.name == current).FirstOrDefault();
                        if (f == null)
                        {
                            st = null;
                            break;
                        }
                        vt = f.type;
                        st = vt as FrontendStructType;
                        if (st == null && vt is FrontendPointerType pt)
                        {
                            st = pt.elementType as FrontendStructType;
                        }
                        if (st == null)
                        {
                            break;
                        }
                    }
                }
            }
            if (vd != null)
            {
                var infos = new List<SignatureInformation>();
                foreach (var v in vd.variables)
                {
                    FrontendFunctionType ft;
                    if (v.node == null)
                    {
                        ft = v.type as FrontendFunctionType;
                    }
                    else
                    {
                        ft = tc.GetNodeType(v.node) as FrontendFunctionType;
                    }
                    if (ft != null)
                    {
                        var info = new SignatureInformation();
                        var parameInfos = new List<ParameterInformation>();
                        var funName = ft.funName;
                        if (string.IsNullOrEmpty(funName))
                        {
                            funName = v.name;
                        }
                        var funString = $"{funName}(";
                        for (int i = 0; i < ft.parameters.Count; ++i)
                        {
                            var p = ft.parameters[i];
                            var paramString = $"{p.name}: {p.type}";

                            parameInfos.Add(
                                new ParameterInformation()
                                {
                                    Label = paramString,
                                }
                            );
                            funString += paramString;
                            if (i < ft.parameters.Count - 1)
                            {
                                funString += "; ";
                            }
                        }
                        funString += $") => {ft.returnType}";
                        info.Label = funString;
                        info.Parameters = parameInfos;
                        infos.Add(info);
                    }
                }
                var result = new SignatureHelp
                {
                    ActiveParameter = activeParam,
                    Signatures = new Container<SignatureInformation>(infos)
                };
                return result;
            }
        }
        return new SignatureHelp();
    }

    public SignatureHelpRegistrationOptions GetRegistrationOptions()
    {
        return new SignatureHelpRegistrationOptions
        {
            DocumentSelector = _documentSelector,
            TriggerCharacters = new List<string> { "(" },
        };
    }

    public void SetCapability(SignatureHelpCapability capability)
    {
        this.capability = capability;
    }
}
