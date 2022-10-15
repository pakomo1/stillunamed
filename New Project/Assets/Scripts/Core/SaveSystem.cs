using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem 
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void INIT()
    {
        //Test if save folder exists
        if (!Directory.Exists(SAVE_FOLDER))
        {
            //Create save folder
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString, string fileToSaveIn)
    {
        File.WriteAllText(SAVE_FOLDER + fileToSaveIn, saveString);
    }

    public static string Load(string fileToRead)
    {
        if (File.Exists(SAVE_FOLDER + fileToRead))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + fileToRead);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
