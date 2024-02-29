using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System;

public class DataBaseManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    // Start is called before the first frame update
    void Start()
    {
        InitializeDB();
    }

    //save user
    public void SaveUser(UserModel userToSave)
    {
        string json = JsonUtility.ToJson(userToSave);
        dbRef.Child("users").Child(userToSave.Username).SetRawJsonValueAsync(json);
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
