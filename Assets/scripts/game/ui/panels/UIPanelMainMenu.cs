using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIPanelMainMenu : UIBase
{
    public Slider Slider;
    public TMP_Text SliderCount;

    public override void LoadData()
    {
        base.LoadData();
    }

    public override void Show()
    {
        base.Show();
        UIHandler.Instance.BaseShow( this );
    }

    public override void Hide()
    {
        base.Hide();
        UIHandler.Instance.BaseHide( this );
    }

    public void UpdateSliderCount( Slider slider )
    {
        SliderCount.text = slider.value.ToString() + " / " + slider.maxValue.ToString();
    }

    public void OnClickPlay()
    {
        UIHandler.Instance.UIGet( "UIPanelLoading" ).Show();

        this.Delay( () => { LoadMap(); }, 2.5f );
    }

    private void LoadMap()
    {
        MapHandler.Instance.SetMapCellsCount( ( int )Slider.value, ( int )Slider.value );

        GameHandler.Instance.ContinueLoadData();

        this.Delay( () => { Hide(); }, 0.1f );
    }
}