using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class AppData
{
    public string LOADED_JSON = null;
    public Dictionary<FutureSeekerData, int> ApprovedSeekers = new Dictionary<FutureSeekerData, int>();
    public Dictionary<FutureSeekerData, int> DeniedSeekers = new Dictionary<FutureSeekerData, int>();
    public Dictionary<FutureSeekerData, int> AllSeekers = new Dictionary<FutureSeekerData, int>();
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string LOADED_JSON = null;
    public Dictionary<FutureSeekerData, int> ApprovedSeekers = new Dictionary<FutureSeekerData, int> ();
    public Dictionary<FutureSeekerData, int> DeniedSeekers = new Dictionary<FutureSeekerData, int> ();
    public Dictionary<FutureSeekerData, int> AllSeekers = new Dictionary<FutureSeekerData, int> ();

    public static string DataPath = "/toolData.txt";
    private void Start()
    {
        Instance = this;
        if (DataValid())
        {
            AppData data = LoadData();
            LOADED_JSON = data.LOADED_JSON;
            ApprovedSeekers = data.ApprovedSeekers;
            DeniedSeekers = data.DeniedSeekers;
            AllSeekers = data.AllSeekers;
        }
        else
        {
            Debug.LogError("Data invalid");
        }
    }

    public void AddSeeker(FutureSeekerData fs, CandidateVoteType vote, int points)
    {
        if (fs != null && vote == CandidateVoteType.Approved)
            ApprovedSeekers.Add(fs, points);
        else if (fs != null && vote == CandidateVoteType.Denied)
            DeniedSeekers.Add(fs, points);
    }

    public void LoadJSON(string json)
    {
        LOADED_JSON = json;
        FutureSeekerData[] fsJsons = JsonHelper.FromJson<FutureSeekerData>(json);
        for (int i = 0; i < fsJsons.Length; i++)
        {
            AllSeekers.Add(fsJsons[i], i);
        }
    }

    public void SaveData(AppData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + DataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public AppData LoadData()
    {
        string path = Application.persistentDataPath + DataPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AppData data = formatter.Deserialize(stream) as AppData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public bool DataValid()
    {
        string path = Application.persistentDataPath + DataPath;

        if (File.Exists(path))
            return true;
        else
            return false;
    }

    public void SaveCurrentData()
    {
        AppData data = new AppData();
        data.LOADED_JSON = LOADED_JSON;
        data.ApprovedSeekers = ApprovedSeekers;
        data.DeniedSeekers = DeniedSeekers;
        data.AllSeekers = AllSeekers;
        SaveData(data);
    }

    public void ResetData()
    {
        AppData data = new AppData();
        data.LOADED_JSON = "";
        data.ApprovedSeekers = new Dictionary<FutureSeekerData, int>();
        data.DeniedSeekers = new Dictionary<FutureSeekerData, int>();
        data.AllSeekers = new Dictionary<FutureSeekerData, int>();
        SaveData(data);
    }
}