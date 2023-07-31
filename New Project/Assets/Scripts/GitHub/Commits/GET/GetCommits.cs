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

    public async Task<Commit> GetAllComitsForRepo(string repo, string owner)
    {
        return new Commit();
    }

    [Serializable]
    public class Commit
    {
        public string name;
    }
}
