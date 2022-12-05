using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class MapHandler : HandlerBase<MapHandler>
{
    public Dictionary<MapCellObject, List<MapCellPosition>> MapCellObjects;

    public GameObject PrefabMap;
    public GameObject PrefabMapCell;
    public GameObject PrefabMapField;

    public GameObject MapParent;
    public Map CurrentMap;

    private int MapCellsHorizontalCount;
    private int MapCellsVerticalCount;

    public Dictionary<string, MapFieldData> MapFieldsData { get; set; }


    public Dictionary<string, MapField> BuildingsOnMap;

    public MapParametersData MapParametersData;
    public MapFieldParametersData MapFieldParametersData;

    public void SetMapCellsCount( int horizontalCount, int verticalCount )
    {
        MapCellsHorizontalCount = horizontalCount;
        MapCellsVerticalCount = verticalCount;
    }

    public override void LoadData()
    {
        MapFieldsData = new Dictionary<string, MapFieldData>();
        for ( int i = 0; i < MapFieldParametersData.MapFieldsType.Count; i++ )
        {
            MapFieldsData.Add( i.ToString(), MapFieldParametersData.MapFieldsType[ i ] );
        }

        //MapFieldsData = new Dictionary<string, MapFieldData>
        //{
        //    {
        //        "0",
        //        new MapFieldData
        //        {
        //            ID = "0001",
        //            Name = "Field",
        //            Description = "Produce on field",
        //            Type = "FIELD_FIELD",
        //            ForType = "PRODUCTION_CELL",
        //            Count = 1,
        //            ProductionDurationInSeconds = 0,
        //            MapFieldContentData = new MapFieldContentData
        //            {
        //                STATE_PRODUCTION = "",
        //                STATE_COMPLETE = "prod_field_field_in_complete",
        //                ICON = "ui_icon_3"
        //            }
        //        }
        //    },
        //    {
        //        "1",
        //        new MapFieldData
        //        {
        //            ID = "0002",
        //            Name = "Tree",
        //            Description = "Produce on tree",
        //            Type = "FIELD_TREE",
        //            ForType = "PRODUCTION_CELL",
        //            Count = 1,
        //            ProductionDurationInSeconds = 0,
        //            MapFieldContentData = new MapFieldContentData
        //            {
        //                STATE_PRODUCTION = "",
        //                STATE_COMPLETE = "prod_field_tree_in_complete",
        //                ICON = "ui_icon_2"
        //            }
        //        }
        //    }
        //};
    }

    public void LoadMap()
    {
        GameObject mapObject = Instantiate( PrefabMap );
        mapObject.transform.SetParent( MapParent.transform, false );

        Map map = mapObject.transform.GetComponent<Map>();
        CurrentMap = map;

        CreateMapCells( map );
    }

    private void CreateMapCells( Map map )
    {
        for ( int i = 0; i < 28; i++ )
        {
            for ( int j = 0; j < 35; j++ )
            {
                GameObject cellObject = Instantiate( PrefabMapCell );
                cellObject.transform.SetParent( map.CellsParent.transform, false );
                cellObject.transform.position = new Vector3( 10 * i, 0, 10 * j );

                MapCell mapCell = cellObject.GetComponent<MapCell>();
                mapCell.Initialization();
                if ( ( i > 3 && i < MapCellsHorizontalCount - 1 ) && ( j > 3 && j < MapCellsVerticalCount - 1 ) )
                {
                    //mapCell.IsStaticOnMap = false;

                    AddFieldOnMap( mapCell, MapFieldsData[ "0" ] );
                }
            }
        }
        BuildNavMesh();
    }

    public void BuildNavMesh()
    {
        this.Delay( () =>
        {
            CurrentMap.NavMeshSurface.BuildNavMesh();
        }, 0.1f );
    }

    public void UpdateNavMesh()
    {
        this.Delay( () =>
        {
            NavMeshData navMeshData = CurrentMap.NavMeshSurface.navMeshData;
            CurrentMap.NavMeshSurface.UpdateNavMesh( navMeshData );
        }, 0.1f );
    }

    public void AddFieldOnMap( MapCell mapCell, MapFieldData mapFieldData )
    {
        mapCell.HasAddProductionItem = true;

        GameObject fieldObject = Instantiate( PrefabMapField );
        fieldObject.transform.SetParent( CurrentMap.FieldsParent.transform, false );
        fieldObject.transform.position = new Vector3( mapCell.transform.position.x, 0, mapCell.transform.position.z );

        if ( mapFieldData.ProductionDurationInSeconds < 1L )
        {
            CreateFieldState( mapFieldData.MapFieldContentData.STATE_COMPLETE, fieldObject.GetChildWithName( "field" ), new Vector3( mapCell.transform.position.x, 0, mapCell.transform.position.z ) );
        }

        if ( mapFieldData.Type == "FIELD_FIELD" )
        {
            MapFieldPlant field = fieldObject.AddComponent<MapFieldPlant>();
            field.Initialization();
        }
        else if ( mapFieldData.Type == "FIELD_TREE" )
        {
            MapFieldTree field = fieldObject.AddComponent<MapFieldTree>();
            field.Initialization();
        }
    }

    public void CreateFieldState( string state, GameObject parent, Vector3 position )
    {
        GameObject itemInProduction = Instantiate( Resources.Load<GameObject>( "prefabs/environment/fields/" + state ) );
        itemInProduction.transform.SetParent( parent.transform, false );
        itemInProduction.transform.localPosition = Vector3.zero;
    }
}