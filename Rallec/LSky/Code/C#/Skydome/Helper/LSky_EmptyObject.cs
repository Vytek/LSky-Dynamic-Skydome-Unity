//////////////////////////////////
/// LSky
/// =============
///
/// Description:
/// =============
/// Empty Object.
//////////////////////////////////


using System;
using UnityEngine;


namespace Rallec.LSky
{

	[Serializable]
	public class LSky_EmptyObject
	{

		public GameObject gameObject;
		public Transform transform;


		public virtual bool CheckComponents
		{
			get
			{

				if(gameObject == null) return false;
				if(transform == null) return false;


				return true;

			}
		}


		public void InitTransform(Transform parent, Vector3 posOffset)
        {

            if(transform == null) return;

            transform.parent        = parent;
            transform.position      = Vector3.zero + posOffset;
            transform.localPosition = Vector3.zero + posOffset;
            transform.rotation      = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale    = Vector3.one;
        }


		public virtual void Instantiate(string parentName, string name)
        {

            if(gameObject == null)
            {
				// Check if exist gameobject with this name.
                var childObj = GameObject.Find("/" + parentName + "/" + name); 

                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);

            }

            if(transform == null)
                transform = gameObject.transform; // Get transform.

        }

        
		public virtual void Instantiate(string rootName, string parentName, string name)
        {

            if(gameObject == null)
            {
				// Check if exist gameobject with this name.
                var childObj = GameObject.Find("/" + rootName + "/" + parentName + "/" + name); 

                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);

            }

            if(transform == null)
                transform = gameObject.transform; // Get transform.

        }

		
	}

}