using UnityEngine;
using System.Collections.Generic;

public class TimeAction
{
	public bool SpeedUp
	{
		get; set;
	}

	public string Type
	{
		get; set;
	}

	public System.DateTime StartTime
	{
		get; set;
	}

	public System.DateTime EndTime
	{
		get; set;
	}

	public int DurationInSeconds
	{
		get; set;
	}

	public float PercentageComplete
	{
		get
		{
			float elapsedSeconds = ( int )( System.DateTime.Now - StartTime ).TotalSeconds;
			float percentageComplete = elapsedSeconds / ( float )DurationInSeconds;
			if ( percentageComplete > 1.0f )
			{
				percentageComplete = 1.0f;
			}
			return percentageComplete;
		}
	}

	public System.TimeSpan RemainingTime
	{
		get
		{
			System.TimeSpan span = EndTime - System.DateTime.Now;
			if ( span.TotalSeconds < 0 )
			{
				return new System.TimeSpan( 0 );
			}
			return span;
		}
	}

	public bool IsAutoTimeAction
	{
		get
		{
			return false;
		}
	}

	public TimeAction()
	{
	}

	public TimeAction( string type, int durationInSeconds, System.DateTime startTime, string supportingId = null )
	{
		Type = type;
		DurationInSeconds = durationInSeconds;
		StartTime = startTime;
		EndTime = startTime + new System.TimeSpan( 0, 0, durationInSeconds );
	}
}

public class TimeActionType
{
	public const string NONE = "NONE";
	public const string BUILD = "BUILD";
	public const string INVENTORY = "INVENTORY";
	public const string GATHER = "GATHER";
	public const string AUTOMATIC = "AUTOMATIC";
}