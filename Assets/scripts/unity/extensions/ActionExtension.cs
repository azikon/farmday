using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionExtension
{
    public static void Delay( this MonoBehaviour _monoBehaviour, System.Action _returnMethod, float _time )
    {
        _monoBehaviour.StartCoroutine( DelayAndCall( _returnMethod, _time ) );
    }

    static IEnumerator DelayAndCall( System.Action _returnMethod, float _time )
    {
        yield return new WaitForSeconds( _time );
        _returnMethod();
    }
}