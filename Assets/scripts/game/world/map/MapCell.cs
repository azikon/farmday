using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapCell : MonoBehaviour
{
    private bool _isStaticOnMap;
    public bool IsStaticOnMap
    {
        get { return _isStaticOnMap; }
        set
        {
            _isStaticOnMap = value;
            EffectsOutline.ForEach( x => x.EffectIsActive = !value );
        }
    }

    private bool _hasAddProductionItem;
    public bool HasAddProductionItem
    {
        get { return _hasAddProductionItem; }
        set { _hasAddProductionItem = value; }
    }
    public bool IsSelected { get; set; }

    public List<cakeslice.Outline> EffectsOutline;

    public void Initialization()
    {
        EffectsOutline = GetComponentsInChildren<cakeslice.Outline>().ToList();
    }

    public void OnClick()
    {
        if ( _isStaticOnMap == false && HasAddProductionItem == false )
        {
            UIHandler.Instance.UIGet( "UIPanelCellProduction" ).Show( this );
        }
    }

    public void OnSelect( bool selected )
    {
        IsSelected = selected;
        SetOutlineIsShow( selected );
    }


    public void OnHover( bool state )
    {
        if ( IsSelected == false )
        {
            SetOutlineIsShow( state );
        }
    }

    private void SetOutlineIsShow( bool isShow )
    {
        if ( isShow == true )
        {
            EffectsOutline.ForEach( x => x.OutlineShow() );
        }
        else
        {
            EffectsOutline.ForEach( x => x.OutlineHide() );
        }
    }
}