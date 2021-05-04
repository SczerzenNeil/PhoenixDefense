using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NS_TurretShopTexts : MonoBehaviour
{
    public Text turretCost, MissleCost, HeavyCost, HutCost, FieldCost, ReconCost;

    public NS_Shop Turret;

    private void Start()
    {
        turretCost.text = "$" + Turret.standartTurret.cost;
        MissleCost.text = "$" + Turret.misslelauncher.cost;
        HeavyCost.text = "$" + Turret.heavyturret.cost;
        HutCost.text = "$" + Turret.currencyHut.cost;
        FieldCost.text = "$" + Turret.fieldturret.cost;
        ReconCost.text = "$" + Turret.ReconBuilding.cost;
    }
}
