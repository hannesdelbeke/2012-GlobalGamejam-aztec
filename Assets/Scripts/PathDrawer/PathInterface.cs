using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	public class PathInterface : MonoBehaviour 
	{
		protected PathDrawer mDrawer = null;
		public CharacterController2D PlayerPosition;
		protected Vector3 mPrevPlayerPosition;
		
		// Use this for initialization
		void Start () 
		{
			mDrawer = GetComponent<PathDrawer>();//GameObject.Find("PathDrawer").GetComponent<PathDrawer>();
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(!PlayerPosition.transform.position.Equals(mPrevPlayerPosition)) 
			{
				mDrawer.AddSection(PlayerPosition.transform.position);
				mPrevPlayerPosition = PlayerPosition.transform.position;
			}
		}
		
		/*
		public void Clear() {
			Clear();
		}
		*/
	}
	
