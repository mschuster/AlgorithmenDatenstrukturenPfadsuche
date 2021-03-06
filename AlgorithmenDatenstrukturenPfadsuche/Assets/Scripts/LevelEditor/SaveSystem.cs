﻿using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

/**
 * Speichersystem beinhaltet die Funktionen zum Speichern und Laden
 *
 * Martin Schuster
 */

public static class SaveSystem {
    // Application.persistentDataPath Programm sucht sich einen beschreibbaren berreich in dem ZielSystem
    // Application.dataPath speichern im bereich des Programms
    private static string savePath = Application.dataPath + "/levels";
    private static string examplePath = Application.streamingAssetsPath + "/LearnModeLevels";

    public static string levelName;

    /*
    Speicher Funktion:
    Erstellt mittels eines BinaryFormatter die Daten als Binär Datei.
    Daten sind damit nicht ohne weiteres Lesbar/veränderbar.
     */
    public static void SaveData(List<LevelData> data) {
        BinaryFormatter formatter = new BinaryFormatter();
        try {
            if (!Directory.Exists(savePath)) {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(savePath);
            }
        } catch (IOException ioex) {
            Debug.Log(ioex.Message);
        }
        string path = savePath + "/" + levelName + ".grid";
        FileStream stream = new FileStream(path, FileMode.Create);
        SavableData saveData = new SavableData(data);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    /*
    Laden Funktion:
    Konvertiert die gespeicherte Binärdatei in ein SavableData Objekt zurück.
     */
    public static SavableData LoadLevel(string filename) {
        string path = savePath + "/" + filename;
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SavableData data = formatter.Deserialize(stream) as SavableData;
            stream.Close();
            return data;
        } else {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    public static SavableData LoadLevelExamples(string filename, string folder) {
        string path = examplePath + "/" + folder + "/" + filename;
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SavableData data = formatter.Deserialize(stream) as SavableData;
            stream.Close();
            return data;
        } else {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
