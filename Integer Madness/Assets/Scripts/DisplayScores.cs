using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class DisplayScores : MonoBehaviour {
	[SerializeField]
    private Text[] scores;//Players' high scores 
	[SerializeField]
    private Text[] initials;//Players' initials
    private BoardManager scoresList;

	void Start ()
    {
        scoresList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BoardManager>();
        if (!scoresList.displayScoresOnly)
        {
            StartCoroutine(SaveHighScores());
            StartCoroutine(DisplayHighScores());
            StartCoroutine(Wait());
        }
        else
        {
            StartCoroutine(DisplayHighScores());
            StartCoroutine(Wait());
        }
	}
	
	//Saves the playerScore to file
	IEnumerator SaveHighScores()
    {
        yield return null;
        int playerScore = PlayerPrefs.GetInt("FinalScore", 0);
        for (int i = 0; i < scoresList.scores.Count; i++)
        {
            string[] temp = scoresList.scores[i].Split(new char[] { ' ' });
            int score2 = Int32.Parse(temp[1]);
            if (score2 < playerScore)
            {
                scoresList.scores.Insert(i, PlayerPrefs.GetString("PlayerInitials", "AAA") + " " + playerScore);
                scoresList.scores.RemoveAt(5);
                break;
            }
        }
        FileStream  fwrite = new FileStream("Scores.dat", FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fwrite, scoresList.scores);
        fwrite.Close();
      
    }
	//Diplays the top five scores
    IEnumerator DisplayHighScores()
    {
        yield return null;
        for (int i =0; i < scoresList.scores.Count; i++)
        {
            string[] temp = scoresList.scores[i].Split(new char[] { ' ' });
            initials[i].text = "";
            initials[i].text = temp[0];
            scores[i].text = "";
            scores[i].text = temp[1];
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Menu");
    }
}
