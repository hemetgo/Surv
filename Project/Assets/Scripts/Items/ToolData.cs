using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool", fileName = "New Tool")]
public class ToolData : ItemData
{
    [Header("Tool")]
    public ToolType toolType;
    public int durability;

    public enum ToolType { Axe, Pickaxe, Shovel, Hoe }

}
