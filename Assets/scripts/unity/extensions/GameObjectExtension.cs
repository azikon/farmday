using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class GameObjectExtension
{
    public static void Activate( this GameObject targetGameObject )
    {
        targetGameObject.SetActive( true );
    }
    public static void Deactivate( this GameObject targetGameObject )
    {
        targetGameObject.SetActive( false );
    }

    public static void LocalVectorsDefaultRT( this GameObject target )
    {
        RectTransform transform = target.GetComponent<RectTransform>();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler( Vector3.zero );
        transform.localScale = Vector3.one;
    }

    public static void LocalVectorsDefault( this GameObject target )
    {
        Transform transform = target.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler( Vector3.zero );
        transform.localScale = Vector3.one;
    }

    public static GameObject GetChildWithName( this GameObject obj, string name )
    {
        Transform[] allChildren = obj.GetComponentsInChildren<Transform>();
        foreach ( Transform child in allChildren )
        {
            if ( child.name.Equals( name ) == true )
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public static void DestroyAllChilds( this GameObject obj )
    {
        foreach ( Transform child in obj.transform )
        {
            UnityEngine.GameObject.Destroy( child.gameObject );
        }
    }
}