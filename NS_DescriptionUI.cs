using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NS_DescriptionUI : MonoBehaviour
{
    public GameObject turretUI;
    public GameObject trapUI;
    public GameObject heroUI;
    public GameObject levelUI;
    public NWB_PauseGame pauseScript;

    private bool UItrap = false;
    private bool UIturret = false;
    private bool UIhero = false;

    

    public void DisplayTurretUI()
    {
        if (trapUI == true || UIhero == true)
            ExitUI();

        turretUI.SetActive(true);
        UIturret = true;
        Time.timeScale = 0;
        pauseScript.DisableNodes();
    }

    public void DisplayTrapUI()
    {
        if (UIturret == true || UIhero == true)
            ExitUI();

        trapUI.SetActive(true);
        UItrap = true;
        Time.timeScale = 0;
        pauseScript.DisableNodes();
    }

    public void DisplayHeroUI()
    {
        if (UIturret == true || UItrap == true)
            ExitUI();

        heroUI.SetActive(true);
        UIhero = true;
        Time.timeScale = 0;
        pauseScript.DisableNodes();
    }

    public void DisplayLevelDescription()
    {
        levelUI.SetActive(true);
        Time.timeScale = 0;
        pauseScript.DisableNodes();
    }

    public void ExitUI()
    {
        Time.timeScale = 1;
        turretUI.SetActive(false);
        UIturret = false;
        trapUI.SetActive(false);
        UItrap = false;
        heroUI.SetActive(false);
        UIhero = false;
        levelUI.SetActive(false);
    }
    
}
