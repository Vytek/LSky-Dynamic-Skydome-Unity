//////////////////////////////////////////////
/// LSky
/// ====================
///
/// Description
/// ==============
/// Basic Object.
//////////////////////////////////////////////


using System;
using UnityEngine;

namespace Rallec.LSky
{


	[Serializable]
	public class LSky_BasicObject : LSky_EmptyObject
	{


		public MeshFilter meshFilter;
		//===============================

		public MeshRenderer meshRenderer;
		//===============================

		public override bool CheckComponents
		{

			get
			{
				
				if(meshFilter == null) return false;
				if(meshRenderer == null) return false;

				return base.CheckComponents;
			}
		}
		//=====================================================================


		public override void Instantiate(string parentName, string name)
        {

			base.Instantiate(parentName, name);
			

			var mf = gameObject.GetComponent<MeshFilter>();

			if(mf != null)
				meshFilter = mf;
			else	
				meshFilter = gameObject.AddComponent<MeshFilter>();

			var mr = gameObject.GetComponent<MeshRenderer>();

			if(mr != null)
				meshRenderer = mr;
			else	
				meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
		//======================================================================

	}

}