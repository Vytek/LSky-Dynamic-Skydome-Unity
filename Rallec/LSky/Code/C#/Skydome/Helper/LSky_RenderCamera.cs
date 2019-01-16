//////////////////////////////////
///
///
///
///
///
//////////////////////////////////


using System;
using UnityEngine;


namespace Rallec.LSky
{

	[System.Serializable]
	public class LSky_RenderCamera : LSky_EmptyObject
	{


		public Camera camera;

		public override bool CheckComponents
		{
			get
			{

				if(camera == null) return false;
				return base.CheckComponents;

			}
		}


		public  void InstantiateCamera(string rootName, string parentName, string camName)
        {

        	Instantiate(rootName, parentName, camName);

			var cam = gameObject.GetComponent<Camera>();

			if(cam != null)
				camera = cam;
			else	
				camera = gameObject.AddComponent<Camera>();

        }


		
	}

}