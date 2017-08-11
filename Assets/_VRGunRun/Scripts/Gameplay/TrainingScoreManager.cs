using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScoreManager : MonoBehaviour
{
    private GameManager gameManager;

    public int Range10MHitCount, Range50MHitCount, Range100MHitCount, Range200MHitCount, Range300MHitCount;

    public int Points = 0;

    public void RegisterPoints(int points, float range)
    {
        Points += points;

        if (range <= 10f)
        {
            Range10MHitCount++;
        }
        else if (range <= 50f)
        {
            Range50MHitCount++;
        }
        else if (range <= 100f)
        {
            Range100MHitCount++;
        }
        else if (range <= 200f)
        {
            Range200MHitCount++;
        }
        else if (range <= 300f)
        {
            Range300MHitCount++;
        }
    }
}
