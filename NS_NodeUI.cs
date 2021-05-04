using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NS_NodeUI : MonoBehaviour
{
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Text sellAmount;
    public Text repairAmount;
    public GameObject rangeDisplay;
    public Vector3 targetRange;
    
    private NS_Node target;
    public Image healthBar;

    public void Update()
    {
        // target = _target;
    }
    public void SetTarget (NS_Node _target)
    {
        
        target = _target;

        transform.position = target.GetBuildPosition();

        if (!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradecost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "MAX";
            upgradeButton.interactable = false;
        }

        targetRange = new Vector3(target.turretBlueprint.range * 2, target.turretBlueprint.range * 2, 0);
        Debug.Log("normalrange");

        if(target.isUpgraded == true)
        {
            targetRange = new Vector3(target.turretBlueprint.upgradedRange * 2, target.turretBlueprint.upgradedRange * 2, 0);
            Debug.Log("upgradedrange");
        }

        healthBar.fillAmount = target.turretHealth;
        Debug.Log("health " + target.turretHealth);

        if(target.isUpgraded == true)
            sellAmount.text = "$" + target.turretBlueprint.GetSellAmount() * 2;
        else
        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        if (target.turretHealth < .5f)
            repairAmount.text = "$" + target.turretBlueprint.GetRepairAmount() * 2;
        else 
        repairAmount.text = "$" + target.turretBlueprint.GetRepairAmount();

        ui.SetActive(true);
        rangeDisplay.SetActive(true);
        rangeDisplay.transform.localScale  = targetRange;
    }

    public void Hide()
    {
        ui.SetActive(false);
        rangeDisplay.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        NS_BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        NS_BuildManager.instance.DeselectNode();
    }

    public void Repair()
    {
        target.RepairTurret();
        NS_BuildManager.instance.DeselectNode();
    }
}
