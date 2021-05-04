using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_ElectricalTrap : MonoBehaviour
{
    public int TimesUsed = 0;
    public AudioSource a_Audio;
    public AudioClip destroyed;
    public AudioClip damage;
    private Renderer rend;
    public Color DamageColor;
    private Color startColor;
    public GameObject particleEffect;

    private void Start()
    {
        a_Audio = GameObject.Find("SceneManager").GetComponent<AudioSource>();

      //  rend = GetComponent<Renderer>();
       // startColor = rend.material.color;

    }

    void Update()
    {
        if (TimesUsed >= 10)
        {
            a_Audio.clip = destroyed;
            a_Audio.PlayOneShot(destroyed);
            Destroy(gameObject,0.1f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "HeavyEnemy(S)" || other.gameObject.tag == "HeavyEnemy(M)" || other.gameObject.tag == "HeavyEnemy" || other.gameObject.tag == "SecretEnemy")
        {
            other.GetComponent<NS_EnemyMovement>().TakeDamage(65);
            a_Audio.clip = damage;
            a_Audio.PlayOneShot(damage);
            GameObject _particleEffect = (GameObject)Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(_particleEffect, 1f);
            //  StartCoroutine("Flash", 1f);
            TimesUsed++;
        }
    }

  /*  IEnumerator Flash()
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
    } */
}
