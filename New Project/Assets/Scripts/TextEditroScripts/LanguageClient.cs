using Newtonsoft.Json;
using OmniSharp.Extensions.LanguageServer.Client;
using OmniSharp.Extensions.LanguageServer.Protocol.Client;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LangaueClient : MonoBehaviour
{
    private static Process _process;
    private static StreamWriter _inputStream;
    private static StreamReader _outputStream;

    private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private static ILanguageClient _client;
    private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static async void StartServer(string serverPath)
    {
        ProcessStartInfo info = new ProcessStartInfo();
        var programPath = serverPath;
        info.FileName = programPath;
        info.WorkingDirectory = Path.GetDirectoryName(programPath);
        info.RedirectStandardInput = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;

        Process process = new Process
        {
            StartInfo = info
        };

        process.Start();
        try
        {
            _client = LanguageClient.Create(
           options =>
           {
               options.WithInput(process.StandardOutput.BaseStream)
                .WithOutput(process.StandardInput.BaseStream)
                .WithCapability(
                 new CompletionCapability
                 {
                     CompletionItem = new CompletionItemCapabilityOptions
                     {
                         DeprecatedSupport = true,
                         DocumentationFormat = new Container<MarkupKind>(MarkupKind.Markdown, MarkupKind.PlainText),
                         PreselectSupport = true,
                         SnippetSupport = true,
                         TagSupport = new CompletionItemTagSupportCapabilityOptions
                         {
                             ValueSet = new[] { CompletionItemTag.Deprecated }
                         },
                         CommitCharactersSupport = true
                     }
                 }
             );
           });


            await _client.Initialize(cancellationTokenSource.Token);
        } catch (Exception ex)
        {
            print(ex);
        }
       
    }

    //request completion
    public static async Task<IEnumerable<CompletionItem>> RequestCompletionAsync(string pathToFile, int line, int character)
    {
        try
        {
            var actualCompletions = await _client.TextDocument.RequestCompletion(
                new CompletionParams
                {
                    TextDocument = pathToFile,
                    Position = (line, character),
                }, cancellationTokenSource.Token
            );

            Thread.Sleep(1000);

            var items = actualCompletions.Items;

            return items;
        }
        catch (Exception ex)
        {
            throw new Exception("Error requesting completion", ex);
        }
    }
}
