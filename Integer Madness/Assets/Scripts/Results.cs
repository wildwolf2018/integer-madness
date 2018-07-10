using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Results : MonoBehaviour {
    [SerializeField]
    private Text scoreView;
    [SerializeField]
    private Text bonusView;
    [SerializeField]
    private Text timeView;
    [SerializeField]
    private Text totalView;

    private BoardManager boardManager;
    private int playerScore;
    private int bonus = 100;
    private int time;
    private int total;
    private const float BONUS_FACTOR = 5000f;

    void Awake()
    {
        boardManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BoardManager>();
    }

    void OnEnable()
    {
        StartCoroutine(DisplayScore());
    }

   
    IEnumerator DisplayScore()
    {
        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        yield return new WaitForSeconds(1.5f);
        scoreView.text = playerScore.ToString();
        StartCoroutine(DisplayTime());
    }

    IEnumerator DisplayTime()
    {
        yield return new WaitForSeconds(1.5f);
        time = PlayerPrefs.GetInt("Time", 0) * 100;
        timeView.text = time.ToString();
        StartCoroutine(DisplayBonus());
    }

    IEnumerator DisplayBonus()
    {
        int correctAnswersTotal = PlayerPrefs.GetInt("CorrectAnswers", 0); 
        int incorrectAnswersTotal = PlayerPrefs.GetInt("WrongAnswers", 0);
        int denominator = correctAnswersTotal + incorrectAnswersTotal;
        if (denominator != 0)
        {
            float temp = correctAnswersTotal * BONUS_FACTOR / denominator;
            bonus = (int)(Mathf.RoundToInt(temp));
            if (PlayerPrefs.GetString("Difficulty").CompareTo("hard") == 0)
            {
                bonus += (correctAnswersTotal * 1000);
            }
        }
        else
        {
            bonus = 0;
        }
        yield return new WaitForSeconds(1.5f);
        bonusView.text = bonus.ToString();
        StartCoroutine(DisplayTotal());
    }

    IEnumerator DisplayTotal()
    {
        int counter = 0;
        PlayerPrefs.SetInt("NewGame", 1);
        total = playerScore + time + bonus;
        PlayerPrefs.SetInt("FinalScore", total);
        yield return new WaitForSeconds(1.5f);
        while (counter <= total)
        {
            totalView.text = counter.ToString();
            counter += 100;
            yield return null;
        }
        totalView.text = total.ToString();
        StartCoroutine(LevelSelect());
    }
  
    IEnumerator LevelSelect()
    {
        yield return new WaitForSeconds(5f);
        if (total > PlayerPrefs.GetInt("LowestScore", 100))
        {
            boardManager.displayScoresOnly = false;
            SceneManager.LoadScene("Registration");
        }
        else
        {
            boardManager.displayScoresOnly = true;
            SceneManager.LoadScene("Scores");
        }
    }


}
