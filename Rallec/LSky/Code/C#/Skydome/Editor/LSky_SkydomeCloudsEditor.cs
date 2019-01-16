////////////////////////////////////////
/// LSKy
/// ==================
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



		#region |Properties|

		// Renderer
		//-------------------------------------
		SerializedProperty m_RenderClouds;
		SerializedProperty m_CloudsLayerIndex;
		//=====================================

		// Scale
		//----------------------------------------
		SerializedProperty m_CloudsMeshHeightSize;
		//========================================

		// Rotation
		//----------------------------------------
		SerializedProperty m_CloudsRotationSpeed;
		//========================================

		// Texture
		//---------------------------------------
		SerializedProperty m_CloudsTexture;
		SerializedProperty m_CloudsSize;
		SerializedProperty m_CloudsTexOffset;
		//=======================================

		// Color
		//---------------------------------------

		SerializedProperty m_CloudsColor, m_CloudsMoonColor;
		SerializedProperty m_CloudsIntensity;
		//=======================================


		// Density.
		//---------------------------------------
		SerializedProperty m_CloudsDensity;
		SerializedProperty m_CloudsCoverage;
		//=====================================


		#endregion

	
		#region |Foldouts|
		bool m_CloudsFoldout;

		#endregion
		

		protected void InitClouds()
		{

			
			// Renderer
			//------------------------------------------------------------------------
			m_RenderClouds = serObj.FindProperty("m_RenderClouds");
			m_CloudsLayerIndex = serObj.FindProperty("m_CloudsLayerIndex");
			//========================================================================

			// Scale
			//------------------------------------------------------------------------
			m_CloudsMeshHeightSize = serObj.FindProperty("m_CloudsMeshHeightSize");
			//========================================================================

			// Rotation
			//------------------------------------------------------------------------
			m_CloudsRotationSpeed = serObj.FindProperty("m_CloudsRotationSpeed");
			//========================================================================


			// Texture
			//-----------------------------------------------------------------------
			m_CloudsTexture    = serObj.FindProperty("m_CloudsTexture");
			m_CloudsSize       = serObj.FindProperty("m_CloudsSize");
			m_CloudsTexOffset  = serObj.FindProperty("m_CloudsTexOffset");
			//=======================================================================

			// Color
			//-----------------------------------------------------------------------
			m_CloudsColor     = serObj.FindProperty("m_CloudsColor");
			m_CloudsMoonColor = serObj.FindProperty("m_CloudsMoonColor");
			m_CloudsIntensity = serObj.FindProperty("m_CloudsIntensity");
			//=======================================================================

			// Density.
			//-----------------------------------------------------------------------
			m_CloudsDensity       = serObj.FindProperty("m_CloudsDensity");
			m_CloudsCoverage      = serObj.FindProperty("m_CloudsCoverage");
			//=======================================================================

		}


		protected virtual void Clouds()
		{

			R_EditorGUIUtility.ShurikenFoldoutHeader("Clouds", TextTitleStyle, ref m_CloudsFoldout);
            if(m_CloudsFoldout)
            {


				R_EditorGUIUtility.ShurikenHeader("Clouds", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_RenderClouds, new GUIContent("Render Clouds"));
					R_EditorGUIUtility.Separator(2);

 					EditorGUILayout.PropertyField(m_CloudsLayerIndex, new GUIContent("Clouds Layer Index"));
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_CloudsMeshHeightSize, new GUIContent("Clouds Mesh Height Size"));
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_CloudsRotationSpeed, new GUIContent("Clouds Rotation Speed"));
					R_EditorGUIUtility.Separator(2);


					m_CloudsTexture.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField("Clouds Texture", m_CloudsTexture.objectReferenceValue, typeof(Texture2D), true);
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_CloudsSize, new GUIContent("Clouds Size"));
					EditorGUILayout.PropertyField(m_CloudsTexOffset, new GUIContent("Clouds Tex Offset"));
                    R_EditorGUIUtility.Separator(2);

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_CloudsColor, new GUIContent("Clouds Color"));
					EditorGUILayout.HelpBox("Evaluate Gradient time by full sun cicle", MessageType.Info);
					EditorGUILayout.EndVertical();

					if(m_NightAtmosphereMode.intValue == 1)
					{
						EditorGUILayout.BeginVertical("Box");
						EditorGUILayout.PropertyField(m_CloudsMoonColor, new GUIContent("Clouds Moon Color"));
						EditorGUILayout.HelpBox("Evaluate Gradient time by full moon cicle", MessageType.Info);
						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.PropertyField(m_CloudsIntensity, new GUIContent("Clouds Intensity"));
					R_EditorGUIUtility.Separator(2);


					EditorGUILayout.PropertyField(m_CloudsDensity, new GUIContent("Clouds Density"));
                    R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_CloudsCoverage, new GUIContent("Clouds Coverage"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.Separator();


                EditorGUILayout.Separator();
				//=============================================================================================================================================



			}


		}

	}

}