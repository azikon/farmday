﻿/*
//  Copyright (c) 2015 José Guerreiro. All rights reserved.
//
//  MIT license, see http://www.opensource.org/licenses/mit-license.php
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
*/

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace cakeslice
{
	[RequireComponent( typeof( Renderer ) )]
	/* [ExecuteInEditMode] */
	public class Outline : MonoBehaviour
	{
		public Renderer Renderer { get; private set; }
		public SpriteRenderer SpriteRenderer { get; private set; }
		public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
		public MeshFilter MeshFilter { get; private set; }

		public int color;
		public bool eraseRenderer;

		private bool _effectIsActive;
		public bool EffectIsActive
		{
			get { return _effectIsActive; }
			set { _effectIsActive = value; }
		}

		private void Awake()
		{
			Renderer = GetComponent<Renderer>();
			SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			SpriteRenderer = GetComponent<SpriteRenderer>();
			MeshFilter = GetComponent<MeshFilter>();
		}

		public void OnHover( bool state )
		{
			if ( EffectIsActive == false )
			{
				return;
			}
			if ( state == true )
			{
				OutlineShow();
			}
			else
			{
				OutlineHide();
			}
		}

		public void OutlineShow()
		{
			OutlineEffect.Instance?.AddOutline( this );
		}

		public void OutlineHide()
		{
			OutlineEffect.Instance?.RemoveOutline( this );
		}

		private Material[] _SharedMaterials;
		public Material[] SharedMaterials
		{
			get
			{
				if ( _SharedMaterials == null )
					_SharedMaterials = Renderer.sharedMaterials;

				return _SharedMaterials;
			}
		}
	}
}