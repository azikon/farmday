using UnityEngine;
using System.Collections;

public class TimeActionData
{
	public TimeAction timeAction;
	public InventoryData inventoryData;
	public System.Action<System.Object> objectUpdateMethod;
	public System.Action<System.Object> objectReturnMethod;
	public string timeActionType;
}