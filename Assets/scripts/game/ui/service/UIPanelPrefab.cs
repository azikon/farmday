using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class UIPanelPrefab
{
    // Mark if need instance this UI
    public bool IsPrefab;
    // Instance proceed when UI initialized
    public bool InitializeInstantiate;
    // Instance proceed when UI showed first time
    public bool ShowInitialize;
    // Set default params to prefab root object
    public bool DefaultVectors;
    // Removed prefab root parent and unpacked contents in root parent folder
    public bool WithoutPanel;

    // Your folder path in "Recources" folder
    public string PrefabPath;
    // Instantiated prefab name
    public string PrefabName;

    // Base UI, setted in UIBase.Initialization()
    private UIBase ui;
    // Marked when ui prefab instanced
    public bool IsInstantiated;
    // Prefab root object, if WithoutPanel == false
    private RectTransform instantiated;


    /// <summary>
    /// Initialize method
    /// </summary>
    public virtual void LoadData()
    {
        if ( IsPrefab == false )
        {
            return;
        }
        if ( InitializeInstantiate == true )
        {
            InstancePrefab();
        }
    }

    /// <summary>
    /// Show method called from UIBase.Show() method
    /// </summary>
    public virtual void Show()
    {
        if ( IsPrefab == false )
        {
            return;
        }
        InstancePrefab();
        if ( ShowInitialize == true )
        {
            if ( ui.IsInited == false )
            {
                ui.LoadData();
            }
        }
    }

    /// <summary>
    /// Hide method called from UIBase.Hide() method
    /// </summary>
    public virtual void Hide()
    {
        if ( IsPrefab == false )
        {
            return;
        }
    }

    public void SetUI( UIBase _ui )
    {
        ui = _ui;
    }

    private void InstancePrefab()
    {
        if ( IsInstantiated == true )
        {
            return;
        }
        IsInstantiated = true;
        GameObject prefab = Resources.Load<GameObject>( PrefabPath + PrefabName );
        GameObject inst = Object.Instantiate( prefab, ui.UIPanel.transform );
        if ( DefaultVectors )
        {
            inst.LocalVectorsDefaultRT();
        }
        if ( WithoutPanel == false )
        {
            instantiated = inst.GetComponent<RectTransform>();

            inst.name = "panel";
        }
        else
        {
            Transform[] childs = new Transform[ inst.transform.childCount ];
            for ( int i = 0; i < childs.Length; i++ )
            {
                childs[ i ] = inst.transform.GetChild( i );
            }
            for ( int i = 0; i < childs.Length; i++ )
            {
                childs[ i ].SetParent( ui.UIPanel.transform, false );
            }
            Object.Destroy( inst );
        }
    }
}