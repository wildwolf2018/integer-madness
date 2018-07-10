using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class BoardManager : MonoBehaviour {
    public static BoardManager instance = null;
    public List<string> scores;
    public bool displayScoresOnly = true;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Initialize();
    }
	//Creates file to hold scores if it doesn't already exist
    void Initialize()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!File.Exists("Scores.dat"))
        {
            scores = new List<string>()
            {
                "AAA 500", "BBB 400", "CCC 300", "DDD 200", "EEE 100"
            };
            FileStream fwrite = new FileStream("Scores.dat", FileMode.Create);
            formatter.Serialize(fwrite, scores);
            fwrite.Close();
            formatter = null;
        }
        else
        {
            FileStream fread = new FileStream("Scores.dat", FileMode.Open);
            scores = (List<string>)formatter.Deserialize(fread);
            fread.Close();
            formatter = null;
        }
        string[] info = scores[4].Split(new char[] { ' ' });
        PlayerPrefs.SetInt("LowestScore", Int32.Parse(info[1]));
    }
}
