using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlomoManager : MonoBehaviour
{
    public bool IsInSlomo = false;

    public float SlomoTimeFactor = 1 / 100; //seconds
    public float CurrentTimeFactor;
    public float SlomoDuration = 3; // seconds

    private void Update()
    {
        if (IsInSlomo)
        {
            Time.timeScale += (1 / SlomoDuration) * Time.unscaledDeltaTime;
            if (Time.timeScale >= 1)
            {
                StopSlomo();
            }
        }
    }

    public void StartSlomoFor(float seconds)
    {
        StopSlomo();
        SlomoDuration = seconds;
        IsInSlomo = true;
        Time.timeScale = SlomoTimeFactor;
        Time.fixedDeltaTime = SlomoTimeFactor;
        CurrentTimeFactor = 0;
    }

    void StopSlomo()
    {
        IsInSlomo = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 1;
    }
}
