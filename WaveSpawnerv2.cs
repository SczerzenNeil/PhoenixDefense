using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveSpawnerv2 : MonoBehaviour
{
    public static int EnemiesInGame = 0;
    public NS_Wave[] waves;
    public Transform spawnpoint;
    public NS_GameManager gameManager;
    public Canvas startwaveCanvas;
    public bool multiPath = false;

    public float WaveTimer = 5f;
    public Text countdownTimer;
    [HideInInspector]
    public float wavespawn = 2f;

    private int waveNumber = 0;
    public GameObject PhoenixBaseObject;
    public Text WaveNum;
    public bool startWave = false;

    public int waveMoney = 100;


    // Start is called before the first frame update
    void Start()
    {
        WaveNum = GameObject.Find("WaveNum").GetComponent<Text>();
        wavespawn = 5f;
        waveNumber = 0;
        EnemiesInGame = 0;
        PhoenixBaseObject = GameObject.Find("PhoenixBase");
    }

    public void StartWaveSpawner()
    {
        startWave = true;
        startwaveCanvas.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemies in Game " + EnemiesInGame);

        if (startWave == true)
        {

            if (EnemiesInGame > 0)
            {
                return;
            }


            if (wavespawn <= 0f)
            {
                StartCoroutine(SpawnWave());
                wavespawn = WaveTimer;
                return;
            }
        }

        if (waveNumber == waves.Length)
        {
            Scene currentscene = SceneManager.GetSceneByName("Level 03");


            if (SceneManager.GetSceneByName("Level03").isLoaded)
            {
                TitleScreen.tutComplete = true;
                Debug.Log("Make tutcomplete true");
            }
        }

        wavespawn -= Time.deltaTime;
        if (wavespawn > 0)
            countdownTimer.text = "WaveTimer: " + wavespawn.ToString("0");
        else
            countdownTimer.text = "WaveTimer: 0";
    }

    public IEnumerator SpawnWave()
    {
        
        if (waveNumber != waves.Length)
        {
            WaveNum.text = "Wave: " + (waveNumber + 1);
            
            NS_Wave wave = waves[waveNumber];
            // Is the second enemy type null?
            if (wave.enemy2 == null && wave.enemy3 == null)
            {
                EnemiesInGame = wave.count; // Then proceed as normal.
            }
            else if (wave.enemy2 != null && wave.enemy3 == null) // If it is,
            {
                EnemiesInGame = wave.count + wave.count2; // Then add to wave.count
            }
            else if (wave.enemy2 != null && wave.enemy3 != null)
            {
                EnemiesInGame = wave.count + wave.count2 + wave.count3;
            }

            for (int i = 0; i < wave.count; i++) // Normal enemy
            {
                if (EnemiesInGame > 0) // Are the enemies left greater than 0?
                {
                    SpawnEnemy(wave.enemy);
                    yield return new WaitForSeconds(1f / wave.enemyspawnrate);

                    if (wave.enemy2 != null)
                    {
                        SpawnEnemy(wave.enemy2);
                        yield return new WaitForSeconds(1f / wave.enemy2spawnrate);
                    }

                    if (wave.enemy3 != null)
                    {
                        SpawnEnemy(wave.enemy3);
                        yield return new WaitForSeconds(1f / wave.enemy3spawnrate);
                    }
                    if (EnemiesInGame <= 0) // Are the enemies left equal to or less than 0?
                    {
                        break;
                    }
                }
            }
            
            waveNumber++;
            NS_GameManager.Money += waveMoney;
            NS_GameManager.MoneyChanged();
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        
        Instantiate(enemy, spawnpoint.position, spawnpoint.rotation);
    }
}
