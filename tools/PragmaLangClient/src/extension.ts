// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as path from 'path';
import * as vscode from 'vscode';

import {
	LanguageClient,
	LanguageClientOptions,
	ServerOptions
} from 'vscode-languageclient';

let client: LanguageClient;

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
	let basePath = context.asAbsolutePath("");
	console.log(`basepath: ${basePath}`)

	let serverPath = path.join(basePath, "server/PragmaLangServer")

	console.log(`serverPath: ${serverPath}`)

	let serverOptions: ServerOptions = {
		run: { command: serverPath },
		debug: { command: serverPath }
	};

	let clientOptions: LanguageClientOptions = {
		documentSelector: [
			{
				pattern: "**/*.prag",
			}
		],
		synchronize: {
			// Notify the server about file changes to '.prag files contained in the workspace
			fileEvents: vscode.workspace.createFileSystemWatcher('**/.prag')
		},
	}

	client = new LanguageClient("PragmaLanguageClient", serverOptions, clientOptions);
	client.start();

	// The command has been defined in the package.json file
	// Now provide the implementation of the command with registerCommand
	// The commandId parameter must match the command field in package.json
	let disposable = vscode.commands.registerCommand('pragmaclient.helloWorld', () => {
		// The code you place here will be executed every time your command is executed

		// Display a message box to the user
		vscode.window.showInformationMessage('Hello World from PragmaClient!');
	});

	context.subscriptions.push(disposable);
}

// this method is called when your extension is deactivated
export function deactivate() { }
