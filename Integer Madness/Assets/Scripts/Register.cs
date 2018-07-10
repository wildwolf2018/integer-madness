using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour {
    public Text[] initials;
    public Button[] initButtons;
    private string playerInitials = "";
    private int currentLetter = 65;
    private int index = 0;
    private Animator anim1;
    private Animator anim2;
    private Animator anim3;

    void Start ()
    {
        anim1 = initials[0].GetComponent<Animator>();
        anim2 = initials[1].GetComponent<Animator>();
        anim3 = initials[2].GetComponent<Animator>();
    }
	
    public void Previous()
    {
        switch (currentLetter)
        {
            case 65:
                currentLetter = 60;
                break;
            case 60:
                currentLetter = 57;
                break;
            case 48:
                currentLetter = 90;
                break;
            default:
                currentLetter -= 1;
                break;
        }
        if (index <= 2) SetInitial();
    }

    private void SetInitial()
    {
        initials[index].text = ((char)currentLetter).ToString();
    }

    public void Next()
    {
        switch (currentLetter)
        {
            case 90:
                currentLetter = 48;
                break;
            case 57:
                currentLetter = 60;
                break;
            case 60:
                currentLetter = 65;
                break;
            default:
                currentLetter += 1;
                break;
        }
        if (index <= 2) SetInitial();
    }

    public void Enter()
    {
        if (index == 1 && currentLetter == 60)
        {
            currentLetter = 65;
            initials[1].text = ((char)currentLetter).ToString();
            anim2.enabled = false;
            initials[1].color = Color.white;
            anim1.enabled = true;
            index = 0;
        }
        else if (index == 2 && currentLetter == 60) 
        {
            currentLetter = 65;
            initials[2].text = ((char)currentLetter).ToString();
            anim3.enabled = false;
            initials[2].color = Color.white;
            anim2.enabled = true;
            index = 1;
        }
        else if (index == 0 && currentLetter == 60)
        {
            index = 0;
        }
        else
        {
            if (index == 0)
            {

                anim1.enabled = false;
                initials[0].color = Color.white;
                anim2.enabled = true;
            }
            else if (index == 1 )
            {
                anim2.enabled = false;
                initials[1].color = Color.white;
                anim3.enabled = true;
                
            }
            else if (index == 2 )
            {
                anim3.enabled = false;
                initials[2].color = Color.white;
                playerInitials = initials[0].text + initials[1].text + initials[2].text;
                PlayerPrefs.SetString("PlayerInitials", playerInitials);
                initButtons[0].enabled = false;
                initButtons[1].enabled = false;
                initButtons[2].enabled = false;
                Invoke("EndRegistration", 3f);
            }
            index += 1;
            if (index < 3)
            {
                initials[index].text = "A";
            }
         
        }
    }

    void EndRegistration()
    {
        SceneManager.LoadScene("Scores");
    }
}
