using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour {
    public Button resume;
    private GameObject music;
    private BoardManager boardManager;

	// Use this for initialization
	void Awake ()
    {
        music = GameObject.FindWithTag("SplashMusic");
        boardManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BoardManager>();
        if (PlayerPrefs.GetInt("NewGame") == 1)
        {
            SetButtonColor(new Color(1f, 0f, 1f, 0.4f));
            resume.enabled = false;
        }
        else if (PlayerPrefs.GetInt("NewGame") == 0)
        {
            SetButtonColor(new Color(1f, 0f, 1f, 1f));
            resume.enabled = true;
        }
    }

    void SetButtonColor(Color newColor)
    {
        ColorBlock co = resume.colors;
        co.normalColor = newColor;
        resume.colors = co;
    }
	
	//Displays settingd menu screen
    public void StartSettingsMenu()
    {
        boardManager.displayScoresOnly = false;
        PlayerPrefs.SetInt("NewGame", 1);
        SetBoardInfo();
        StopSplashMusic();
        SceneManager.LoadScene("Settings");
    }

	//Sets default values for board level
    void SetBoardInfo()
    {
        PlayerPrefs.SetInt("Row", 0);
        PlayerPrefs.SetInt("Column", 0);
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetInt("Time", 300);
        PlayerPrefs.SetInt("CorrectAnswers", 0);
        PlayerPrefs.SetInt("WrongAnswers", 0);
    }

	//Loads the screen that displays high scores
    public void HighScores()
    {
        boardManager.displayScoresOnly = true;
        SceneManager.LoadScene("Scores");
    }
	
	//Continues previously active game
    public void ResumeGame()
    {
        StopSplashMusic();
        SceneManager.LoadScene("Board01");
    }

	//Causes game to stop when Quit button is pressed
    public void QuitGame()
    {
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
                    Application.Quit ();
        #endif
    }

    //Displays help information
    public void Help()
    {
        SceneManager.LoadScene("Help");
    }
	
	//Plays intro music
    void StopSplashMusic()
    {
        if (music != null)
        {
            music.GetComponent<AudioSource>().Stop();
            Destroy(music);
        }
    }
}
