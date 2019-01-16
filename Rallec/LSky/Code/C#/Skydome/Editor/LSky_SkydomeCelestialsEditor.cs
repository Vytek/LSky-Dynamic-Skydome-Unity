////////////////////////////////////////
/// LSKy
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


		#region |Deep Space|

		// Galaxy Background
		//--------------------------------------------------
		SerializedProperty m_RenderGalaxy;
		SerializedProperty m_GalaxyLayerIndex;
		SerializedProperty m_GalaxyCubemap;
		SerializedProperty m_GalaxyColor;
		SerializedProperty m_GalaxyIntensity;
		SerializedProperty m_GalaxyExponentFade;
		//=================================================

		// Stars Field
		//-------------------------------------------------
		SerializedProperty m_RenderStarsField;
		SerializedProperty m_StarsFieldeLayerIndex;
		SerializedProperty m_StarsFieldCubemap;
		SerializedProperty m_StarsFieldNoiseCubemap;
		SerializedProperty m_StarsFieldColor;
		SerializedProperty m_StarsFieldIntensity;
		SerializedProperty m_StarsFieldScintillation;
		SerializedProperty m_StarsFieldScintillationSpeed;
		//=================================================

		#endregion

		#region |Near Space|

		// General
		//-------------------------------------
		SerializedProperty m_RenderNearSpace;
		SerializedProperty m_NearSpaceLayerIndex;

		
		// Sun
		//--------------------------------------
		SerializedProperty m_SunPI;
		SerializedProperty m_SunTheta;
		SerializedProperty m_RenderSun;
		SerializedProperty m_SunSize;
		SerializedProperty m_SunColor;
		SerializedProperty m_SunIntensity;
		//=====================================

		// Moon
		//----------------------------------------
		SerializedProperty m_MoonPI;
		SerializedProperty m_MoonTheta;
		SerializedProperty m_RenderMoon;
		SerializedProperty m_MoonRenderTexture;
		SerializedProperty m_MoonRenderLayerIndex;
		SerializedProperty m_MoonTexture;
		SerializedProperty m_MoonTextureOffset;
		SerializedProperty m_MoonSize;
		SerializedProperty m_MoonColor;
		SerializedProperty m_MoonIntensity;
		SerializedProperty m_MoonExponentFade;
		//=========================================


		#endregion


		#region |Foldouts|
		bool m_DeepSpaceFoldout, m_NearSpaceFoldout;

		#endregion
		
 
		protected void InitCelestials()
		{

			// Deep Space
			//--------------------------------------------------------------------------------------
			m_RenderGalaxy       = serObj.FindProperty("m_RenderGalaxy");
			m_GalaxyLayerIndex   = serObj.FindProperty("m_GalaxyLayerIndex");
			m_GalaxyCubemap      = serObj.FindProperty("m_GalaxyCubemap");
		 	m_GalaxyColor        = serObj.FindProperty("m_GalaxyColor");
		 	m_GalaxyIntensity    = serObj.FindProperty("m_GalaxyIntensity");
		 	m_GalaxyExponentFade = serObj.FindProperty("m_GalaxyExponentFade");
			//--------------------------------------------------------------------------------------
		 	m_RenderStarsField      	   = serObj.FindProperty("m_RenderStarsField");
		 	m_StarsFieldeLayerIndex        = serObj.FindProperty("m_StarsFieldLayerIndex");
		 	m_StarsFieldCubemap            = serObj.FindProperty("m_StarsFieldCubemap");
		 	m_StarsFieldNoiseCubemap       = serObj.FindProperty("m_StarsFieldNoiseCubemap");
		 	m_StarsFieldColor              = serObj.FindProperty("m_StarsFieldColor");
		 	m_StarsFieldIntensity          = serObj.FindProperty("m_StarsFieldIntensity");
		 	m_StarsFieldScintillation      = serObj.FindProperty("m_StarsFieldScintillation");
		 	m_StarsFieldScintillationSpeed = serObj.FindProperty("m_StarsFieldScintillationSpeed");
			//======================================================================================

			// Near Space
			//--------------------------------------------------------------------------------------
		 	m_RenderNearSpace     = serObj.FindProperty("m_RenderNearSpace");
		 	m_NearSpaceLayerIndex = serObj.FindProperty("m_NearSpaceLayerIndex");
			//--------------------------------------------------------------------------------------
		 	m_SunPI        = serObj.FindProperty("m_SunPI");
		 	m_SunTheta     = serObj.FindProperty("m_SunTheta");
		 	m_RenderSun    = serObj.FindProperty("m_RenderSun");
		 	m_SunSize      = serObj.FindProperty("m_SunSize");
		 	m_SunColor     = serObj.FindProperty("m_SunColor");
		 	m_SunIntensity = serObj.FindProperty("m_SunIntensity");
			//-------------------------------------------------------------------------------------
		 	m_MoonPI            = serObj.FindProperty("m_MoonPI");
		 	m_MoonTheta         = serObj.FindProperty("m_MoonTheta");
		 	m_RenderMoon        = serObj.FindProperty("m_RenderMoon");
		 	m_MoonRenderTexture = serObj.FindProperty("m_MoonRenderTexture");
			m_MoonRenderLayerIndex = serObj.FindProperty("m_MoonRenderLayerIndex");
		 	m_MoonTexture       = serObj.FindProperty("m_MoonTexture");
		 	m_MoonTextureOffset = serObj.FindProperty("m_MoonTextureOffset");
		 	m_MoonSize          = serObj.FindProperty("m_MoonSize");
		 	m_MoonColor         = serObj.FindProperty("m_MoonColor");
		 	m_MoonIntensity     = serObj.FindProperty("m_MoonIntensity");
		 	m_MoonExponentFade  = serObj.FindProperty("m_MoonExponentFade");
			//-------------------------------------------------------------------------------------

			
		}

		protected virtual void Celestials()
		{

			#region |Deep Space|

			R_EditorGUIUtility.ShurikenFoldoutHeader("Deep Space", TextTitleStyle, ref m_DeepSpaceFoldout);
			if(m_DeepSpaceFoldout)
            {

				R_EditorGUIUtility.ShurikenHeader("Galaxy Background", TextSectionStyle, 20);
                EditorGUILayout.Separator();


					EditorGUILayout.PropertyField(m_RenderGalaxy, new GUIContent("Render Galaxy"));

					if(m_RenderGalaxy.boolValue)
					{
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

 						EditorGUILayout.PropertyField(m_GalaxyLayerIndex, new GUIContent("Galaxy Layer Index"));
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						m_GalaxyCubemap.objectReferenceValue = (Cubemap)EditorGUILayout.ObjectField("Galaxy Cubemap", m_GalaxyCubemap.objectReferenceValue, typeof(Cubemap), true);
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_GalaxyColor, new GUIContent("Galaxy Color"));

						EditorGUILayout.BeginVertical("Box");
						EditorGUILayout.PropertyField(m_GalaxyIntensity, new GUIContent("Galaxy Intensity"));
						EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
						EditorGUILayout.EndVertical();
						
						EditorGUILayout.PropertyField(m_GalaxyExponentFade, new GUIContent("Galaxy Exponent Fade"));
						R_EditorGUIUtility.Separator(2);
		
					}
				EditorGUILayout.Separator();

					

				R_EditorGUIUtility.ShurikenHeader("Stars Field", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_RenderStarsField, new GUIContent("Stars Field"));

					if(m_RenderStarsField.boolValue)
					{
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_StarsFieldeLayerIndex, new GUIContent("Stars Field Layer Index"));
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						m_StarsFieldCubemap.objectReferenceValue = (Cubemap)EditorGUILayout.ObjectField("Stars Field Cubemap", m_StarsFieldCubemap.objectReferenceValue, typeof(Cubemap), true);
						m_StarsFieldNoiseCubemap.objectReferenceValue = (Cubemap)EditorGUILayout.ObjectField("Stars Field Noise Cubemap (R)", m_StarsFieldNoiseCubemap.objectReferenceValue, typeof(Cubemap), true);
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_StarsFieldColor, new GUIContent("Stars Field Color"));

						EditorGUILayout.BeginVertical("Box");
						EditorGUILayout.PropertyField(m_StarsFieldIntensity, new GUIContent("Stars Field Intensity"));
						EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
						EditorGUILayout.EndVertical();

						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_StarsFieldScintillation, new GUIContent("Stars Field Scintillation"));
						EditorGUILayout.PropertyField(m_StarsFieldScintillationSpeed, new GUIContent("Stars Field Scintillation Speed"));
						R_EditorGUIUtility.Separator(2);

					}

 				EditorGUILayout.Separator();
				//========================================



			}

			#endregion


			#region |Near Space|

			R_EditorGUIUtility.ShurikenFoldoutHeader("Near Space", TextTitleStyle, ref m_NearSpaceFoldout);
			if(m_NearSpaceFoldout)
            {

				R_EditorGUIUtility.ShurikenHeader("General Settings", TextSectionStyle, 20);
                EditorGUILayout.Separator();

				EditorGUILayout.PropertyField(m_RenderNearSpace, new GUIContent("Render Near Space"));
				
					R_EditorGUIUtility.Separator(2);
					EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_NearSpaceLayerIndex, new GUIContent("Near Space Layer Index"));
					R_EditorGUIUtility.Separator(2);

					R_EditorGUIUtility.ShurikenHeader("Sun", TextSectionStyle, 20);

						EditorGUILayout.PropertyField(m_SunPI, new GUIContent("Sun PI"));
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_SunTheta, new GUIContent("Sun Theta"));
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

				if(m_RenderNearSpace.boolValue)
				{
				

						EditorGUILayout.PropertyField(m_RenderSun, new GUIContent("Render Sun"));
						if(m_RenderSun.boolValue)
						{
							EditorGUILayout.Separator();
							EditorGUILayout.BeginVertical("Box");
							EditorGUILayout.PropertyField(m_SunSize, new GUIContent("Sun Size"));
							EditorGUILayout.HelpBox("Evaluate Curve time by sun above horizon", MessageType.Info);
							EditorGUILayout.EndVertical();
							EditorGUILayout.Separator();

							EditorGUILayout.BeginVertical("Box");
							EditorGUILayout.PropertyField(m_SunColor, new GUIContent("Sun Color"));
							EditorGUILayout.HelpBox("Evaluate Gradient time by sun above horizon", MessageType.Info);
							EditorGUILayout.EndVertical();
							
							EditorGUILayout.PropertyField(m_SunIntensity, new GUIContent("Sun Intensity"));
							EditorGUILayout.Separator();

						}
				}

					R_EditorGUIUtility.ShurikenHeader("Moon", TextSectionStyle, 20);

						EditorGUILayout.PropertyField(m_MoonPI, new GUIContent("Moon PI"));
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_MoonTheta, new GUIContent("Moon Theta"));
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

				if(m_RenderNearSpace.boolValue)
				{
				
						EditorGUILayout.PropertyField(m_RenderMoon, new GUIContent("Render Moon"));
						if(m_RenderMoon.boolValue)
						{

							R_EditorGUIUtility.Separator(2);
							EditorGUILayout.BeginVertical("Box");
							m_MoonRenderTexture.objectReferenceValue = (RenderTexture)EditorGUILayout.ObjectField("Moon Render Texture", m_MoonRenderTexture.objectReferenceValue, typeof(RenderTexture), true);
							
							R_EditorGUIUtility.Separator(2);
							EditorGUILayout.PropertyField(m_MoonRenderLayerIndex, new GUIContent("Moon Render Layer Index"));
							R_EditorGUIUtility.Separator(2);
							EditorGUILayout.EndVertical();
							R_EditorGUIUtility.Separator(2);
							EditorGUILayout.Separator();

							m_MoonTexture.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField("Moon Texture(Map)", m_MoonTexture.objectReferenceValue, typeof(Texture2D), true);
							EditorGUILayout.Separator();

							EditorGUILayout.PropertyField(m_MoonTextureOffset, new GUIContent("Moon Texture Offset"));
							EditorGUILayout.Separator();

							EditorGUILayout.Separator();
							EditorGUILayout.BeginVertical("Box");
							EditorGUILayout.PropertyField(m_MoonSize, new GUIContent("Moon Size"));
							EditorGUILayout.HelpBox("Evaluate curve time by sun above horizon", MessageType.Info);
							EditorGUILayout.EndVertical();
							EditorGUILayout.Separator();

							EditorGUILayout.BeginVertical("Box");
							EditorGUILayout.PropertyField(m_MoonColor, new GUIContent("Moon Color"));
							EditorGUILayout.HelpBox("Evaluate gradient time by moon above horizon", MessageType.Info);
							EditorGUILayout.EndVertical();

							EditorGUILayout.PropertyField(m_MoonIntensity, new GUIContent("Moon Intensity"));
							EditorGUILayout.PropertyField(m_MoonExponentFade, new GUIContent("Moon Exponent Fade"));
							EditorGUILayout.Separator();
						}
					EditorGUILayout.Separator();
				}

			}

			#endregion

		}


	}

}