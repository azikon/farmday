using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPanelProductionProgressBar : UIBase
{
    public TMP_Text labelTime;
    public MapField MapField;

    public override void LoadData()
    {
        base.LoadData();
        UIHandler.Instance.UIUpdateFrameAdd( this );
    }

    public override void Show<T>( T mapField )
    {
        if ( mapField.GetType() == typeof( MapFieldPlant ) )
        {
            UIHandler.Instance.BaseShow( this, true );

            ( UIPanel.transform as RectTransform ).localPosition = new Vector3( 0, -125f, 0f );

            MapFieldPlant field = mapField as MapFieldPlant;
            MapField = field;
            CameraHandler.Instance.EnvironmentCameraController.CameraRepositionToPoint( field.transform.position );
        }

        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        UIHandler.Instance.BaseHide( this );
    }

    public override void UIUpdateEveryFrame()
    {
        if ( IsShowed == true )
        {
            if ( MapField != null && MapField.TimeAction != null )
            {
                labelTime.text = TimeActionHandler.Instance.TimeToString( MapField.TimeAction.RemainingTime );
            }
        }
    }
}