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
	public class LSky_Light : LSky_EmptyObject
	{


		public Light light;

		public override bool CheckComponents
		{
			get
			{

				if(light == null) return false;
				return base.CheckComponents;

			}
		}


		public  void InstantiateLight(string parentName, string lightName)
        {

        	Instantiate(parentName, lightName);

			var l = gameObject.GetComponent<Light>();

			if(l != null)
				light = l;
			else	
				light = gameObject.AddComponent<Light>();

        }


		
	}

}