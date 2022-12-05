using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIPanelFieldProductionItem : MonoBehaviour
{
	public InventoryData InventoryData;
	public Image ItemIcon;
	public MapField MapField;

	public void OnClick()
    {
        UIHandler.Instance.UIGet( "UIPanelFieldProduction" ).Hide();

        CharacterHandler.Instance.CurrentCharacter.SetDestination( () => { MapField.AddProductionItem( InventoryData ); }, MapField.transform.position );
	}
}