using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    public Transform target;

    [Header("Attributes")]

    public float range;
    public float fireRate = 1f;
    public float fireTimer = 0f;
    public GameObject bulletPrefab;
    public float rotatespeed = 10f;

    [Header("Unity Setup")]

    public string enemyTag = "Enemy";
    public string enemyTag2 = "HeavyEnemy";
    public string enemyTag3 = "HeavyEnemy(S)";
    public string enemyTag4 = "HeavyEnemy(M)";
    public string enemyTag5 = "SecretEnemy";

    public Transform Rotate;
    public Transform bulletspawn;
    public AudioSource a_Audio;
    public AudioClip targethit;
    public bool usetracker;
    public LineRenderer linerenderer;
    public bool isEnhanced = false;
    public bool shootObject = false;
    public bool isAttacking = false; //needed bool to tell HeroMovement that the hero is enganging

    public Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        a_Audio = GameObject.Find("SceneManager").GetComponent<AudioSource>();
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag(enemyTag2);
        GameObject[] enemies3 = GameObject.FindGameObjectsWithTag(enemyTag3);
        GameObject[] enemies4 = GameObject.FindGameObjectsWithTag(enemyTag4);
        GameObject[] enemies5 = GameObject.FindGameObjectsWithTag(enemyTag5);
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

        foreach (GameObject enemy2 in enemies2) //added for heavy enemy
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy2.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy2;
            }
        }

        foreach (GameObject enemy3 in enemies3) //added for heavy enemy (S)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy3.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy3;
            }
        }

        foreach (GameObject enemy4 in enemies4) //added for heavy enemy (M)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy4.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy4;
            }
        }

        foreach (GameObject enemy5 in enemies5) //added for secret Enemy
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy5.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy5;
            }
        }

        {
            if (closestEnemy != null && shortestDistance <= range)
            {
                target = closestEnemy.transform;
                isAttacking = true; //setting it to true when attacking
            }
            else
            {
                target = null;
                isAttacking = false; //seetting in back to false when not attacking
            }
        }

    }


    private void Update()
    {
        if (target == null)
        {
            if (usetracker)
            {
                if (linerenderer.enabled)
                    linerenderer.enabled = false;

            }
            anim.SetBool("isAttacking", false);
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
        a_Audio.Play();

        anim.SetBool("isAttacking", true);

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, bulletspawn.position, bulletspawn.rotation);
        NS_Bullet bullet = bulletGO.GetComponent<NS_Bullet>();

        if (shootObject == true)
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
