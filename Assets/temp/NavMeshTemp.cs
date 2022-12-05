using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTemp : MonoBehaviour
{

    public bool a;
    public NavMeshSurface NavMeshSurface;

    private void Update()
    {
        if ( a )
        {
            a = false;
            NavMeshSurface.BuildNavMesh();
        }
    }
}
