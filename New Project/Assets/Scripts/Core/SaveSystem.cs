using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem 
{
    public static readonly string APP_PATH = Application.dataPath;

    public static void INIT(string fileDirectory)
    {
        //Test if save folder exists
        if (!Directory.Exists(APP_PATH + fileDirectory))
        {
            //Create save folder
            Directory.CreateDirectory(APP_PATH + fileDirectory);
        }
    }

    public static void Save(string saveString, string fileToSaveIn, string fileDirectory)
    {
        File.WriteAllText((APP_PATH + fileDirectory) + fileToSaveIn, saveString);
    }

    public static string Load(string fileToRead, string fileDirectory)
    {
        if (File.Exists((APP_PATH + fileDirectory) + fileToRead))
        {
            string saveString = File.ReadAllText((APP_PATH + fileDirectory) + fileToRead);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
