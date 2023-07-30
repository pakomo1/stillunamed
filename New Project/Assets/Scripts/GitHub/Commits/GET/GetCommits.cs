using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GetCommits : MonoBehaviour
{
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<List<Commit>> GetAllComitsForRepo(string repo, string owner)
    {
        string url = $"/repos/{owner}/{repo}/commits";
        var commits = new List<Commit>();

       UnityWebRequest request = apiRequestHelper.CreateAuthRequest(url);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if(request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            commits = JsonUtility.FromJson<List<Commit>>(jsonResponse);
        }
        else
        {
            print(request.error);
        }

        return commits;
    }

    [Serializable]
    public class Commit
    {
        public string name;
    }
}
