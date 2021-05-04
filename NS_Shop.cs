using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_Shop : MonoBehaviour
{
    public TurretBlueprint standartTurret;
    public TurretBlueprint misslelauncher;
    public TurretBlueprint heavyturret;
    public TurretBlueprint fieldturret;
    public TurretBlueprint currencyHut;
    public TurretBlueprint ReconBuilding;

    NS_BuildManager BuildManager;

    private void Start()
    {
        BuildManager = NS_BuildManager.instance;
    }
   
    public void SelectStandardTurret()  //PurchaseStandardTurret
    {
        Debug.Log("Standard Turret Selected");
        BuildManager.SelectTurretToBuild(standartTurret); //BuildManager.SetTurretToBuild(BuildManager.StandardturretPrefab);
    }

    public void SelectMissleLauncher()
    {
        Debug.Log("Missle Launcher Selected");
        BuildManager.SelectTurretToBuild(misslelauncher);
    }

    public void SelectHeavyTurret()
    {
        Debug.Log("Heavy Turret Selected");
        BuildManager.SelectTurretToBuild(heavyturret);
    }

    public void SelectFieldTurret()
    {
        Debug.Log("Field Turret Selected");
        BuildManager.SelectTurretToBuild(fieldturret);
    }

    public void SelectCurrencyHut()
    {
        Debug.Log("Currency Hut Selected");
        BuildManager.SelectTurretToBuild(currencyHut);
    }

    public void SelectReconBuilding()
    {
        Debug.Log("Recon Building Selected");
        BuildManager.SelectTurretToBuild(ReconBuilding);
    }

}
