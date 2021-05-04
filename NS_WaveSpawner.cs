using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NS_WaveSpawner : MonoBehaviour
{
    public bool multiPath = false;
    public GameObject win;
    public static int EnemiesInGame = 0;
    public NS_Wave[] waves;
    public Transform spawnpoint;
    public NS_GameManager gameManager;
    public Canvas startwaveCanvas;
    public int waveMoney = 100;

    public float WaveTimer = 5f;
    public Text countdownTimer;
    [HideInInspector]
    public float wavespawn = 2f;


    private int waveNumber = 0;
    public int TimesRun = 0;
    private int LastWaveNumber = 0;

    public GameObject AchievementObserver;
    public GameObject PhoenixBaseObject;
    public GameObject DialogSystem;
    public GameObject EndGameStats;

    public string[] DialogTips = { "You can press T to release a powerful deathray.", "You can upgrade a turret by clicking on it and pressing 'upgrade'.", "Some enemies are stronger than others, so place your turrets carefully!", "I think there are some enemies that only be killed by one type of turret...", "You can sell turrets by clicking on them and pressing 'sell'.", "Some enemies ignore traps, so be careful!", "The currency hut is a great way to make more money, provided that you can afford it!", "The hero unit is great for desperate situations!", "Want to slow down units? Use the heavy turret!", "Be sure to save up your money! Some turrets only deal damage to certain enemies!", "Here is a tip for meteor levels: Your turrets are magnets to them!", "The hero only hangs out by the base! Keep this in mind before using it!", "The enemies don't like it when the hero is in their path, so they will fire back!", "Are enemies taking longer to kill than earlier? Try upgrading a few turrets.", "Certain turrets cannot see some enemies, be sure to have a mix of turrets placed!" };

    public Image LevelCompleteImageThatIsStupid;

    public AudioSource a;
    public AudioClip Bloop;
    public bool startWave = false;
    //public bool HasReconBuilt = false;
    //public bool HasReconUpgraded = false;
    public bool IsTutorialLevel = false;
    public bool EnemyDiag1, EnemyDiag2, EnemyDiag3, EnemyDiag4, MultipleEnemiesInAWaveDialog;

    public Text WaveNum;
    public RectTransform WaveNumRT;
    public NWB_AudioManager audioManager;

    private void Start()
    {

        WaveNum = GameObject.Find("WaveNum").GetComponent<Text>();
        WaveNumRT = WaveNum.GetComponent<RectTransform>();
        AchievementObserver = FindObjectOfType<MB_AchievementObserver>().gameObject;
        if(GameObject.Find("EndGameStatChecker") != null)
        {
            EndGameStats = GameObject.FindObjectOfType<EndGameStats>().gameObject;
        }
        wavespawn = 5f;
        waveNumber = 0;
        EnemiesInGame = 0;
        PhoenixBaseObject = GameObject.Find("PhoenixBase");
        DialogSystem = FindObjectOfType<MB_DialogSystem>().gameObject;


        a = GameObject.Find("SceneManager").GetComponent<AudioSource>();
        audioManager = FindObjectOfType<NWB_AudioManager>();

        StartCoroutine(EndWaveChecker());

        var wave = waves[waveNumber];
        var dialog = DialogSystem.GetComponent<MB_DialogSystem>();

        if (waveNumber == 0 && IsTutorialLevel == false)
        {
            if (wave.enemy.gameObject.name == "Enemy1")
            {
                StartCoroutine(dialog.ShowReconDialog("I see a purple looking fella in the distance!"));
                EnemyDiag1 = true;
            }
            if (wave.enemy.gameObject.name == "Enemy2")
            {
                StartCoroutine(dialog.ShowReconDialog("I see a light blue looking fella in the distance!"));
                EnemyDiag2 = true;
            }
            if (wave.enemy.gameObject.name == "Enemy3")
            {
                StartCoroutine(dialog.ShowReconDialog("I see a strong looking orange fella in the distance!"));
                EnemyDiag3 = true;
            }
            if (wave.enemy.gameObject.name == "Enemy4")
            {
                StartCoroutine(dialog.ShowReconDialog("I see a pretty fast yella fella in the distance! He sure does look agile!"));
                EnemyDiag4 = true;
            }
        }

        LevelCompleteImageThatIsStupid = GameObject.Find("LevelCompleteImage").GetComponent<Image>();
        // Time.timeScale = 0;
    }

    public void StartWaveSpawner()
    {
        startWave = true;
        startwaveCanvas.enabled = false;
        //  Time.timeScale = 1;
        //  wavespawn = 0;
    }

    private void Update()
    {
        EnemiesLeft = EnemiesLeftOver.Count; //not sure why but when using an int, the number of enemies lines up
                                             //  Debug.Log("Enemies in Scene " + EnemiesLeftOver.Count); //debugging enemies left over
                                             //  Debug.Log("Enemies in Game " + EnemiesInGame);
                                             //  Debug.Log("WaveSpawn " + wavespawn);

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    Debug.Log(" Wave number: " + waveNumber + " timer: " + " " + wavespawn + " Enemies Left: " + " " + EnemiesInGame + " Has Recon Been Built Yet: " + HasReconBuilt);
        //}

        //if (Input.GetKeyDown(KeyCode.Space)) // Ends the game on command.
        //{
        //    if (IsTutorialLevel == false)
        //    {
        //        EndGameStats.GetComponent<EndGameStats>().ProcessScore();
        //        EndGameStats.GetComponent<EndGameStats>().CurrentScene = SceneManager.GetActiveScene();
        //    }
        //    gameManager.LevelComplete();
        //}

        var dialog = DialogSystem.GetComponent<MB_DialogSystem>();
        if (startWave == true)
        {

            // if (EnemiesInGame > 0) //checking for enemies in game to minus a certian amount since its broken but also checking to see if enemies in game equals zero to progress
            //  {
            //     return;
            //  }


            if (wavespawn <= 0)// && !dialog.IsShowingReconText)
            {
                audioManager.PlayWaveChange();
                StartCoroutine(SpawnWave());
                wavespawn = WaveTimer;
                return;
            }

            var Achievement = AchievementObserver.GetComponent<MB_AchievementObserver>();
            if (waveNumber == waves.Length)
            {
                //Achievement.CheckAchievements(); // For checking end game achievements...
                // win.SetActive(true);

                var PhoenixBase = PhoenixBaseObject.GetComponent<PhoenixBase>();
                if (EnemiesLeft <= 0) //added for check if all enemies are dead
                {
                    if (Achievement.CanGetDeathRayAchievement == false && PhoenixBase.Health <= 2)
                    {
                        Debug.Log("1");

                        gameManager.LevelComplete();
                        this.enabled = false;
                    }
                    if (PhoenixBase.Health >= 2 && Achievement.CanGetDeathRayAchievement == false)
                    {
                        Debug.Log("2");
                        StartCoroutine(DelayEndGame());
                    }
                    if (PhoenixBase.Health <= 2 && Achievement.CanGetDeathRayAchievement == true)
                    {
                        Debug.Log("3");
                        StartCoroutine(DelayEndGame()); // We need to wait for the achievement to finish showing....
                    }
                    if (PhoenixBase.Health >= 2 && Achievement.CanGetDeathRayAchievement == true)
                    {
                        Debug.Log("4");
                        StartCoroutine(DelayEndGameButLonger()); // This is for if the player unlocked both achievements.
                    }
                }
                Scene currentscene = SceneManager.GetSceneByName("Level 03");


                if (SceneManager.GetSceneByName("Level03").isLoaded)
                {
                    TitleScreen.tutComplete = true;
                    Debug.Log("Make tutcomplete true");
                }
                // Time.timeScale = 0;
                Debug.Log("Level complete");
                Debug.Log("Has unlocked base health = " + Achievement.HasUnlockedBaseHealthAchievement + ". " + "Has unlocked death ray achievement = " + Achievement.HasUnlockedDeathRayAchievement + ".");
                //this.enabled = false;
            }
            if (EnemiesInGame <= 15 && EnemiesLeft <= 0) //checking for enemies in game to minus a certian amount since its broken but also checking to see if enemies in game equals zero to progress
                wavespawn -= Time.deltaTime;
            if (wavespawn > 0)
                countdownTimer.text = "WaveTimer: " + wavespawn.ToString("0");
            else
                countdownTimer.text = "WaveTimer: 0";
        }
    }
    public List<GameObject> EnemiesLeftOver; //list for enemies
    public int EnemiesLeft; // used for converting the list count into an int, it works better and idk why lol

    public IEnumerator SpawnWave() // IEnumerator for spawning a wave.
    {

        var dialog = DialogSystem.GetComponent<MB_DialogSystem>(); // Use this in your script to reference the dialog system.
        if (waveNumber != waves.Length)
        {
            WaveNum.text = "Wave: " + (waveNumber + 1) + "/" + waves.Length;
            StartCoroutine(WaveTextEvent());
            if (IsTutorialLevel == false)
            {
                StartCoroutine(dialog.ShowTutorialDialog(DialogTips[Random.Range(0, DialogTips.Length)])); // Shows a random tutorial tip.
            }
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
            else if (wave.enemy2 != null && wave.enemy3 != null) // Conditions for third enemy
            {
                EnemiesInGame = wave.count + wave.count2 + wave.count3;
            }

            for (int i = 0; i < wave.count; i++) // Enemy Spawning
            {
                //  if (EnemiesInGame > 0) // Are the enemies left greater than 0?
                // {
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
                if (EnemiesInGame <= 15) // Are the enemies left equal to or less than 0?
                {
                    if (EnemiesLeft <= 0) //checks for enemies left in game
                    {
                        EnemiesLeftOver.RemoveAll((o) => o == null); //resets list, just a safety measure
                        break;
                    }
                    //    }
                }
            }

            //if (LastWaveNumber != waveNumber)
            //{
            //    EnemyDiag1 = false;
            //    EnemyDiag2 = false;
            //    EnemyDiag3 = false;
            //    EnemyDiag4 = false;
            //    MultipleEnemiesInAWaveDialog = false;
            //}

            StartCoroutine(EndWaveChecker());
            CheckDiags();
            waveNumber++;
            NS_GameManager.Money += waveMoney;
            NS_GameManager.MoneyChanged();
        }
    }

    //public float textEventTimer = 1f;
    public IEnumerator WaveTextEvent()
    {
        //WaveNumRT.sizeDelta = new Vector3(WaveNum.fontSize * 10, 200);
        WaveNumRT.localScale = new Vector3(2, 2, 2);
        WaveNumRT.localPosition = new Vector3(250, 200, 0);
        WaveNum.GetComponent<Text>().color = Color.yellow;
        yield return new WaitForSecondsRealtime(0.5f);
        WaveNum.GetComponent<Text>().color = Color.red;
        WaveNumRT.localScale = new Vector3(0.33368f, 0.33368f, 0.33368f);
        WaveNumRT.localPosition = new Vector3(-620, 430, 0);


    }

    public void CheckDiags()
    {
        var dialog = DialogSystem.GetComponent<MB_DialogSystem>();
        //  ⚠ WARNING! Lots of tabs ahead! Don't say you weren't warned! ⚠
        // For normal recon dialog
        if (waveNumber < waves.Length && waveNumber != 0) // Is the game not over?
        {
            if (waveNumber != 0 && dialog.IsShowingReconText == false && IsTutorialLevel == false)
            {
                NS_Wave wave = waves[waveNumber];

                if (wave.enemy.gameObject.name == "Enemy1" && !EnemyDiag1)
                {
                    if (dialog.IsShowingReconText == false)
                    {
                        StartCoroutine(dialog.ShowReconDialog("I see a weak purple looking fella in the distance! I hear they can avoid blockades!"));
                        EnemyDiag1 = true;
                    }
                }
                if (wave.enemy.gameObject.name == "Enemy2")
                {
                    if (dialog.IsShowingReconText == false && !EnemyDiag2)
                    {
                        StartCoroutine(dialog.ShowReconDialog("I see a light blue looking fella in the distance! I think they are similar to the purple guys stats wise."));
                        EnemyDiag2 = true;
                    }
                }
                if (wave.enemy.gameObject.name == "Enemy3")
                {
                    if (dialog.IsShowingReconText == false && !EnemyDiag3)
                    {
                        StartCoroutine(dialog.ShowReconDialog("I see a strong looking orange fella in the distance!"));
                        EnemyDiag3 = true;
                    }
                }
                if (wave.enemy.gameObject.name == "Enemy4")
                {
                    if (dialog.IsShowingReconText == false && !EnemyDiag4)
                    {
                        StartCoroutine(dialog.ShowReconDialog("I see a pretty fast yella fella in the distance! He sure does look agile enough to avoid mines!"));
                        EnemyDiag4 = true;
                    }
                }
            }
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        var Achievement = AchievementObserver.GetComponent<MB_AchievementObserver>();

        Achievement.WaveNum = waveNumber;

        if (!Achievement.HasUnlockedWaveAchievement)
        {
            Achievement.CheckAchievements();
        }

        Instantiate(enemy, spawnpoint.position, spawnpoint.rotation);
    }

    public IEnumerator EndWaveChecker()
    {
        if (LastWaveNumber != waveNumber)
        {
            EnemyDiag1 = false;
            EnemyDiag2 = false;
            EnemyDiag3 = false;
            EnemyDiag4 = false;
            MultipleEnemiesInAWaveDialog = false;
        }
        if (wavespawn > 1)
        {
            CheckDiags();
        }
        yield return new WaitForSecondsRealtime(0.5f);
        LastWaveNumber = waveNumber - 1;
        //StartCoroutine(EndWaveChecker());
        yield break;
    }

    public IEnumerator DelayEndGame()
    {
        StartCoroutine(LevelCompleteVisuals());
        var Achievement = AchievementObserver.GetComponent<MB_AchievementObserver>();
        Achievement.CheckForEndGameAchievements = true;
        Achievement.CheckAchievements();
        yield return new WaitForSecondsRealtime(3);
        if (IsTutorialLevel == false)
        {
            if(EndGameStats != null)
            {
                EndGameStats.GetComponent<EndGameStats>().ProcessScore();
               // EndGameStats.GetComponent<EndGameStats>().CurrentScene = SceneManager.GetActiveScene();
                EndGameStats.GetComponent<EndGameStats>().ProcessScene();
            }
        }
        gameManager.LevelComplete();
        this.enabled = false;
        yield break;
    }
    public IEnumerator DelayEndGameButLonger()
    {
        StartCoroutine(LevelCompleteVisuals());
        var Achievement = AchievementObserver.GetComponent<MB_AchievementObserver>();
        Achievement.CheckForEndGameAchievements = true;
        Achievement.CheckAchievements();
        yield return new WaitForSecondsRealtime(6);
        if (IsTutorialLevel == false)
        {
            if (EndGameStats != null)
            {
                EndGameStats.GetComponent<EndGameStats>().ProcessScore();
               // EndGameStats.GetComponent<EndGameStats>().CurrentScene = SceneManager.GetActiveScene();
                EndGameStats.GetComponent<EndGameStats>().ProcessScene();
            }
        }
        gameManager.LevelComplete();
        this.enabled = false;
        yield break;
    }
    public IEnumerator LevelCompleteVisuals()
    {
        TimesRun++;
        if (TimesRun == 1)
        {
            LevelCompleteImageThatIsStupid.enabled = true;
            a.clip = Bloop;
            a.Play();
            yield return new WaitForSecondsRealtime(1);
            LevelCompleteImageThatIsStupid.enabled = false;
            TimesRun = 0;
            yield break;
        }
    }
}
