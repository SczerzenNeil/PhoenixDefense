using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradecost;

    public int GetSellAmount ()
    {
        return cost / 3;
    }

    public int GetRepairAmount()
    {
        return cost / 2;
    }

    public int range;
    public int upgradedRange;
}
