using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;
using System.Net;

public static class JSONManager {

    private static string basePath = Application.streamingAssetsPath + "/JSON/";
    private static string webBasePath = "https://tommasobianchi.github.io/LudumDare40/WebGL/StreamingAssets/JSON/";
    private static string myjsonHighscorePath = "https://api.myjson.com/bins/m716f";
    private static bool hasInternetConnectivity;

    public static void Initialize()
    {
        hasInternetConnectivity = checkForInternetConnection();
        Debug.Log(hasInternetConnectivity);
    }

    public static void Save<T>(string name, T item, bool saveLocally = false)
    {
        if(name == "Highscores" && hasInternetConnectivity && !saveLocally)
        {
            saveHighscores(JsonConvert.SerializeObject(item, Formatting.Indented));
            return;
        }

        StreamWriter writer = new StreamWriter(basePath + name + ".json", false);
        writer.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
        writer.Close();

        Debug.Log(name + " saved");
    }

    public static T Load<T>(string name)
    {
        if (name == "Highscores" && hasInternetConnectivity)
        {
            return JsonConvert.DeserializeObject<T>(loadHighscores());
        }

        if (!hasInternetConnectivity)
        {
            StreamReader reader = new StreamReader(basePath + name + ".json");
            string s = reader.ReadToEnd();
            reader.Close();
            return JsonConvert.DeserializeObject<T>(s);
        }
        else
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(webBasePath + name + ".json");
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string s = reader.ReadToEnd();
            reader.Close();
            T el = JsonConvert.DeserializeObject<T>(s);
            Save(name, el, true);
            return el;
        }        
    }

    public static Dictionary<string, T> LoadDirectory<T>(string directory)
    {
        Dictionary<string, T> result = new Dictionary<string, T>();

        foreach (string filePath in Directory.GetFiles(basePath + directory, "*.json"))
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            result.Add(fileName, Load<T>(directory + "/" + fileName));
        }

        return result;
    }

    private static void saveHighscores(string highscoreJson)
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(myjsonHighscorePath);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";
        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        streamWriter.Write(highscoreJson);
        streamWriter.Flush();
        streamWriter.Close();
    }

    private static string loadHighscores()
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(myjsonHighscorePath);
        HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string s = reader.ReadToEnd();
        Debug.Log(s);
        reader.Close();
        return s;
    }

    private static bool checkForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
