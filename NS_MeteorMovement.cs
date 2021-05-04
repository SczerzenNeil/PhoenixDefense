using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_MeteorMovement : MonoBehaviour
{
    public float speed = 3f;
    public Transform target;
    private Vector3 turretLocation;
    private int waypointIndex = 0;
    private string turretTag = "Turret";
    public int turretIndex;
    //GameObject[] Turrets;
    public List<GameObject> Turrets;

    private void Start()
    {
      //  target = NS_MeteorWaypoints.waypoints[0];
    }

    public void selectTurret()
    {
        turretIndex = Random.Range(0, Turrets.Count);
    }

    /*
     * Out of Index error was fixed by me
     * adding the turret to the list when
     * the turret spawns and removing it
     * when the turret destroys itself
     * or being sold.
     * 
     * Scripts involved in bug fix: NS_MeteorMovement, NS_Turret, and NS_Node
     * 
     * In this script, I converted the
     * array into a list and then added
     * in a check to see if the index is
     * null, if it is then it will choose
     * a new turret and remove the faulty
     * one.
     * 
     *                  ~ Matthew :)
     * 
     */

    private void Update()
    {
        var TurretsFound = GameObject.FindGameObjectsWithTag(turretTag);

        //foreach(GameObject turret in TurretsFound)
        //{
        //    Turrets.Add(turret);
        //}

        if (Turrets.Count > 0)
        {
            if (target == null)
                selectTurret();

            if(Turrets[turretIndex].gameObject == null) // Does the turret not exist?
            {
                Turrets.Remove(Turrets[turretIndex]); // Then remove it.
                selectTurret(); // Add this or else it will out of index error
            }

            target = Turrets[turretIndex].transform;
            turretLocation = new Vector3(target.position.x, 140, target.position.z);

            if (target == null)
                selectTurret();

            Vector3 _dir = turretLocation - transform.position;
            transform.Translate(_dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, turretLocation) <= 0.1f)
            {
                selectTurret();
                return;
            }
        }

        else 
        {
            target = NS_MeteorWaypoints.waypoints[waypointIndex];

            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);


            if (Vector3.Distance(transform.position, target.position) <= 0.4f)
            {
                GetNextWaypoint();
            }
            return;
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex == NS_MeteorWaypoints.waypoints.Length - 1)
        {
            waypointIndex = 0;
            return;
        }
        waypointIndex++;
        
    }
}
