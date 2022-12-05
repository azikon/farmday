using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapFieldData
{
	public string UID;
	public string ID;
	public string Name;
	public string Description;

    public string Type;
    public string ForType;
    public int Count;


    public long ProductionStartTimeInSeconds;
    public long ProductionDurationInSeconds;

    public MapFieldContentData MapFieldContentData;
}