using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;

    public bool timerIsRunning = false;

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public string DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return string.Format("{0}", seconds);
    }

    public bool TimeUp() => !timerIsRunning;
}
