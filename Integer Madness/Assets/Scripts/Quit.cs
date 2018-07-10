using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour {
    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
