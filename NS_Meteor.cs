using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_Meteor : MonoBehaviour
{
    public float damageRadius = 10;
    public int meteorDamage = 50;
    public GameObject ExplodeParticle;
    public AudioSource a_Audio;
    public AudioClip explode;


    public void Start()
    {
        a_Audio = GameObject.Find("SceneManager").GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Node" || other.gameObject.tag == "EnemyPathNode")
        {
            Explode();

            GameObject effect = (GameObject)Instantiate(ExplodeParticle, transform.position, Quaternion.identity);
            Destroy(effect, 3f);

            a_Audio.clip = explode;
            a_Audio.PlayOneShot(explode);

            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Turret")
            {
                collider.GetComponent<NS_Turret>().TakeDamage(meteorDamage);
            }
        }

    }
}
