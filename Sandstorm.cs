using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm : MonoBehaviour
{
    public float SandTimer = 60.0f;
    public bool Use = false;
    public GameObject Sandeffect;
    //public GameObject ssTimer;
    


    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    

    private void Update()
    {
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != null)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("HeavyEnemy") || other.gameObject.CompareTag("HeavyEnemy(S)") || other.gameObject.CompareTag("HeavyEnemy(M)") || other.gameObject.CompareTag("SecretEnemy"))
            {
                if (other.GetComponent<NS_EnemyMovement>() == true) //sandstorm null reference fix 
                {
                    other.GetComponent<NS_EnemyMovement>().DeathRayDamage = 2;
                    StartCoroutine(other.GetComponent<NS_EnemyMovement>().SlowlyHarm());
                }
            }

                if (other.gameObject.CompareTag("PhoenixBase"))
                {
                if (other.GetComponent<PhoenixBase>() == true)
                    other.GetComponent<PhoenixBase>().TakeDamage(5);
                     // StartCoroutine(other.GetComponent<PhoenixBase>().SlowlyHarm());
                }

                if (other.gameObject.CompareTag("Turret"))
                {
                  if (other.GetComponent<NS_Turret>() == true)
                      other.GetComponent<NS_Turret>().TakeDamage(20);
               // StartCoroutine(other.GetComponent<NS_Turret>().SlowlyHarm());
                } 

        } 
    }

  

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PhoenixBase"))
        {
            Debug.Log("Exited");
        }
    }


}
