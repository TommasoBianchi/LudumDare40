using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class JSONManager {

    private static string basePath = Application.streamingAssetsPath + "/JSON/";

    public static void Save<T>(string name, T item)
    {
        StreamWriter writer = new StreamWriter(basePath + name + ".json", false);
        writer.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
        writer.Close();

    }

    public static T Load<T>(string name)
    {
        StreamReader reader = new StreamReader(basePath + name + ".json");
        string s = reader.ReadToEnd();
        reader.Close();
        return JsonConvert.DeserializeObject<T>(s);
    }
}
