using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NS_TrapText : MonoBehaviour
{
    public Text MineCost, BlockadeCost, ElectricalCost, FreezeCost;

    public TrapShop Trap;

    private void Start()
    {
        MineCost.text = "$" + Trap.mine.cost;
        BlockadeCost.text = "$" + Trap.blockade.cost;
        ElectricalCost.text = "$" + Trap.Electrical.cost;
        FreezeCost.text = "$" + Trap.freeze.cost;

    }
}
