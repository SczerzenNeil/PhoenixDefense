using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NS_GameManager : MonoBehaviour
{
    public static int Money;
    public int MoneyHutsBuilt = 0;
    public int startMoney = 100;
    public static Text MoneyUI;
    public string sceneName;

    public Image LevelCompleteImageThatIsStupid;

    public AudioSource a;
    public AudioClip Bloop;

    public GameObject waveSpawner;

    public void Start()
    {
        Money = startMoney;

        a = GameObject.Find("SceneManager").GetComponent<AudioSource>();
        waveSpawner = FindObjectOfType<NS_WaveSpawner>().gameObject;

        LevelCompleteImageThatIsStupid = GameObject.Find("LevelCompleteImage").GetComponent<Image>();

        MoneyUI = GameObject.Find("MoneyNum").GetComponent<Text>(); // Can't set it in the editor, so we have to use the other way...
        NS_GameManager.MoneyChanged(); //matthews function addition for money UI

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
    }

    public string nextlevel = "Level02";
    public int levelToUnock = 2;

    public static void MoneyChanged()
    {
        MoneyUI.text = "$" + Money;
        var achievement = FindObjectOfType<MB_AchievementObserver>().GetComponent<MB_AchievementObserver>();

        achievement.CheckAchievements();

    }
    

  public void LevelComplete()
    {

        if(waveSpawner.GetComponent<NS_WaveSpawner>().EnemiesLeft <= 0)
        {
        Debug.Log("Level Compelete");
        PlayerPrefs.SetInt("levelreached", levelToUnock);
      //  StartCoroutine(LevelCompleteVisuals());
        SceneManager.LoadScene(nextlevel);

        }
      /*  if (sceneName == "NewMetrics")
            {
            SceneManager.LoadScene("Level01");
            return;
            }  */

    }

    public void DebugLevelComplete()
    {
        Debug.Log("Level Compelete");
        PlayerPrefs.SetInt("levelreached", levelToUnock);
        //StartCoroutine(LevelCompleteVisuals());
        SceneManager.LoadScene(nextlevel);
    }

    public IEnumerator LevelCompleteVisuals()
    {
        LevelCompleteImageThatIsStupid.enabled = true;
        a.PlayOneShot(Bloop);
        yield return new WaitForSecondsRealtime(1);
        LevelCompleteImageThatIsStupid.enabled = false;
        yield break;
    }
}
