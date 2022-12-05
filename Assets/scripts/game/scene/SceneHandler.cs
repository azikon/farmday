using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : HandlerBase<SceneHandler>
{
    public override void LoadData()
    {
    }

    [SerializeField]
    private GameObject _rootUI;
    [SerializeField]
    private GameObject _rootEnvironment;
    [SerializeField]
    private Camera _environmentCamera;
    [SerializeField]
    private Camera _uiCamera;

    public GameObject RootUI
    {
        get
        {
            return _rootUI;
        }
    }

    public GameObject RootEnvironment
    {
        get
        {
            return _rootEnvironment;
        }
    }

    public Camera EnvironmentCamera
    {
        get
        {
            return _environmentCamera;
        }
    }

    public Camera UICamera
    {
        get
        {
            return _uiCamera;
        }
    }
}