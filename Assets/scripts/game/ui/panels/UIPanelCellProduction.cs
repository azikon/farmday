using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UIPanelCellProduction : UIBase
{
    public GameObject PrefabItem;
    public GameObject ItemsParent;

    public List<Sprite> _loadedSprites = new List<Sprite>();

    public override void LoadData()
    {
        base.LoadData();

        _loadedSprites = Resources.LoadAll<Sprite>( "sprites/ui/" ).ToList();
    }

    public override void Show<T>( T cell )
    {
        base.Show();

        UIHandler.Instance.BaseHideAll();

        UIHandler.Instance.BaseShow( this, true );

        ( UIPanel.transform as RectTransform ).localPosition = new Vector3( 200f, 25f, 0f );

        MapCell mapCell = cell as MapCell;
        CameraHandler.Instance.EnvironmentCameraController.CameraRepositionToPoint( mapCell.transform.position );
        CreateItems( mapCell );
    }

    public override void Hide()
    {
        ClearItems();
        base.Hide();
        UIHandler.Instance.BaseHide( this );
    }

    public void CreateItems( MapCell mapCell )
    {
        ClearItems();
        List<MapFieldData> datas = MapHandler.Instance.MapFieldsData.Values.ToList();
        for ( int i = 0; i < datas.Count; i++ )
        {
            GameObject itemObject = Instantiate( PrefabItem );
            itemObject.transform.SetParent( ItemsParent.transform, false );
            ( itemObject.transform as RectTransform ).localPosition = new Vector3( 125 * i, 0, 0 );

            UIPanelCellProductionItem productionItem = itemObject.GetComponent<UIPanelCellProductionItem>();
            productionItem.MapFieldData = datas[ i ];
            productionItem.ItemIcon.sprite = _loadedSprites.Where( a => a.name == datas[ i ].MapFieldContentData.ICON ).First();
            productionItem.ItemIcon.SetNormalizedSize( 70 );
            productionItem.MapCell = mapCell;
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