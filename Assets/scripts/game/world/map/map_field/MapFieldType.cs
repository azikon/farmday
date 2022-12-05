using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MapFieldType
{
	//BuildingData data { get; set; }
	//System.Object BuildingTypeObject { get; set; }

	//TimeAction buildingTimeAction { get; set; }

	Dictionary<string, InventoryData> inventoryInProduction { get; set; }
	Dictionary<string, InventoryData> inventoryInReady { get; set; }
	InventoryData inventoryCurrent { get; set; }

	GameObject buildingRoot { get; set; }
	GameObject buildingObject { get; set; }

	void Initialization();

	void OnClick();

	void OnPress( bool pressed );

	void OnDragStart();

	void OnDrag( Vector2 delta );

	void OnDragEnd();

	void InventoryAdd( InventoryData inventoryItem );

	void InventoryRemove();

	//CellVector GetCellPosition { get; }
}