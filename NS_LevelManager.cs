using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NS_LevelManager : MonoBehaviour
{
    public Button[] LevelButtons;

    public void Start()
    {
        int levelreached = PlayerPrefs.GetInt("levelreached", 1);

        for (int i = 0; i < LevelButtons.Length; i++)
        {

            if (i + 1 > levelreached)
             LevelButtons[i].interactable = false;
        }
    }
    public void Select(string level)
    {
        SceneManager.LoadScene(level);
    }
}
