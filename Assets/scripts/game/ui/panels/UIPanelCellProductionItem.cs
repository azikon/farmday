using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIPanelCellProductionItem : MonoBehaviour
{
    public MapFieldData MapFieldData;
    public Image ItemIcon;
    public MapCell MapCell;

    public void OnClick()
    {
        UIHandler.Instance.UIGet( "UIPanelCellProduction" ).Hide();

        CharacterHandler.Instance.CurrentCharacter.SetDestination( () => { MapHandler.Instance.AddFieldOnMap( MapCell, MapFieldData ); MapHandler.Instance.UpdateNavMesh(); }, MapCell.transform.position );
    }
}