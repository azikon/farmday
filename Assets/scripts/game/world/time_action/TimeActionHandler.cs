using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class TimeActionHandler : HandlerBase<TimeActionHandler>
{
	public Dictionary<string, TimeActionData> timeActionsCache;

	public override void LoadData()
	{
		timeActionsCache = new Dictionary<string, TimeActionData>();
	}

	public void TimeActionAdd( string UID, TimeAction objectTimeAction, InventoryData inventoryItem, System.Action<System.Object> objectReturnMethod )
	{
		if ( timeActionsCache.ContainsKey( UID ) == true )
		{ return; }
		TimeActionData timeActionData = new TimeActionData
		{
			timeAction = objectTimeAction,
			inventoryData = inventoryItem,
			objectReturnMethod = objectReturnMethod,
			timeActionType = "TIME_ACTION_TYPE_NONE"
		};
		timeActionsCache.Add( UID, timeActionData );
	}

	public void TimeActionAdd( string UID, TimeAction objectTimeAction, InventoryData inventoryItem, System.Action<System.Object> objectReturnMethod, System.Action<System.Object> objectUpdateMethod )
	{
		if ( timeActionsCache.ContainsKey( UID ) == true )
		{ return; }
		TimeActionData timeActionData = new TimeActionData
		{
			timeAction = objectTimeAction,
			inventoryData = inventoryItem,
			objectReturnMethod = objectReturnMethod,
			objectUpdateMethod = objectUpdateMethod,
			timeActionType = "TIME_ACTION_TYPE_NONE"
		};
		timeActionsCache.Add( UID, timeActionData );
	}

	public void TimeActionAdd( string UID, TimeAction objectTimeAction, InventoryData inventoryItem, System.Action<System.Object> objectReturnMethod, System.Action<System.Object> objectUpdateMethod, string timeActionType )
	{
		if ( timeActionsCache.ContainsKey( UID ) == true )
		{ return; }
		TimeActionData timeActionData = new TimeActionData
		{
			timeAction = objectTimeAction,
			inventoryData = inventoryItem,
			objectReturnMethod = objectReturnMethod,
			objectUpdateMethod = objectUpdateMethod,
			timeActionType = timeActionType
		};
		timeActionsCache.Add( UID, timeActionData );
	}

	public void TimeActionRemove( string UID )
	{
		if ( timeActionsCache.ContainsKey( UID ) == true )
		{
			timeActionsCache.Remove( UID );
		}
	}

	public void TimeActionReportProgress( string UID, TimeActionData timeActionData )
	{
		if ( timeActionsCache.ContainsKey( UID ) == false )
		{ return; }
		if ( timeActionData.timeActionType == "TIME_ACTION_TYPE_INVENTORY" )
		{
			timeActionData.objectReturnMethod( timeActionData.inventoryData );
		}
		else if ( timeActionData.timeActionType == "TIME_ACTION_TYPE_NONE" )
		{
			timeActionData.objectReturnMethod( timeActionData.inventoryData );
		}
		else if ( timeActionData.timeActionType == "TIME_ACTION_TYPE_BUILDING" )
		{
			timeActionData.objectReturnMethod( timeActionData );
		}
		if ( timeActionsCache[ UID ].inventoryData != null )
		{
		}
		timeActionsCache.Remove( UID );
	}

	public void DoUpdateFrame()
	{
		if( IsInited == false )
		{
			return;
		}
		string[] UIDs = timeActionsCache.Keys.ToArray();
		TimeActionData[] timeActionsData = timeActionsCache.Values.ToArray();
		for ( int i = 0; i < timeActionsCache.Count; i++ )
		{
			TimeActionData timeActionData = timeActionsData[ i ];
			TimeAction objectTimeAction = timeActionData.timeAction;
			InventoryData inventoryData = timeActionData.inventoryData;
			System.Action<System.Object> objectReturnMethod = timeActionData.objectReturnMethod;
			timeActionData.objectUpdateMethod?.Invoke( objectTimeAction );
			if ( objectTimeAction.PercentageComplete < 1.0f && objectTimeAction.SpeedUp == false )
			{
			}
			else
			{
				TimeActionReportProgress( UIDs[ i ], timeActionData );
			}
		}
	}

	public TimeAction GetTimeActionByUID( string UID )
	{
		TimeAction timeAction = null;
		if ( timeActionsCache.ContainsKey( UID ) == true )
		{
			timeAction = timeActionsCache[ UID ].timeAction;
		}
		return timeAction;
	}

	public string TimeToString( System.TimeSpan time )
	{
		StringBuilder builder = new StringBuilder();

		int hours = time.Days * 24 + time.Hours;
		if ( hours == 0 && time.Minutes == 0 && time.Seconds == 0 )
		{
			builder.Append( "Complete" );
		}
		else
		{
			if ( hours != 0 )
			{
				builder.Append( hours + " hr. " );
			}
			if ( time.Minutes != 0 )
			{
				builder.Append( time.Minutes + " min. " );
			}
			if ( time.Seconds != 0 )
			{
				builder.Append( time.Seconds + " sec. " );
			}
		}
		return builder.ToString();
	}

	public string GetTimeText( string type, string forType )
	{
		string result;
		if ( ( forType == "PLANT" && type == "TYPE_SILO" ) || ( forType == "PROD_TREE" && type != "DESTROYER" ) )
		{
			result = "Grow";
		}
		else if ( type == "TYPE_PRODUCTION_BUILDING" )
		{
			result = "Build";
		}
		else
		{
			result = "Produce";
		}
		result += " time: ";
		return result;
	}
	public string TimeToString( int duration_in_second )
	{
		System.TimeSpan time = System.TimeSpan.FromSeconds( duration_in_second );
		return TimeToString( time );
	}
}