using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootMenuManager : MonoBehaviour
{
    public void ActivateMenu(string menu, string objectName)
    {
        switch (menu)
        {
            case "Start":
                NewGame();
                break;
            case "Quit":
                ExitGame();
                break;
            case "Load":
                LoadGame();
                break;
            case "":
                Debug.LogError("Set up proper name for : " + objectName);
                break;
            default:
                Debug.LogError("Incorrect Selection on :" + objectName);
                break;
        }
    }

    void NewGame()
    {
        Application.LoadLevel(1);
        Application.UnloadLevel(0);
    }

    void LoadGame()
    {

    }

    void ExitGame()
    {
        Application.Quit();
    }


}
