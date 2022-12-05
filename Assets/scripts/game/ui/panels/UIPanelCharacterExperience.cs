using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIPanelCharacterExperience : UIBase
{
    public GameObject PrefabItem;
    public GameObject ItemsParent;

    public TMP_Text lblLevelExperience;
    public TMP_Text lblLevelCount;
    public RectTransform rtCarrotIcon;

    public Image spForeground;

    public int LevelExperience;
    public int CurrentLevel = 1;
    public int MaxExperiencePerLevel = 5;

    public override void Show<T>( T mapField )
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        UIHandler.Instance.BaseHide( this );
    }

    public void AddExperience( MapField mapField )
    {
        if ( mapField.GetType() == typeof( MapFieldPlant ) )
        {
            UIHandler.Instance.BaseShow( this );

            MapFieldPlant fieldPlant = mapField as MapFieldPlant;
            CreateItems( fieldPlant );
        }
    }

    public void CreateItems( MapField mapField )
    {
        GameObject itemObject = Instantiate( PrefabItem );
        itemObject.transform.SetParent( ItemsParent.transform, false );

        UIPanelCharacterExperienceItem carrotItem = itemObject.GetComponent<UIPanelCharacterExperienceItem>();

        MoveResourceItem( itemObject, mapField.transform.position, rtCarrotIcon.transform.position, ( a ) =>
        {
            if ( LevelExperience + 1 > MaxExperiencePerLevel )
            {
                CurrentLevel += 1;
                lblLevelCount.text = CurrentLevel.ToString();

                LevelExperience = 0;
            }
            else
            {
                LevelExperience += 1;
            }
            lblLevelExperience.text = LevelExperience.ToString() + " / 5";
            spForeground.fillAmount = ( float )LevelExperience / ( float )MaxExperiencePerLevel;
        }, 1 );
    }

    public void MoveResourceItem( GameObject itemObject, Vector3 startWorldPosition, Vector3 targetScreenPosition, System.Action<int> returnMethod, int count )
    {
        startWorldPosition = SceneHandler.Instance.EnvironmentCamera.WorldToScreenPoint( startWorldPosition );
        itemObject.transform.position = startWorldPosition;
        Vector3[] path = CreatePosition( startWorldPosition, targetScreenPosition );
        ( itemObject.transform as RectTransform ).DOPath( path, 1f ).OnComplete( () => { returnMethod( count ); GameObject.Destroy( itemObject ); } );
    }

    Vector3[] CreatePosition( Vector3 startWorldPosition, Vector3 targetScreenPosition )
    {
        Vector3[] path = new Vector3[ 3 ];
        path[ 0 ] = startWorldPosition;
        path[ 0 ] = new Vector3( path[ 0 ].x, path[ 0 ].y, 0.0f );
        path[ 1 ] = new Vector3( path[ 0 ].x - 280f, path[ 0 ].y + 120f, 0.0f );
        path[ 2 ] = targetScreenPosition;
        path[ 2 ] = new Vector3( path[ 2 ].x, path[ 2 ].y, 0.0f );
        return path;
    }
}