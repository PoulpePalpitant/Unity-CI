using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Xml;
/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * Date     : 23/04/2022

 * Brief    : Gère lecture/écriture des données du jeu dans un fichier
 * 
 * In-Depth : Il n'existera qu'un seul fichier de sauvegarde pour le jeu
 * 
 * Reference: https://www.red-gate.com/simple-talk/development/dotnet-development/saving-game-data-with-unity/
 * **********************************************************************
*/

public class SaveManager
{
    static readonly string SAVE_PATH = Path.Combine(Application.persistentDataPath, "save.secret");
    static readonly string SAVE_PATH_BACKUP = Path.Combine(Application.persistentDataPath, "save_backup.secret");

    // Fo real, ne jamais exposer ça!!!
    static private SaveData __savedData;
    static public bool IsSaveDataSet { get => __savedData != null; }

    public static void SaveGame()
    {
        var serialized = JsonUtility.ToJson(__savedData);

        string encrypted = Utils.Data.EncryptAES(serialized);
        SaveToFile(SAVE_PATH, encrypted);
    }
    public static void LoadGame()
    {
        string stringData = LoadFile(SAVE_PATH);

        if (stringData == "")
        {
            Debug.LogWarning("No Save file found");
            return;
        }

        stringData = Utils.Data.DecryptAES(stringData);
        var data = JsonUtility.FromJson<SaveData>(stringData);
        __savedData = data;
    }

    static void SaveToFile(string saveFilePath, string data)
    {
        using (StreamWriter stream = new StreamWriter(saveFilePath, false, System.Text.Encoding.UTF8))
        {
            stream.Write(data);
        }
    }

    /// <summary>
    /// If no save file was found, returns an empty string
    /// </summary>
    static string LoadFile(string saveFilePath)
    {
        string savedData = "";
        if (File.Exists(saveFilePath))
        {
            using (StreamReader stream = new StreamReader(saveFilePath, System.Text.Encoding.UTF8))
            {
                savedData = stream.ReadToEnd();
            }
        }
        return savedData;
    }

    /// <summary>
    /// WARNING: ceci va overwrite la save file précédente
    /// </summary>
    static public void CreateNewSaveFile(string name)
    {
        __savedData = new SaveData(name);
        SaveGame();
    }

    public static void SeriouslyDeleteAllSaveFiles()
    {
        try
        {
            // Check if file exists with its full path    
            if (File.Exists(SAVE_PATH))
            {
                // If file found, delete it    
                File.Delete(SAVE_PATH);
            }
        }
        catch (IOException ioExp)
        {
            Debug.LogError(ioExp.Message);
        }
    }

    // *********************************
    // ----------------------------------------
    // Accès et modification du save file      | ----------------->
    // ----------------------------------------
    // *********************************

    // WARNING: Le save data doit être créé préalablement

    // ACCESS METHODS
    // *****************


    // REGISTER METHODS
    // *****************

}
