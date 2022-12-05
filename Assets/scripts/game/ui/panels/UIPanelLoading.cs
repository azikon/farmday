using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;
using DG;
using DG.Tweening;

public class UIPanelLoading : UIBase
{
    public RectTransform[] ArrayLeftBackground;
    public RectTransform[] ArrayRightBackground;

    public RectTransform rtLogo;

    public override void LoadData()
    {
        base.LoadData();
    }

    public override void Show()
    {
        base.Show();
        //UIHandler.Instance.BaseShow( this );

        //this.Delay( StartShowAnimation, 1f );
        StartShowAnimation();
    }

    public override void Hide()
    {
        base.Hide();
        //UIHandler.Instance.BaseHide( this );

        //this.Delay( StartHideAnimation, 1f );
        StartHideAnimation();
    }

    private void StartShowAnimation()
    {
        StartShowAnimationLogo();
    }
    private void StartHideAnimation()
    {
        StartHideAnimationLogo();
    }

    public void StartShowAnimationLogo()
    {
        rtLogo.DOLocalMoveX( -50f, 0.5f ).
            OnComplete( ()=>
            {
                rtLogo.DOLocalMoveX( 0f, 0.35f ).
                OnComplete( () => 
                {
                    AnimationBackground( true );
                } );
            } );
    }

    public void StartHideAnimationLogo()
    {
        rtLogo.DOLocalMoveX( -50f, 0.35f ).
            OnComplete( () =>
            {
                rtLogo.DOLocalMoveX( 2000f, 0.5f ).
                OnComplete( () =>
                {
                    AnimationBackground( false );
                } );
            } );
    }

    public void AnimationBackground( bool _show )
    {
        int multiplier = _show == true ? 0 : 1;
        foreach ( RectTransform rt in ArrayLeftBackground )
        {
            rt.DOLocalMoveX( 4000f * multiplier, Random.Range( 0.9f, 2f ) );
        }
        foreach ( RectTransform rt in ArrayRightBackground )
        {
            rt.DOLocalMoveX( -4000f * multiplier, Random.Range( 0.9f, 2f ) );
        }
    }
}