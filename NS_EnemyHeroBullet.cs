using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_EnemyHeroBullet : MonoBehaviour
{
    public Transform target;
    public float speed = 50f;
    public GameObject PartilceEffect;
    public string HeroTag = "Hero";
    public GameObject Father;
    public NWB_AudioManager audioManager;


    public void Start()
    {
        audioManager = FindObjectOfType<NWB_AudioManager>();
    }


    void Update()
    {
        GameObject Hero = GameObject.FindGameObjectWithTag(HeroTag);
        if (Hero != null)
        {
            target = Hero.transform;
        }

        if (target == null || Hero == null)
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
        HeroHealth health = enemy.GetComponent<HeroHealth>();
        {
            health.Damaged(10);
            audioManager.PlayHeroHurt();
        }
    }
}
