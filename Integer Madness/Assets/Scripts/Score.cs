using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Score : MonoBehaviour {
    public AudioSource correctSound;
    public AudioSource climbLadderSound;
    public AudioSource snakeSound;

    private Text scoreTotal;
    private int playerScore;
    Dictionary<int, int> ladders = new Dictionary<int, int>() { { 3, 51 }, { 6, 27 }, { 20, 70 },
                                                                { 36, 55 }, { 63, 95 }, { 68, 98 } };
    Dictionary<int, int> snakes = new Dictionary<int, int>() { { 25, 5 }, { 34, 1 }, { 47, 19 },
                                                                { 65, 52 }, { 87, 57 }, { 91, 61 }, { 99, 69 } };

    void Awake ()
    {
        scoreTotal = GetComponent<Text>();
        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
    }

    void Update()
    {
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        scoreTotal.text = playerScore.ToString();
    }
    public int calculateScore(int index)
    {
        int cellIndex = 0;
        if (index == 3 || index == 6 || index == 20 || index == 36 || index == 63 || index == 68)
        {
            playerScore += 250;
            climbLadderSound.Play();
            cellIndex = checkLadders(index);
        }
        else if (index == 25 || index == 34 || index == 47 || index == 65 || index == 87 || index == 91 || index == 99)
        {
            playerScore += 125;
            if (playerScore < 0)
                playerScore = 0;
            snakeSound.Play();
            cellIndex = checkSnakes(index);
        }
        else
        {
            playerScore += 100;
            cellIndex = index;
            correctSound.Play();
        }
        return cellIndex;
    }

    int checkLadders(int index)
    {
        int pos = 0;
        switch (index)
        {
            case 3:
                pos = ladders[3];
                break;
            case 6:
                pos = ladders[6];
                break;
            case 20:
                pos = ladders[20];
                break;
            case 36:
                pos = ladders[36];
                break;
            case 63:
                pos = ladders[63];
                break;
            default:
                pos = ladders[68];
                break;
        }
        return pos;
    }

    int checkSnakes(int index)
    {
        int pos = 0;
        switch (index)
        {
            case 25:
                pos = snakes[25];
                break;
            case 34:
                pos = snakes[34];
                break;
            case 47:
                pos = snakes[47];
                break;
            case 65:
                pos = snakes[65];
                break;
            case 87:
                pos = snakes[87];
                break;
            case 91:
                pos = snakes[91];
                break;
            default:
                pos = snakes[99];
                break;
        }
        return pos;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", playerScore);
    }
}
