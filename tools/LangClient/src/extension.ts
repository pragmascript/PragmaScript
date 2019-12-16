/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */

import * as vscode from 'vscode';
import { LanguageClient, LanguageClientOptions, SettingMonitor, ServerOptions, TransportKind, InitializeParams } from 'vscode-languageclient';
import { Trace } from 'vscode-jsonrpc';

export function activate(context: vscode.ExtensionContext) {

    // The server is implemented in node
    let serverExe = 'C:\\Projects\\dotnet\\PragmaScript\\tools\\LangServer\\bin\\Debug\\netcoreapp3.0\\LangServer.exe';

    // If the extension is launched in debug mode then the debug server options are used
    // Otherwise the run options are used
    let serverOptions: ServerOptions = {
        run: { command: serverExe },
        debug: { command: serverExe }
    }
    // Use the console to output diagnostic information (conso le.log) and errors (console.error)
    // This line of code will only be executed once when your extension is activated
    console.log('Congratulations, your extension "pragma-language-server" is now active!');

    // The command has been defined in the package.json file
    // Now provide the implementation of the command with registerCommand
    // The commandId parameter must match the command field in package.json
    {
        let disposable = vscode.commands.registerCommand('extension.helloWorld', () => {
            // The code you place here will be executed every time your command is executed

            // Display a message box to the user
            vscode.window.showInformationMessage('Hello World!');
        });

        context.subscriptions.push(disposable);
    }


    // Options to control the language client
    let clientOptions: LanguageClientOptions = {
        // Register the server for plain text documents
        documentSelector: [
            {
                pattern: '**/*.prag',
            }
        ],
        synchronize: {
            // Synchronize the setting section 'languageServerExample' to the server
            configurationSection: 'PragmaLanguageServer'
        }
    }

    // Create the language client and start the client.
    const client = new LanguageClient('PragmaLanguageServer', 'Pragma Language Server', serverOptions, clientOptions);

    client.trace = Trace.Verbose;
    let disposable = client.start();

    // Push the disposable to the context's subscriptions so that the
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);

}


// this method is called when your extension is deactivated
export function deactivate() { }
