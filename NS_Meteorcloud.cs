using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_Meteorcloud : MonoBehaviour
{
    public GameObject meteor;
    public Vector3 bottom;
    public AudioSource a_Audio;
    public AudioClip meteorFall;
    public float fallTime;

    public void Start()
    {
        a_Audio = GameObject.Find("SceneManager").GetComponent<AudioSource>();
        InvokeRepeating("SpawnMeteor", fallTime, fallTime);
    }
    public void SpawnMeteor()
    {
        Instantiate(meteor, transform.position, Quaternion.identity);
        a_Audio.clip = meteorFall;
        a_Audio.PlayOneShot(meteorFall);
    }

    public void OnDrawGizmos()
    {
        bottom = new Vector3(transform.position.x, 0, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, bottom);
    }
}
