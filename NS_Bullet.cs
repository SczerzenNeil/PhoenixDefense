using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 50f;
    public GameObject PartilceEffect;
    public float splashRadius = 0f;
    public int damage = 50;
    public bool isHeavyBullet = false;
    public bool isEnhanced = false;
    public bool isShootingObject = false;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

       if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        } 

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        
        GameObject particle = (GameObject)Instantiate(PartilceEffect, transform.position, transform.rotation);
        Destroy(particle, 4f);

        if (splashRadius > 0f)
        {
            Explode();
        }
        else if (isHeavyBullet == true)
        {
            SlowDown(target);
        } 
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    } 

    void SlowDown(Transform enemy)
    {
        if (enemy.GetComponent<NS_EnemyMovement>() == false)
        {
            DestructibleObjHealth healthh = enemy.GetComponent<DestructibleObjHealth>();
            healthh.Damaged(damage);
            return;
        }

        NS_EnemyMovement health = enemy.GetComponent<NS_EnemyMovement>();
        health.TakeDamage(damage);
        health.speed = health.speed / 1.4f;
    }


    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, splashRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy" || collider.tag == "HeavyEnemy(M)" || collider.tag == "DesObjHealth")
            {
                Damage(collider.transform);
            }
        }
    }
    void Damage (Transform enemy) 
    {
        NS_EnemyMovement health = enemy.GetComponent<NS_EnemyMovement>();

        if (enemy.GetComponent<NS_EnemyMovement>() == false)
        {
            DestructibleObjHealth healthh = enemy.GetComponent<DestructibleObjHealth>();
            healthh.Damaged(damage);
            return;
        }

        health.TakeDamage(damage);
    }
   public void DamageObject(Transform enemy)
    {
       DestructibleObjHealth health = enemy.GetComponent<DestructibleObjHealth>();
       health.Damaged(damage);
    } 
  
}
