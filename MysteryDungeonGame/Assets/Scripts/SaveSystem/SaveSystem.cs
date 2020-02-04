using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayerData()
    {
        SaveSystemPlayer.elapsedTime = Time.time;
        SaveSystemPlayer.tempPlayer.timePlayed += SaveSystemPlayer.elapsedTime - SaveSystemPlayer.timeSinceLast;
        SaveSystemPlayer.timeSinceLast = Time.time;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.SaveData";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, SaveSystemPlayer.tempPlayer);
        stream.Close();
        Debug.Log("File saved to " + path);
    }

    public static void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/Player.SaveData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData loadedData =  formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            Debug.Log("File loaded from " + path);
            SaveSystemPlayer.tempPlayer = loadedData;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            SaveSystemPlayer.tempPlayer = new PlayerData();
        }
    }
}
