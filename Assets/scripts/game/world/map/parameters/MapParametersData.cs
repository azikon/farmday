using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Map Parameters", menuName = "FL Map Parameters", order = 51 )]
public class MapParametersData :  ScriptableObject
{
    [SerializeField]
    private Dictionary<string, string> a;
    [SerializeField]
    public string MapName;
}