using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour
{


    public float interval = 5;
    float timeToShot;
    public bool autoshot = false;

    private void Update()
    {
        timeToShot += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoshot = !autoshot;
        }

        if (autoshot)
        {
            if (timeToShot > interval)
            {
                timeToShot = 0;
                Application.CaptureScreenshot("VRSHOOTER_" + System.DateTime.Now.ToString("yyyy-MM-dd_THH_mm_ss_Z") + ".png", 2);
            }
        }
    }



}
