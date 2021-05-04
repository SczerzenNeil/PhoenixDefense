using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_BuildManager : MonoBehaviour
{
    public static NS_BuildManager instance;
    public NS_NodeUI nodeUI;
    public bool _canBuild;

    void Awake()
    {
        instance = this;
    }
    public MB_MoneySystem money;
    public GameObject StandardturretPrefab;
    public GameObject MissleLauncherPrefab;
    public GameObject HeavyTurretPrefab;
    public GameObject FieldTurretPrefab;
    public GameObject CurrencyHutPrefab;

    public TurretBlueprint turretToBuild;
    private NS_Node selectedNode;
    
    public void Start()
    {
        money = GetComponent<MB_MoneySystem>();
    }
    public void SelectNode (NS_Node node)
    {
        
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }
    
    /*public void toggleInput()
    {
        if(_canBuild)
            public bool CanBuild { get { return turretToBuild != null; } }
    }*/
    //I tried the function above with line 49 disabled, but this completely breaks this script. I also tried accessing the CanBuild bool from NS_Node it gives an error saying it wants {get;} but gives an error either with that as well.
 
    
    public bool CanBuild { get { return turretToBuild != null; } }
  

    public void DeselectNode() //call this function when selecting other things, aka traps
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild (TurretBlueprint turret)
    {
        turretToBuild = turret;
        selectedNode = null;

        DeselectNode();
    }

    public TurretBlueprint GetTuuretToBuild()
    {
        return turretToBuild;
    }
}
