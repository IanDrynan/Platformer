using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text Timer;
    private double counter;
    private TimeSpan timespan;
    private bool gamePlaying;
    

	// Use this for initialization
	void Start () {
        counter = 0;
        gamePlaying = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (gamePlaying)
        {
            UpdateTime();
        }
        else
        {
            stopGame();
        }
    }
    
    // Update the timer text per frame.
    void UpdateTime()
    {
        counter += Time.deltaTime * 1.0;
        Timer.text = TimeFormat(counter);
    }

    //Formats the counter variable to fit a "00:00" format. 
    String TimeFormat(double time)
    {
        timespan = TimeSpan.FromSeconds(time);
        var result = string.Format("{0:D2}:{1:D2}", (int)timespan.TotalMinutes, timespan.Seconds);
        return result.ToString();
    }

    //Stop the timer, display score, etc. Load game over menu probably.
    public void stopGame()
    {
        gamePlaying = false;
    }
}
