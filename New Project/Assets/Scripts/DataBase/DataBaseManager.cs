using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;
using System.Linq;
using Unity.VisualScripting;

public class DataBaseManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeDB();
    }

    //save user
    public async void SaveUser(string username)
    {
        UserModel userToSave = new UserModel(username);
        userToSave.recentLobbies = new List<int>();

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(userToSave);
        await dbRef.Child("users").Child(userToSave.Username).SetRawJsonValueAsync(json).ContinueWith(task =>
              {
                if (task.IsFaulted || task.IsCanceled)
                {
                   print(task.Exception);
                }
              });
    }

    public Task<UserModel> GetUser(string username)
    {
        // Get the user data from /users/$username
        return dbRef.Child("users").Child(username).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to retrieve user: " + task.Exception);
                throw task.Exception;
            }
            else
            {
                DataSnapshot snapshot = task.Result;

                // Check if the user exists
                if (snapshot.Exists)
                {
                    // Convert the DataSnapshot to UserModel
                    UserModel user = JsonUtility.FromJson<UserModel>(snapshot.GetRawJsonValue());

                    return user;
                }
                else
                {
                    throw new Exception("User does not exist.");
                }
            }
        });
    }
    public Task<bool> CheckIfUserExists(string username)
    {
        // Get the user data from /users/$username
        return dbRef.Child("users").Child(username).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to check if user exists: " + task.Exception);
                throw task.Exception;
            }
            else
            {
                DataSnapshot snapshot = task.Result;

                // Check if the user exists
                return snapshot.Exists;
            }
        });
    }
    //gets the lobbies for a user
    public async Task UpdateRecentLobbies(string username, string lobbyId)
    {
        var userRecord = await dbRef.Child("users").Child(username).GetValueAsync();
        var recentLobbiesJson = userRecord.Child("recentLobbies").GetRawJsonValue();

        List<string> recentLobbies = string.IsNullOrEmpty(recentLobbiesJson)
            ? new List<string>()
            : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(recentLobbiesJson);

        recentLobbies.Add(lobbyId);

        if (recentLobbies.Count > 10)
        {
            recentLobbies = recentLobbies.TakeLast(10).ToList();
        }

        // Update the user's record
        await dbRef.Child("users").Child(username).Child("recentLobbies").SetRawJsonValueAsync(Newtonsoft.Json.JsonConvert.SerializeObject(recentLobbies));
    }

    public async Task<List<string>> GetRecentLobbies(string username)
    {
        var userRecord = await dbRef.Child("users").Child(username).GetValueAsync();
        var recentLobbiesJson = userRecord.Child("recentLobbies").GetRawJsonValue();

        List<string> recentLobbies = string.IsNullOrEmpty(recentLobbiesJson)
            ? new List<string>()
            : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(recentLobbiesJson);

        return recentLobbies;
    }

    //removes the recent lobby
    public async Task RemoveRecentLobby(string username, string lobbyId)
    {
        var userRecord = await dbRef.Child("users").Child(username).GetValueAsync();
        var recentLobbies = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(userRecord.Child("recentLobbies").GetRawJsonValue());

        recentLobbies.Remove(lobbyId);

        // Update the user's record
        await dbRef.Child("users").Child(username).Child("recentLobbies").SetRawJsonValueAsync(Newtonsoft.Json.JsonConvert.SerializeObject(recentLobbies));
    }

   
    private void InitializeDB()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
}
