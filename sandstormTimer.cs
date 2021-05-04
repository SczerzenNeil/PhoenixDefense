using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sandstormTimer : MonoBehaviour
{
    public bool stormActive = false;
    public float cooldown = 7.0f;
    public bool canUse = true;
    public float heroTime = 10.0f;
    public GameObject Sandstorm;
    
    void Start()
    {
      
    }
    
    void Update()
    {
      
        if (!canUse && stormActive == true)
        {
            heroTime -= Time.deltaTime;
        }
        if (heroTime <= 0)
        {
            Sandstorm.SetActive(false);
            stormActive = false;
            heroTime = 10f;
        }
        if (stormActive == false)
        {
            cooldown -= Time.deltaTime;;
        }
        if (cooldown <= 0)
        {
            canUse = true;
            cooldown = 0f;
        }
    }
}
