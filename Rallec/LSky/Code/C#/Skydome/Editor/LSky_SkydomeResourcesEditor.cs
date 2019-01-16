////////////////////////////////////////
/// LSky
/// ====================
///
/// Description:
/// =============
/// Custom inspector for skydome.
////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using Rallec.Utility;

namespace Rallec.LSky
{

	public partial class LSky_SkydomeEditor : LSky_CommonEditor
	{


		// Resources
		//----------------------------------
		SerializedProperty m_Resources;
		//==================================

		// Components
		//----------------------------------
		//SerializedProperty m_Camera;
		//==================================

		// Foldouts
		//----------------------------------
		bool m_RCFoldout;
		//==================================


		protected void InitResources()
		{

			// Resources
			//------------------------------------------------
			m_Resources = serObj.FindProperty("m_Resources");
			//================================================

			// Camera
			//------------------------------------------------
			//m_Camera = serObj.FindProperty("m_Camera");
			//================================================
		}

		protected virtual void Resources()
		{

			R_EditorGUIUtility.ShurikenFoldoutHeader("Resources", TextTitleStyle, ref m_RCFoldout);
            if (m_RCFoldout)
            {

				// Resources
				//---------------------------------------------------------------------------------------------
				R_EditorGUIUtility.ShurikenHeader("Resources", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					GUI.backgroundColor = (m_Resources.objectReferenceValue != null) ? green : red;
                    EditorGUILayout.PropertyField(m_Resources, new GUIContent("Resources"));
					GUI.backgroundColor = Color.white;

					if (m_Resources.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("Resources Data is not assigned", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Be sure to allocate all resources", MessageType.Info);
                    }

                EditorGUILayout.Separator();
				//=============================================================================================

				/* 
				// Components
				//---------------------------------------------------------------------------------------------

				R_EditorGUIUtility.ShurikenHeader("Components", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    GUI.backgroundColor = (m_Camera.objectReferenceValue != null) ? green : red;
                    EditorGUILayout.PropertyField(m_Camera, new GUIContent("Camera"));
					GUI.backgroundColor = Color.white;

				//if(m_Camera.objectReferenceValue == null)
                 //   EditorGUILayout.HelpBox("Camera is not assigned", MessageType.Warning);

                EditorGUILayout.Separator();*/
				//=============================================================================================

			}

		}

	}

}