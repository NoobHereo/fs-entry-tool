using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

[Serializable]
public class AppData
{
    public string LOADED_JSON = null;
    public Dictionary<Candidate, int> Candidates = new Dictionary<Candidate, int>();
    public Dictionary<FutureSeekerData, int> AllSeekers = new Dictionary<FutureSeekerData, int>();
}

[Serializable]
public class Candidate
{
    public string IGN;
    public int Points;
    public FutureSeekerData Data;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string LOADED_JSON = null;
    public Dictionary<Candidate, int> Candidates = new Dictionary<Candidate, int>();
    public Dictionary<FutureSeekerData, int> AllSeekers = new Dictionary<FutureSeekerData, int> ();

    public static string DataPath = "/toolData.txt";
    private void Start()
    {
        Instance = this;
        if (DataValid())
        {
            AppData data = LoadData();
            Candidates = data.Candidates;
            LOADED_JSON = data.LOADED_JSON;
            AllSeekers = data.AllSeekers;
        }
        else
        {
            Debug.LogError("Data invalid");
        }
    }

    public void AddSeeker(FutureSeekerData fs, CandidateVoteType vote, int points)
    {
        Candidate candidate = new Candidate();
        candidate.IGN = fs.IGN;
        candidate.Points = points;
        candidate.Data = fs;
        Candidates.Add(candidate, Candidates.Count + 1);
    }

    public void LoadJSON(string json)
    {
        try 
        { 
            FutureSeekerData[] fsJsons = JsonHelper.FromJson<FutureSeekerData>(json);
            for (int i = 0; i < fsJsons.Length; i++)
            {
                AllSeekers.Add(fsJsons[i], i);
            }

            Debug.Log("setting JSON");
            LOADED_JSON = json;
        }
        catch 
        {
            StartCoroutine(LoadError());
            return;
        }
    }

    private IEnumerator LoadError()
    {
        yield return new WaitForSeconds(0.25f);
        UIHandler.Instance.DispatchPopUp("Something went wrong..", "The file you tried to import could not be loaded. Are you sure you are importing the right file? The export file cannot be loaded as it only holds anonymous data about entries and their respective votes.", false, false, OptionType.Misc, "");
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
        data.Candidates = Candidates;
        data.AllSeekers = AllSeekers;
        SaveData(data);
    }

    public void ResetData()
    {
        AppData data = new AppData();
        data.LOADED_JSON = "";
        data.Candidates = new Dictionary<Candidate, int>();
        data.AllSeekers = new Dictionary<FutureSeekerData, int>();
        SaveData(data);
    }
}