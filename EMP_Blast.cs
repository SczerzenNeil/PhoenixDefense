using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Blast : MonoBehaviour
{
    public ParticleSystem Pa;
    public float disableRadius;
    public GameObject Timer;
    

    // Start is called before the first frame update
    void Start()
    {
        Timer = FindObjectOfType<EMP_Timer>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer.GetComponent<EMP_Timer>().EMPeffect < 15)
            DisableNodes();
        if (Timer.GetComponent<EMP_Timer>().EMPeffect < .1f)
            EnableNodes();
    }

    public void DisableNodes()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, disableRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Node")
            {
                collider.GetComponent<NS_Node>().notActive = true;
            }
        }
    }

    public void EnableNodes()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, disableRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Node")
            {
                collider.GetComponent<NS_Node>().notActive = false;
                collider.gameObject.GetComponent<Renderer>().material = collider.gameObject.GetComponent<NS_Node>().startMat;
            }
        }
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Node"))
        {
            other.GetComponent<NS_Node>().notActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Node"))
        {
            other.GetComponent<NS_Node>().notActive = false;
        }
    } */

    /*public IEnumerator BlastEffect()
    {
        Pa.Play();
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<NS_Node>().notActive = true;
        yield return new WaitForSecondsRealtime(5);
        Pa.Stop();
        gameObject.GetComponent<NS_Node>().notActive = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        isActive = false;
    }*/
}
