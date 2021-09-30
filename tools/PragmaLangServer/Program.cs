using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;

namespace LangServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = await LanguageServer.From(options =>
            options
                .WithInput(Console.OpenStandardInput())
                .WithOutput(Console.OpenStandardOutput())
                .WithLoggerFactory(new LoggerFactory())
                .AddDefaultLoggingProvider()
                .WithServices(ConfigureServices)
                .WithHandler<TextDocumentSyncHandler>()
                .WithHandler<CompletionHandler>()
                .WithHandler<DefinitionHandler>()
                .WithHandler<SignatureHelpHandler>()
                .WithHandler<HoverHandler>()
                .OnStarted(async (languageServer, result, token) => {
                    var configuration = await languageServer.Configuration.GetConfiguration(
                        new ConfigurationItem {
                            Section = "pragma"
                        }
                    ).ConfigureAwait(false);
                    TextDocumentSyncHandler.includeDirectory = configuration["pragma:includeDirectory"];
                })

            ).ConfigureAwait(false);
            await server.WaitForExit;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BufferManager>();
        }
    }
}
