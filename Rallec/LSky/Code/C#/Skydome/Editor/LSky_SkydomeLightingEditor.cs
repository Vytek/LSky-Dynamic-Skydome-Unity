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



		#region |Direction Light|
		SerializedProperty m_SunLightIntensity;
		SerializedProperty m_SunLightColor;

		SerializedProperty m_MoonLightIntensity;
		SerializedProperty m_MoonLightColor;

		SerializedProperty m_SunMoonLightFade;

		#endregion


		#region |Ambient|
		SerializedProperty m_SendSkybox;
		SerializedProperty m_AmbientUpdateInterval;
		SerializedProperty m_AmbientGroundColor;

		#endregion


		#region |Fog|

		SerializedProperty m_EnableUnityFog;
		SerializedProperty m_UnityFogMode;
		SerializedProperty m_UnityFogColor;
		SerializedProperty m_UnityMoonFogColor;
		SerializedProperty m_UnityFogDensity;
		SerializedProperty m_UnityFogStartDistance;
		SerializedProperty m_UnityFogEndDistance;

		#endregion


		#region |Foldouts|
		bool m_LightingFoldout;

		#endregion
		

		protected void InitLighting()
		{


			#region |Direction Light|


			m_SunLightIntensity = serObj.FindProperty("m_SunLightIntensity");
			m_SunLightColor     = serObj.FindProperty("m_SunLightColor");

			m_MoonLightIntensity = serObj.FindProperty("m_MoonLightIntensity");
	 		m_MoonLightColor     = serObj.FindProperty("m_MoonLightColor");

			m_SunMoonLightFade   = serObj.FindProperty("m_SunMoonLightFade");

			#endregion


			#region |Ambient|

			m_SendSkybox = serObj.FindProperty("m_SendSkybox");
			m_AmbientUpdateInterval = serObj.FindProperty("m_AmbientUpdateInterval");
			m_AmbientGroundColor = serObj.FindProperty("m_AmbientGroundColor");

			#endregion


			#region |Fog|

			m_EnableUnityFog  = serObj.FindProperty("m_EnableUnityFog");
			m_UnityFogMode    = serObj.FindProperty("m_UnityFogMode");
			m_UnityFogColor   = serObj.FindProperty("m_UnityFogColor");
			m_UnityMoonFogColor   = serObj.FindProperty("m_UnityMoonFogColor");
			m_UnityFogDensity = serObj.FindProperty("m_UnityFogDensity");
			m_UnityFogStartDistance = serObj.FindProperty("m_UnityFogStartDistance");
			m_UnityFogEndDistance   = serObj.FindProperty("m_UnityFogEndDistance");

			#endregion


		}


		protected virtual void Lighting()
		{

			R_EditorGUIUtility.ShurikenFoldoutHeader("Lighitng", TextTitleStyle, ref m_LightingFoldout);
            if(m_LightingFoldout)
            {


				R_EditorGUIUtility.ShurikenHeader("Direction Light", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_SunLightIntensity, new GUIContent("Sun Light Intensity"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_SunLightColor, new GUIContent("Sun Light Color"));
					EditorGUILayout.HelpBox("Evaluate Gradient time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();

					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_MoonLightIntensity, new GUIContent("Moon Light Intensity"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full moon cycle", MessageType.Info);
					EditorGUILayout.EndVertical();


					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_MoonLightColor, new GUIContent("Moon Light Color"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full moon cycle", MessageType.Info);
					EditorGUILayout.EndVertical();
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_SunMoonLightFade, new GUIContent("Sun Moon Light Fade"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();
					R_EditorGUIUtility.Separator(2);

                EditorGUILayout.Separator();
				//=======================================================================================================


				R_EditorGUIUtility.ShurikenHeader("Ambient", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_SendSkybox, new GUIContent("Send Skybox"));
					EditorGUILayout.PropertyField(m_AmbientUpdateInterval, new GUIContent("Ambient Update Interval"));

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_AmbientGroundColor, new GUIContent("Ambient Ground Color"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
				//=======================================================================================================

				R_EditorGUIUtility.ShurikenHeader("Fog", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_EnableUnityFog, new GUIContent("Enable Unity Fog"));
				

					if(m_EnableUnityFog.boolValue)
					{

						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						EditorGUILayout.BeginVertical("Box");
						EditorGUILayout.PropertyField(m_UnityFogColor, new GUIContent("Unity Fog Color"));
						EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
						EditorGUILayout.EndVertical();
						R_EditorGUIUtility.Separator(2);

						if(m_NightAtmosphereMode.enumValueIndex == 1)
						{
							EditorGUILayout.BeginVertical("Box");
							EditorGUILayout.PropertyField(m_UnityMoonFogColor, new GUIContent("Unity Moon Fog Color"));
							EditorGUILayout.HelpBox("Evaluate Curve time by moon above horizon", MessageType.Info);
							EditorGUILayout.EndVertical();
							R_EditorGUIUtility.Separator(2);

						}
						EditorGUILayout.Separator();

						EditorGUILayout.PropertyField(m_UnityFogMode, new GUIContent("Unity Fog Mode"));
						R_EditorGUIUtility.Separator(2);
						EditorGUILayout.Separator();

						if(m_UnityFogMode.enumValueIndex == 0)
						{
							
							EditorGUILayout.PropertyField(m_UnityFogStartDistance, new GUIContent("Unity Fog Start Distance"));
							EditorGUILayout.PropertyField(m_UnityFogEndDistance, new GUIContent("Unity Fog End Distance"));

						}
						else
						{
							EditorGUILayout.PropertyField(m_UnityFogDensity, new GUIContent("Unity Fog Density"));

						}



					}

                EditorGUILayout.Separator();
				//=======================================================================================================



			}


		}

	}

}