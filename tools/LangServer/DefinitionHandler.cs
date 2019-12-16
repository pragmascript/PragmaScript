using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

using PragmaScript;

using static LanguageHelper;


class DefinitionHandler : IDefinitionHandler
{
    DefinitionCapability capability;
    ILanguageServer router;
    BufferManager buffers;

    private readonly DocumentSelector _documentSelector = new DocumentSelector(
        new DocumentFilter()
        {
            Pattern = "**/*.prag"
        }
    );

    public DefinitionHandler(ILanguageServer router, BufferManager buffers)
    {
        this.router = router;
        this.buffers = buffers;
    }

    public async Task<LocationOrLocationLinks> Handle(DefinitionParams request, CancellationToken cancellationToken)
    {
        var documentPath = UriHelper.GetFileSystemPath(request.TextDocument.Uri);
        if (string.IsNullOrEmpty(documentPath))
        {
            return new LocationOrLocationLinks();
        }
        var (rootScope, tc) = buffers.GetScope(documentPath);
        if (rootScope == null)
        {
            return new LocationOrLocationLinks();
        }

        var pos = (int)request.Position.Character;
        var line = (int)request.Position.Line;

        var scope = GetCurrentScope(rootScope, (int)request.Position.Character, (int)request.Position.Line, documentPath);

        var node = FindNode(scope, pos, line);
        if (node == null)
        {
            return new LocationOrLocationLinks();
        }
        if (node is AST.VariableReference vr)
        {
            var ov = node.scope.GetVar(vr.variableName, node.token);
            Scope.VariableDefinition vd;
            if (!ov.IsOverloaded)
            {
                vd = ov.First;
            }
            else
            {
                vd = ov.variables[vr.overloadedIdx];
            }

            Token token = Token.Undefined;
            if (vd.isFunctionParameter)
            {
                var function = node.scope.function;
                var ts = function.typeString.functionTypeString;
                var par = ts.parameters[vd.parameterIdx];
                token = par.typeString.token;
            }
            else
            {
                token = vd.node.token;
            }
            var location = new Location()
            {
                Uri = UriHelper.FromFileSystemPath(token.filename),
                Range = new Range(
                    new Position(token.Line - 1, token.Pos - 1),
                    new Position(token.Line - 1, token.Pos - 1 + token.length)
                )
            };
            return new LocationOrLocationLinks(location);
        }
        return new LocationOrLocationLinks();
    }

    TextDocumentRegistrationOptions IRegistration<TextDocumentRegistrationOptions>.GetRegistrationOptions()
    {
        return new TextDocumentRegistrationOptions
        {
            DocumentSelector = _documentSelector,
        };
    }

    public void SetCapability(DefinitionCapability capability)
    {
        this.capability = capability;
    }
}