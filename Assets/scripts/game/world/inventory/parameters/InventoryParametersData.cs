using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Inventory Parameters", menuName = "FL Inventory Parameters", order = 51 )]
public class InventoryParametersData : ScriptableObject
{
    [SerializeField]
    public List<InventoryData> InventoriesType;
}