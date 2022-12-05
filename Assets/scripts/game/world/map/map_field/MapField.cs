using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class MapField : MonoBehaviour
{
    public bool HasAddProductionItem { get; set; }
    public bool ProductionItemCompleted { get; set; }
    public bool IsSelected { get; set; }


    public InventoryData InventoryData { get; set; }

    public GameObject InventoryInProductionParent;
    public GameObject InventoryCompleteParent;

    public TimeAction TimeAction { get; set; }

    public List<cakeslice.Outline> EffectsOutline;

    public MapFieldData MapFieldData { get; set; }


    public void Initialization()
    {
        EffectsOutline = GetComponentsInChildren<cakeslice.Outline>().ToList();

        InventoryInProductionParent = gameObject.GetChildWithName( "inventory_in_production" );
        InventoryCompleteParent = gameObject.GetChildWithName( "inventory_complete" );
    }

    public void OnPress( bool pressed )
    {

    }

    public void OnClick()
    {

    }

    public void OnSelect( bool selected )
    {
        IsSelected = selected;
        SetOutlineIsShow( selected );
    }


    public void OnHover( bool state )
    {
        if ( IsSelected == false )
        {
            SetOutlineIsShow( state );
        }
    }

    public virtual void AddProductionItem( InventoryData inventoryData )
    {
    }

    public virtual void RemoveProductionItem( InventoryData inventoryData )
    {
    }

    public virtual void ProductionItemComplete( System.Object obj )
    {
    }

    public virtual void CreateInventoryState( string state, GameObject parent )
    {
    }

    public virtual void RemoveInventoryState( GameObject parent )
    {
    }

    private void SetOutlineIsShow( bool isShow )
    {
        if ( isShow == true )
        {
            EffectsOutline.ForEach( x => x.OutlineShow() );
        }
        else
        {
            EffectsOutline.ForEach( x => x.OutlineHide() );
        }
    }
}