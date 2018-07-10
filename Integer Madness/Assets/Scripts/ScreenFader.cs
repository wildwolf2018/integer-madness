using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour {
    public SpriteRenderer sprite;
    public AudioSource startMusic;
    public float screenFadeSpeed = 0.5f;
  
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(startMusic);
	}
	
	void Update ()
    {
        FadeToDark();
    }

    public void FadeToDark()
    {
        DarkScreen();
        if (sprite.color.a >= 0.95f)
        {
            sprite.color = Color.black;
            SceneManager.LoadScene("Menu");
        }
    }

    void DarkScreen()
    {
        sprite.color = Color.Lerp(sprite.color, Color.black, screenFadeSpeed * Time.deltaTime);
    }
}
