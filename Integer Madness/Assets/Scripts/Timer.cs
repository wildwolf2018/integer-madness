using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
    public float count;//Remaining time before level ends
    private float timer = 1f;
    private float countRate = 2f;//Rate at which timer is decreasing
    private Text timeValue;//Current timer value

    void Awake ()
    {
        timeValue = GetComponent<Text>();
        count = PlayerPrefs.GetInt("Time", 300);
        timeValue.text = count.ToString();
    }
	
	
	void Update ()
    {
        if (Time.time > timer && count > 0)
        {
            timer = Time.time + countRate;
            count--;
            timeValue.text = count.ToString();
        }
        PlayerPrefs.SetInt("Time", (int)count);
    }
}
