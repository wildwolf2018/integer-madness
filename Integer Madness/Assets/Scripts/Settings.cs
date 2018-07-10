using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour {
 
	
	public void EasyMode()
    {
        PlayerPrefs.SetString("Difficulty", "easy");
        LoadScene();
    }

    public void HardMode()
    {
        PlayerPrefs.SetString("Difficulty", "hard");
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Board01");
    }
}
