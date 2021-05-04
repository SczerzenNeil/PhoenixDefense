using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class NS_EnemyMovement : MonoBehaviour
{
    public static NS_EnemyMovement Instance;

    [Header("Attributes")]
    public float speed = 10f;
    public float returnSpeed = 10f;
    public float startHealth = 100;
    public float health;
    public int Damage;
    public float fireRate = 1f;
    public GameObject Bullet, BlockadeBullet, HeroBullet;
    public float startingSpeed;
    public int TimesDamaged = 0;
    public int moneyRewarded;
    private bool isDead = false;
    public bool canAvoidMines = false;
    public bool canAvoidBlockades = false;
    public bool route2 = false;
    public int DeathRayDamage = 30;
    public bool shootEnemy = false;
    public bool secretEnemy = false;

    [Header("Unity Setup")]
    public GameObject deatheffect;
    public float fireTimer = 0f;
    private int waypointIndex = 0;
    private Transform target;
    public Color DamageColor;
    private Color startColor;
    private Renderer rend;
    public Image healthBar;
    public Transform turretTarget;
    

    public AudioSource a_Audio;
    public AudioClip enemydeath, damagesound, fire, startup, shutdown;

    public GameObject scoreObject;
    public GameObject Target;
    public GameObject AchievementObserver;

    public bool IsBeingPoisoned = false;
    public int PathType;

    public NS_WaveSpawner waveSpawner;
    public GameObject WaveSpawnerObject;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        AchievementObserver = FindObjectOfType<MB_AchievementObserver>().gameObject;
        a_Audio = GameObject.Find("SceneManager").GetComponent<AudioSource>();

        PathType = Random.Range(0, 10);
    }

    public void Start()
    {
        WaveSpawnerObject = FindObjectOfType<NS_WaveSpawner>().gameObject; // wavespawner object 
        var WaveSpawnerWave = WaveSpawnerObject.GetComponent<NS_WaveSpawner>();

        WaveSpawnerWave.EnemiesLeftOver.Add(this.gameObject); //add this to wavespawner list
        waveSpawner = GameObject.Find("GameManager").GetComponent<NS_WaveSpawner>();

        target = NS_Waypoints.waypoints[0];
        
        if (waveSpawner.multiPath == true)
        {
            if (PathType < 5)
                target = NS_Waypoints.waypoints[0];
            if (PathType >= 5)
                target = NS_Waypoints2.waypoints[0];
        }


        health = startHealth;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        scoreObject = this.gameObject;

    }

    public void TakeDamage (int amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        a_Audio.clip = damagesound;
        a_Audio.PlayOneShot(damagesound);
        StartCoroutine("Flash", 1f);

        if (health <= 0)
        {
            StopCoroutine("Flash");
            var WaveSpawnerWave = WaveSpawnerObject.GetComponent<NS_WaveSpawner>(); //variable for wavespawner list access

            WaveSpawnerWave.EnemiesLeftOver.Remove(this.gameObject); //remove this item from list
            Die();
        }
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            rend.material.color = DamageColor;
            Invoke("ResetMaterial", 0.2f);
        }
    }
    public void ResetMaterial()
    {
        rend.material.color = startColor;
    }

    public void Die()
    {
       // var WaveSpawnerWave = WaveSpawnerObject.GetComponent<NS_WaveSpawner>();

      //  WaveSpawnerWave.EnemiesLeftOver.Remove(this.gameObject);
        isDead = true; 
        NS_WaveSpawner.EnemiesInGame--;

        var score = scoreObject.GetComponent<ScoreHolder>(); // reference to score manager
        var Achievement = AchievementObserver.GetComponent<MB_AchievementObserver>();

        NS_GameManager.Money += moneyRewarded;
        NS_GameManager.MoneyChanged(); //matthews function addition for money UI

        score.AddOrSubtractPoints(); // No need to actually put a number in there. The script handles that. :3

        a_Audio.clip = enemydeath;
        a_Audio.PlayOneShot(enemydeath);
        GameObject effect = (GameObject)Instantiate(deatheffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        Destroy(gameObject,0.15f);

        Achievement.EnemyDeathNum++; // Increases the amount of enemies we killed...

        if (!Achievement.HasUnlockedEnemyDeathAchievement) // Did we meet the enemy achievement goal?
        {
            Achievement.CheckAchievements();
        }

    }

    

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
     

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            if(waveSpawner.multiPath == true)
            {
                 if (PathType < 5)
                 {
                     GetNextWaypoint();
                     return;
                 }
                 else if (PathType >= 5)
                 {
                     GetNextWayPoint2();
                     return;

                 }

            }
                GetNextWaypoint();
           /* if (route2 == true)
            {
                GetNextWayPoint2();
            }

            else if (route2 == false)
            {
            GetNextWaypoint();
            }  */
        }
        if (shootEnemy == true) //special eneemy shoots turrets
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);

            if (turretTarget != null)
            {
                speed = 0;
                AttackTurret();
            }
            else if (turretTarget == null)
                    speed = returnSpeed;
        }
        

    }

    void GetNextWayPoint2()
    {
      //  target = NS_Waypoints2.waypoints[0];

        if (waypointIndex >= NS_Waypoints2.waypoints.Length - 1)
        {
            speed = 0f;
            Attack();
            return;
        }
        waypointIndex++;
        target = NS_Waypoints2.waypoints[waypointIndex];
    }

    void GetNextWaypoint()
    {
             if(waypointIndex >= NS_Waypoints.waypoints.Length - 1)
             {
                 speed = 0f;
                 Attack();
                 return;
             }
             waypointIndex++;
             target = NS_Waypoints.waypoints[waypointIndex];
    }

    void Attack()
    {
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = 1f / fireRate;
        }

        fireTimer -= Time.deltaTime;
    }

    void Shoot()
    {
        a_Audio.clip = fire;
        a_Audio.PlayOneShot(fire);
        var Son = Instantiate(Bullet, transform.position, Quaternion.identity);
        Son.gameObject.GetComponent<NS_EnemyBullet>().Father = this.gameObject;

        Debug.Log("Shoot Base");
    }

    public void AttackBlockade()
    {
        ShootBlockade();;
    }

    public void ShootBlockade()
    {
        a_Audio.clip = fire;
        a_Audio.PlayOneShot(fire);

        Instantiate(BlockadeBullet, transform.position, Quaternion.identity);
    }

    public void AttackHero()
    {
        ShootHero();
    }
    public void ShootHero()
    {
        a_Audio.clip = fire;
        a_Audio.PlayOneShot(fire);

        Instantiate(HeroBullet, transform.position, Quaternion.identity);
    }
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Blockade")
        {
            if (canAvoidBlockades == false)
            {
            Target = other.gameObject;
            other.GetComponent<NWB_EnemyHealth>().Enemies.Add(this.gameObject);
            InvokeRepeating("AttackBlockade", 1, 1);
            speed = 0;
            }
            else if (canAvoidBlockades == true)
            {
                ChangeColortoWhite();
            }
           // ResetMaterial();
            // return;
        }

        if (other.gameObject.tag == "Hero")
        {
            Target = other.gameObject;
            other.GetComponent<HeroHealth>().Enemies.Add(this.gameObject);
            InvokeRepeating("AttackHero", 1, 1);
            speed = 0;
        }

        if (other.gameObject.name == "RealDeathRay" && FindObjectOfType<DeathRayFoReal>().IsBeingUsed == true)
        {
            StartCoroutine(SlowlyHarm()); // AHH
        }

        if (other.gameObject.tag == "Turret")
        {
            if (secretEnemy == true)
            {
                if (other.GetComponent<NS_Turret>().cantShoot == false)
                {
                    other.GetComponent<NS_Turret>().cantShoot = true;
                    other.GetComponent<LineRenderer>().enabled = false;
                    a_Audio.clip = shutdown;
                    a_Audio.PlayOneShot(shutdown);

                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Blockade")
            ResetMaterial();

        if (other.gameObject.tag == "Turret")
        {
            if (secretEnemy == true)
            {
                if (other.GetComponent<NS_Turret>().cantShoot == true)
                {
                    other.GetComponent<NS_Turret>().cantShoot = false;
                    other.GetComponent<LineRenderer>().enabled = true;
                    a_Audio.clip = startup;
                    a_Audio.PlayOneShot(startup);
                }
            }
        }
    }

    public void ChangeColortoWhite()
    {
        rend.material.color = Color.white;
    }

    public IEnumerator SlowlyHarm()
    {
        if (isDead == false) //sandstorm null reference fix 
        {
            if( this.gameObject != null)
            {

                if (TimesDamaged != 10)
                {
                    Debug.Log("OUCH");
                    IsBeingPoisoned = true;
                    TimesDamaged++;
                    yield return new WaitForSecondsRealtime(1);
                    TakeDamage(DeathRayDamage);
                    StartCoroutine(SlowlyHarm());
                }
                if (TimesDamaged == 10)
                {
                    IsBeingPoisoned = false;
                    //TimesDamaged = 0;

                }

            }
                yield break;
        }
        else
            Destroy(gameObject); //sandstorm null reference fix 
    }

    public int FreezeDamage = 5;
    public bool isFrozen;
    
    public void EnemySlowDown(Transform enemy)
    {
        //if (this.gameObject != null)
        //{
            if (isFrozen == false)
            {
                NS_EnemyMovement health = enemy.gameObject.GetComponent<NS_EnemyMovement>();
                enemy.gameObject.GetComponent<NS_EnemyMovement>().isFrozen = true;
                health.TakeDamage(FreezeDamage);
                health.speed = health.speed / 2;

            }

        //}
    }

    public string turretTag = "Turret";
    private float range = 5;

    void UpdateTarget()
    {
        if (shootEnemy == true)
        {


            GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
            float shortestDistance = Mathf.Infinity;
            GameObject closestTurret = null;

            foreach (GameObject turret in turrets)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, turret.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestTurret = turret;
                }
            }

            {
                if (closestTurret != null && shortestDistance <= range)
                {
                    turretTarget = closestTurret.transform;
                }
                else
                {
                    turretTarget = null;
                }
            }
        }
    }
    public GameObject TurretBullet; //special eneemy shoots turrets

    void AttackTurret() //special eneemy shoots turrets
    {
        if (fireTimer <= 0f)
        {
            ShootTurret();
            fireTimer = 1f / fireRate;
        }

        fireTimer -= Time.deltaTime;
    }

    public void ShootTurret() //special eneemy shoots turrets
    {
        Debug.Log("shoot turret");
        a_Audio.clip = fire;
        a_Audio.PlayOneShot(fire);


        Instantiate(TurretBullet, transform.position, Quaternion.identity);
    }
}
