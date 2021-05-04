using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NS_Turret : MonoBehaviour
{
    public Transform target;

    [Header("Attributes")]

    public float range;
    public float fireRate = 1f;
    public float fireTimer = 0f;
    public GameObject bulletPrefab;
    public float rotatespeed = 10f;
    public float startHealth = 200;
    public bool cantShoot = false;
    

    [Header("Unity Setup")]

    public string enemyTag = "Enemy";
    public string enemyTag2 = "HeavyEnemy";

  //  public Canvas _healthBar;
    public GameObject destroyedEffect;
   // public Image healthBar; 

    [HideInInspector]
    public float health;

    private bool isDestroyed = false;
    public Transform Rotate;
    public Transform bulletspawn;
    public AudioSource a_Audio;
    public AudioClip targethit, damageSound, destroyed;
    public bool usetracker;
    public LineRenderer linerenderer;
    public bool isEnhanced = false;
    public bool shootObject = false;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        a_Audio = GameObject.Find("SceneManager").GetComponent<AudioSource>();

        health = startHealth;

        Scene CurrentScene = SceneManager.GetActiveScene(); // Gets the current scene
        
        if (CurrentScene.name == "NS_ProtoType 05" || CurrentScene.name == "MeteorLV2" || CurrentScene.name == "NS_MeteorxSandstorm") // Are we on the Meteor level?
        {
            var MeatyOre = GameObject.FindObjectOfType<NS_MeteorMovement>(); // Reference to the meteor script
            MeatyOre.Turrets.Add(gameObject); // Adds self to turrets
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
       // healthBar.fillAmount = health / startHealth;
        a_Audio.clip = damageSound;
        a_Audio.PlayOneShot(damageSound);

        if (health <= 0 && !isDestroyed)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        Scene CurrentScene = SceneManager.GetActiveScene(); // Gets the current scene
        if(CurrentScene.name == "NS_ProtoType 05" || CurrentScene.name == "MeteorLV2" || CurrentScene.name == "NS_MeteorxSandstorm") // Are we on the Meteor level?
        {
            var MeatyOre = GameObject.FindObjectOfType<NS_MeteorMovement>(); // Reference to the meteor script
            MeatyOre.Turrets.Remove(gameObject); // Removes self from turrets
        }
        
        isDestroyed = true;

        a_Audio.clip = destroyed;
        a_Audio.PlayOneShot(destroyed);
        GameObject effect = (GameObject)Instantiate(destroyedEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        Destroy(gameObject, 0.15f);
    }

    public int TimesDamaged = 0;
    public bool IsBeingPoisoned = false;
    public IEnumerator SlowlyHarm()
    {
        if (this.gameObject != null)
        {
             if (TimesDamaged != 10)
         {
             Debug.Log("OUCH");
             IsBeingPoisoned = true;
             TimesDamaged++;
             yield return new WaitForSecondsRealtime(1);
             TakeDamage(3);
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

    void UpdateTarget()
    {
        if (cantShoot == false)
        {


            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            GameObject[] enemies2 = GameObject.FindGameObjectsWithTag(enemyTag2);
            float shortestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }

            foreach (GameObject enemy2 in enemies2)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy2.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = enemy2;
                }
            }

            {
                if (closestEnemy != null && shortestDistance <= range)
                {
                    target = closestEnemy.transform;
                }
                else
                {
                    target = null;
                }
            }
        }    
    }

 
    private void Update()
    {
         if (Input.GetKeyDown(KeyCode.K))
         {
             TakeDamage(50);
         } 
        if (cantShoot == false)
        {


            if (target == null)
            {
                if (usetracker)
                {
                    if (linerenderer.enabled)
                        linerenderer.enabled = false;

                }

                return;
            }

            LockOnTarget();

            if (usetracker)
            {
                Laser();
            }

            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = 1f / fireRate;
            }

            fireTimer -= Time.deltaTime;

        }
    }

    void LockOnTarget()
    {
        //Target Lock
        Vector3 dir = target.position - transform.position;
        Quaternion lookrotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(Rotate.rotation, lookrotation, Time.deltaTime * rotatespeed).eulerAngles;
        Rotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }
    //tracker laser
    void Laser()
    {

        if (!linerenderer.enabled)
            linerenderer.enabled = true; 

        linerenderer.SetPosition(0, bulletspawn.position);
        linerenderer.SetPosition(1, target.position);
    }

    void Shoot()
    {
        a_Audio.clip = targethit;
        a_Audio.PlayOneShot(targethit);
       
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, bulletspawn.position, bulletspawn.rotation);
        NS_Bullet bullet = bulletGO.GetComponent<NS_Bullet>();

        if(shootObject == true)
        {
            bullet.isShootingObject = true;
        } 
        if (bullet != null)
            bullet.Seek(target);
    }
   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
