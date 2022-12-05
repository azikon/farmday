using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapFieldPlant : MapField
{
    public new void Initialization()
    {
        base.Initialization();
    }

    public new void OnClick()
    {
        base.OnClick();

        if ( HasAddProductionItem == false )
        {
            UIHandler.Instance.UIGet( "UIPanelFieldProduction" ).Show( this );
            UIHandler.Instance.UIGet( "UIPanelProductionProgressBar" ).Hide();
        }
        else
        {
            if ( ProductionItemCompleted == true )
            {
                CameraHandler.Instance.EnvironmentCameraController.CameraRepositionToPoint( this.transform.position );
                UIHandler.Instance.UIGet( "UIPanelProductionProgressBar" ).Hide();
                CharacterHandler.Instance.CurrentCharacter.SetDestination( () =>
                {
                    ( UIHandler.Instance.UIGet( "UIPanelCharacterCarrot" ) as UIPanelCharacterCarrot ).AddCarrot( this );
                    MapHandler.Instance.Delay( () =>
                    {
                        ( UIHandler.Instance.UIGet( "UIPanelCharacterExperience" ) as UIPanelCharacterExperience ).AddExperience( this );
                    }, 0.1f );

                    RemoveProductionItem( InventoryData );
                }, this.transform.position );
            }
            else
            {
                UIHandler.Instance.UIGet( "UIPanelProductionProgressBar" ).Show( this );
                UIHandler.Instance.UIGet( "UIPanelFieldProduction" ).Hide();
            }
        }
    }

    public new void OnSelect( bool selected )
    {
        base.OnSelect( selected );
    }

    public override void AddProductionItem( InventoryData inventoryData )
    {
        if ( HasAddProductionItem == true )
        {
            return;
        }
        HasAddProductionItem = true;
        InventoryData = inventoryData;

        InventoryData.ProductionStartTimeInSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

        CreateInventoryState( InventoryData.InventoryContentData.STATE_PRODUCTION, InventoryInProductionParent );

        TimeAction = new TimeAction( TimeActionType.INVENTORY, ( int )inventoryData.ProductionDurationInSeconds, DateTime.Now );
        TimeActionHandler.Instance.TimeActionAdd( System.Guid.NewGuid().ToString(), TimeAction, inventoryData, ProductionItemComplete );

        UIHandler.Instance.UIGet( "UIPanelProductionProgressBar" ).Show( this );
        //UIHandler.Instance.Delay( () => { UIHandler.Instance.UIGet( "UIPanelProductionProgressBar" ).Show( this ); }, 0.05f );
    }

    public override void RemoveProductionItem( InventoryData inventoryData )
    {
        HasAddProductionItem = false;
        ProductionItemCompleted = false;
        TimeAction = null;
        RemoveInventoryState( InventoryInProductionParent );
    }



    public override void ProductionItemComplete( System.Object obj )
    {
        InventoryInProductionParent.DestroyAllChilds();
        CreateInventoryState( InventoryData.InventoryContentData.STATE_COMPLETE, InventoryCompleteParent );
        ProductionItemCompleted = true;
    }

    public override void CreateInventoryState( string state, GameObject parent )
    {
        GameObject itemInProduction = Instantiate( Resources.Load<GameObject>( "prefabs/environment/production/" + state ) );
        itemInProduction.transform.SetParent( parent.transform, false );
        itemInProduction.transform.localPosition = Vector3.zero;
    }

    public override void RemoveInventoryState( GameObject parent )
    {
        InventoryCompleteParent.DestroyAllChilds();
    }
}