using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveManager
{//sistema de guardado con serializacion binaria
    static string dataPath = Application.persistentDataPath + "level.save";
    static BinaryFormatter binaryFormatter = new BinaryFormatter();
    public static  void SaveLevelData(int level)
    {
        Data data = new Data(level);
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }
    public static Data LoadLevelData()
    {
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            Data data = (Data)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        else
        {
            Debug.Log("NO SE ENCONTRO EL ARCHIVO");
            return null;
        }
    }
}