using Newtonsoft.Json;
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
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = serverPath,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            }
        };

        _process.Start();

        _inputStream = _process.StandardInput;
        _outputStream = _process.StandardOutput;
    }
    public static async Task SendRequest(string method, object parameters)
    {
        var request = new
        {
            jsonrpc = "2.0",
            id = 1,
            method = method,
            param = parameters
        };

        var json = JsonConvert.SerializeObject(request);


        await _lock.WaitAsync();
        try
        {
            await _inputStream.WriteLineAsync(json);
            await _inputStream.FlushAsync();
        }
        finally
        {
            _lock.Release();
        }
       
    }
    public static async Task<string> ReadResponse()
    {
        await _lock.WaitAsync();
        try
        {
         return await _outputStream.ReadLineAsync();
        }
        finally
        {
            _lock.Release();
        }
    }
}
