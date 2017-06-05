using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject Panel;
    public Text Timer;
    public Text Level;
    private Text BestText;
    private float counter;
    private TimeSpan timespan;
    private bool gamePlaying;
    private float storeTime;
    private static float newTime;
    private static float bestTime;
    public UIHandler UI;
    

	// Use this for initialization
	void Start () {
        BestText = Panel.transform.Find("BestTime").gameObject.GetComponent<Text>();
        counter = 0;
        gamePlaying = true;
        Level.text = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        storeTime = Time.timeScale;
        bestTime = 9999999999f;
        bestTime = PlayerPrefs.GetFloat(Level.text, bestTime);
        BestText.text = TimeFormat(bestTime);
    }
	
	// Update is called once per frame
	void Update () {
        if (gamePlaying)
        {
            UpdateTime();
        }
        else
        {
            
        }
    }
    
    // Update the timer text per frame.
    void UpdateTime()
    {
        counter += Time.deltaTime * 1.0f;
        Timer.text = TimeFormat(counter);
    }

    //Formats the counter variable to fit a "00:00" format. 
    String TimeFormat(float time)
    {
        timespan = TimeSpan.FromSeconds(time);
        var result = string.Format("{0:D2}:{1:D2}", (int)timespan.TotalMinutes, timespan.Seconds);
        return result.ToString();
    }

    //Stop the timer, display score, etc. Load game over menu probably.
    public void stopGame()
    {
        gamePlaying = false;
        UI.showPanel(true);
        Time.timeScale = 0f;
        newTime = counter;
        if(newTime < bestTime)
        {
            bestTime = newTime;
            BestText.text = TimeFormat(bestTime);
            PlayerPrefs.SetFloat(Level.text, bestTime);
        }
    }
    
    //Stop the timer, and show the pause menu
    public void pauseGame()
    {
        gamePlaying = false;
        Time.timeScale = 0f;
    }

    //Resume timer, close pause menu
    public void resumeGame()
    {
        gamePlaying = true;
        Time.timeScale = storeTime;
    }

    //Get best Time
    public float getBestTime()
    {
        return bestTime;
    }
 
}
