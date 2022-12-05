using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Map Field Parameters", menuName = "FL Map Field Parameters", order = 51 )]
public class MapFieldParametersData : ScriptableObject
{
    [SerializeField]
    public List<MapFieldData> MapFieldsType;
}