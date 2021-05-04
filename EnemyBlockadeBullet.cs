using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockadeBullet : MonoBehaviour
{
    public Transform target;
    public float speed = 50f;
    public GameObject PartilceEffect;
    public string blockadeTag = "Blockade";
    public GameObject Father;
    public float range = 10;


    public void Start()
    {
       // InvokeRepeating("UpdateTarget", 0f, 0f);
    }


    void Update()
    {
        UpdateTarget();

       // GameObject Blockade = GameObject.FindGameObjectWithTag(blockadeTag);
       
       /* if(target != null)
        {
            target = Blockade.transform;
        } */

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
        NWB_EnemyHealth health = enemy.GetComponent<NWB_EnemyHealth>();
        {
            health.Damaged(20);
        }
    }

    void UpdateTarget()
    {
        GameObject[] blockades = GameObject.FindGameObjectsWithTag(blockadeTag);
        float shortestDistance = Mathf.Infinity;
        GameObject closestBlockade = null;

        foreach (GameObject turret in blockades)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, turret.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestBlockade = turret;
            }
        }

        {
            if (closestBlockade != null && shortestDistance <= range)
            {
                target = closestBlockade.transform;
            }
            else
            {
                target = null;
            }
        }
    }
}
