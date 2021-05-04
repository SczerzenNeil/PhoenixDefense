using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_EnemyBullet : MonoBehaviour
{
    public Transform target;
    public float speed = 50f;
    public GameObject PartilceEffect;
    public string phoenixbaseTag = "PhoenixBase";
    public GameObject Father;
   

    public void Start()
    {
        
    }
    

    void Update()
    {
        GameObject phoenixbase = GameObject.FindGameObjectWithTag(phoenixbaseTag);
        target = phoenixbase.transform;

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
    //   if (Father == null)
         //  Destroy(gameObject);
        
       GameObject particle = (GameObject)Instantiate(PartilceEffect, transform.position, transform.rotation);
       Destroy(particle, 2f);
       
       Damage(target);
       Destroy(gameObject);
    } 
   
    void Damage(Transform enemy) //gets access to enemy's health
    {
        PhoenixBase health = enemy.GetComponent<PhoenixBase>();
        {
            if (Father != null)
            health.TakeDamage(Father.gameObject.GetComponent<NS_EnemyMovement>().Damage);


            health.TakeDamage(5);
        }
    }
}
