using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedLevel : MonoBehaviour
{
    public void LoadedLevels(int loadedLevel)
    {
        Application.LoadLevel(loadedLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
