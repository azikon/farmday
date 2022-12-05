using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InventoryHandler : HandlerBase<InventoryHandler>
{
    public Dictionary<string, InventoryData> InventoriesData { get; set; }

    public InventoryParametersData InventoryParametersData;

    public override void LoadData()
    {
        InventoriesData = new Dictionary<string, InventoryData>();
        for ( int i = 0; i < InventoryParametersData.InventoriesType.Count; i++ )
        {
            InventoriesData.Add( i.ToString(), InventoryParametersData.InventoriesType[ i ] );
        }

        //InventoriesData = new Dictionary<string, InventoryData>
        //{
        //    {
        //        "0",
        //        new InventoryData
        //        {
        //            ID = "0001",
        //            Name = "Carrot",
        //            Description = "Produce on field",
        //            ForType = "PRODUCTION_FIELD",
        //            Count = 1,
        //            ProductionDurationInSeconds = 10,
        //            InventoryContentData = new InventoryContentData
        //            {
        //                STATE_PRODUCTION = "prod_item_carrot_in_production",
        //                STATE_COMPLETE = "prod_item_carrot_in_complete",
        //                ICON = "ui_icon_0"
        //            }
        //        }
        //    },
        //    {
        //        "1",
        //        new InventoryData
        //        {
        //            ID = "0002",
        //            Name = "Grass",
        //            Description = "Produce on field",
        //            ForType = "PRODUCTION_FIELD",
        //            Count = 1,
        //            ProductionDurationInSeconds = 22,
        //            InventoryContentData = new InventoryContentData
        //            {
        //                STATE_PRODUCTION = "prod_item_grass_in_production",
        //                STATE_COMPLETE = "prod_item_grass_in_complete",
        //                ICON = "ui_icon_1"
        //            }
        //        }
        //    }
        //};
    }
}