using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

using PragmaScript;



class TextDocumentSyncHandler : ITextDocumentSyncHandler
{
    public static string includeDirectory;
    private readonly ILanguageServer router;
    private readonly BufferManager buffers;
    private SynchronizationCapability capability;
    private Compiler compiler;

    private readonly DocumentSelector documentSelector = new DocumentSelector(
        new DocumentFilter()
        {
            Language = "pragma",
            Scheme = "file",
            Pattern = "**/*.prag"
        }
    );

    public TextDocumentSyncHandler(ILanguageServer router, BufferManager buffers)
    {
        this.router = router;
        this.buffers = buffers;
        
        compiler = new Compiler((fp) =>
        {
            var result = buffers.GetBuffer(fp);
            if (result == null)
            {
                if (File.Exists(fp))
                {
                    result = File.ReadAllText(fp);
                    buffers.UpdateBuffer(fp, result);
                }
            }
            return result;
        });
    }

    public TextDocumentChangeRegistrationOptions GetRegistrationOptions()
    {
        return new TextDocumentChangeRegistrationOptions
        {
            SyncKind = OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities.TextDocumentSyncKind.Full,
            DocumentSelector = documentSelector
        };
    }

    public TextDocumentAttributes GetTextDocumentAttributes(Uri uri)
    {
        return new TextDocumentAttributes(uri, "pragma");
    }

    string UpdateBuffer(DocumentUri document, string text)
    {
        var documentPath = DocumentUri.GetFileSystemPath(document);
        if (documentPath == null)
        {
            return documentPath;
        }
        if (string.IsNullOrEmpty(text))
        {
            return documentPath;
        }
        buffers.UpdateBuffer(documentPath, text);
        return documentPath;

    }

    void compileAndSubmitErrors(DocumentUri document, string text)
    {
        var documentPath = UpdateBuffer(document, text);
        bool foundError = false;
        try
        {
            var co = new PragmaScript.CompilerOptionsBuild
            {
                inputFilename = document.GetFileSystemPath(),
                buildExecuteable = false,
                lib_path = new List<string>(),
                libs = new List<string>(),
                include_dirs = new List<string>{ includeDirectory }
            };
            var (rootScope, tc) = compiler.Compile(co);
            buffers.Annotate(documentPath, rootScope, tc);
        }
        catch (CompilerError error)
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            var diag = new Diagnostic
            {
                Message = $"{process.ProcessName}::{process.Id}: " + error.message,
                Severity = DiagnosticSeverity.Error,
                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(new Position(error.token.Line - 1, error.token.Pos - 1), new Position(error.token.Line - 1, error.token.Pos - 1 + error.token.length)),
            };

            DocumentUri uri;
            if (error.token == Token.Undefined)
            {
                uri = document;
            }
            else
            {
                uri = DocumentUri.FromFileSystemPath(error.token.filename);
            }
            router.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
            {
                Diagnostics = new List<Diagnostic> { diag },
                Uri = uri,
            });
            foundError = true;
        }
        if (!foundError)
        {
            router.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
            {
                Diagnostics = new List<Diagnostic> { },
                Uri = document,
            });
        }
    }

    public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        compileAndSubmitErrors(request.TextDocument.Uri, request.ContentChanges.FirstOrDefault()?.Text);
        return Unit.Task;
    }

    public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        compileAndSubmitErrors(request.TextDocument.Uri, request.TextDocument.Text);
        return Unit.Task;
    }

    public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public void SetCapability(SynchronizationCapability capability)
    {
        this.capability = capability;
    }

    TextDocumentRegistrationOptions IRegistration<TextDocumentRegistrationOptions>.GetRegistrationOptions()
    {
        return new TextDocumentRegistrationOptions
        {
            DocumentSelector = documentSelector
        };
    }

    TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions>.GetRegistrationOptions()
    {
        return new TextDocumentSaveRegistrationOptions
        {
            DocumentSelector = documentSelector,
            IncludeText = true
        };
    }

    public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
        return new TextDocumentAttributes(uri, "pragma");
    }
}