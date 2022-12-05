using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UIPanelFieldProduction : UIBase
{
    public GameObject PrefabItem;
    public GameObject ItemsParent;

    public List<Sprite> _loadedSprites = new List<Sprite>();

    public override void LoadData()
    {
        base.LoadData();

        _loadedSprites = Resources.LoadAll<Sprite>( "sprites/ui/" ).ToList();
    }

    public override void Show<T>( T mapField )
    {
        base.Show();

        if ( mapField.GetType() == typeof( MapFieldPlant ) )
        {
            UIHandler.Instance.BaseHideAll();

            UIHandler.Instance.BaseShow( this, true );

            ( UIPanel.transform as RectTransform ).localPosition = new Vector3( 200f, 25f, 0f );

            MapFieldPlant fieldPlant = mapField as MapFieldPlant;
            CameraHandler.Instance.EnvironmentCameraController.CameraRepositionToPoint( fieldPlant.transform.position );
            CreateItems( fieldPlant );
        }
    }

    public override void Hide()
    {
        ClearItems();
        base.Hide();
        UIHandler.Instance.BaseHide( this );
    }

    public void CreateItems( MapField mapField )
    {
        ClearItems();
        List<InventoryData> datas = InventoryHandler.Instance.InventoriesData.Values.ToList();
        for ( int i = 0; i < datas.Count; i++ )
        {
            GameObject itemObject = Instantiate( PrefabItem );
            itemObject.transform.SetParent( ItemsParent.transform, false );
            ( itemObject.transform as RectTransform ).localPosition = new Vector3( 125 * i, 0, 0 );

            UIPanelFieldProductionItem productionItem = itemObject.GetComponent<UIPanelFieldProductionItem>();
            productionItem.InventoryData = datas[ i ];
            productionItem.ItemIcon.sprite = _loadedSprites.Where( a => a.name == datas[ i ].InventoryContentData.ICON ).First();
            productionItem.ItemIcon.SetNormalizedSize( 70 );
            productionItem.MapField = mapField;
        }
    }

    private void ClearItems()
    {
        foreach ( Transform child in ItemsParent.transform )
        {
            Destroy( child.gameObject );
        }
    }
}