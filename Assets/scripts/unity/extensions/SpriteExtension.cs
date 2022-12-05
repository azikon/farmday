using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public static class SpriteExtension
{
	public static void SetNormalizedSize( this Image sprite, int size )
	{
		if ( ( float )sprite.sprite.rect.width > ( float )sprite.sprite.rect.height )
		{
			float sizeX = ( ( float )sprite.sprite.rect.height / ( float )sprite.sprite.rect.width ) * size;
			sprite.GetComponent<RectTransform>().sizeDelta = new Vector2( size, ( int )sizeX );
		}
		else
		{
			float sizeX = ( ( float )sprite.sprite.rect.width / ( float )sprite.sprite.rect.height ) * size;
			sprite.GetComponent<RectTransform>().sizeDelta = new Vector2( ( int )sizeX, size );
		}
	}
}