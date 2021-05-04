using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretBullet : MonoBehaviour
{
    public float range = 5;
    public string turretTag = "Turret";
    public Transform target;
    public GameObject PartilceEffect;
    public float speed = 5;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        //add sfx
        GameObject particle = (GameObject)Instantiate(PartilceEffect, transform.position, transform.rotation);
        Destroy(particle, 2f);


        Damage(target);

        Destroy(gameObject);
    }

    void Damage(Transform enemy) //gets access to enemy's health
    {
        NS_Turret health = enemy.GetComponent<NS_Turret>();
        {
            health.TakeDamage(10);
        }
    }

    void UpdateTarget()
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
                    target = closestTurret.transform;
                }
                else
                {
                    target = null;
                }
            }
    }
}
