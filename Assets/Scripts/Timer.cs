using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] SuccesssPanel successsPanel;

    float startTime;
    float stopedTime;

    bool timerRunning = false;

    Cube cube;

    void Start()
    {
        cube = FindObjectOfType<Cube>();
    }

    void Update()
    {
        if (timerRunning == true)
        {
            timeText.text = getTimeString(Time.time - startTime);
        }
    }

    string getTimeString(float time)
    {
        int whole = Mathf.FloorToInt(time);
        int fraction = Mathf.FloorToInt((time - whole) * 100);

        int seconds = whole % 60;
        int minutes = whole / 60;

        string stringTime;
        if (minutes > 0)
        {
            stringTime = $"{minutes}:{seconds:00}.{fraction:00}";
        }
        else
        {
            stringTime = $"{seconds:0}.{fraction:00}";
        }

        
        return stringTime;
    }

    public void StartTimerPressed()
    {
        if (timerRunning == false)
        {
            StartCoroutine("ScrambleAndStart");
        }
    }

    IEnumerator ScrambleAndStart()
    {
        cube.ScrambleCube();
        // Wait a bit so the cube can start the scramble
        yield return new WaitForSeconds(1f); 
        // Wait until scrambling is done
        yield return new WaitUntil(() => cube.currentlyRotating == 0);
        StartTimer();
    }

    public void StartTimer()
    {
        if (timerRunning == false)
        {
            timerRunning = true;
            startTime = Time.time;
        }
        else
        {
            Debug.Log("Timer already running.");
        }
    }

    public void StopTimer()
    {
        if (timerRunning == true)
        {
            timerRunning = false;
            stopedTime = Time.time - startTime;
            successsPanel.gameObject.SetActive(true);
            successsPanel.setTime(getTimeString(stopedTime));
        }
        else
        {
            Debug.Log("Can't stop. It is not running.");
        }
        
    }
}
