////////////////////////////////////////
/// LSky
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


		#region |Render And Quality|

		// Renderer
		//---------------------------------------------

		SerializedProperty m_RenderAtmosphere;
		SerializedProperty m_AtmosphereLayerIndex;
		//=============================================


		// Quality
		//---------------------------------------------
		SerializedProperty m_AtmosphereMeshQuality;
		SerializedProperty m_AtmosphereShaderQuality;
		//=============================================
		
		SerializedProperty m_RealtimeBetaMie;
		SerializedProperty m_RealtimeBetaRay;
		SerializedProperty m_AtmosphereSamples;
		//=============================================

		// Model
		//---------------------------------------------
		SerializedProperty m_AtmosphereModel;

		//=============================================

		#endregion

		#region Rayleigh


		// Wavelength
		//-------------------------------------------
		SerializedProperty m_WavelengthR;
		SerializedProperty m_WavelengthG;
		SerializedProperty m_WavelengthB;
		//==========================================


		// Rayleigh
		//------------------------------------------
		SerializedProperty m_RayleighScattering;
		SerializedProperty m_AtmosphereHaziness;
		SerializedProperty m_AtmosphereZenith;
		//==========================================


		// Sun/Day
		//------------------------------------------
		SerializedProperty m_SunBrightness;
		SerializedProperty m_DayAtmosphereTint;
		//=========================================


		// Moon/NIght
		//------------------------------------------
		SerializedProperty m_NightAtmosphereMode;
		SerializedProperty m_MoonBrightness;
		SerializedProperty m_NightAtmosphereTint;
		//=========================================

		#endregion

		#region |Mie|

		// General
		//-------------------------------------
		SerializedProperty m_Mie;
		//=====================================


		// Sun
		//-------------------------------------
		SerializedProperty m_SunMieColor;
		SerializedProperty m_SunMieAnisotropy;
		SerializedProperty m_SunMieScattering;
		//=====================================


		// Moon
		//-------------------------------------

		SerializedProperty m_MoonMieColor;
		SerializedProperty m_MoonMieAnisotropy;
		SerializedProperty m_MoonMieScattering;
		//=====================================

		// Other
		//-------------------------------------
		SerializedProperty m_AtmosphereExponentFade;
		//=====================================

		#endregion


		#region |Foldouts|

		bool m_AtmosphereFoldout;

		#endregion




		protected void InitAtmosphere()
		{

			// Render and quality
			//-------------------------------------------------------------------------------
			m_RenderAtmosphere     = serObj.FindProperty("m_RenderAtmosphere");
		 	m_AtmosphereLayerIndex = serObj.FindProperty("m_AtmosphereLayerIndex");
			//-------------------------------------------------------------------------------
		 	m_AtmosphereMeshQuality  = serObj.FindProperty("m_AtmosphereMeshQuality");
		 	m_AtmosphereShaderQuality = serObj.FindProperty("m_AtmosphereShaderQuality");
			//-------------------------------------------------------------------------------
			m_RealtimeBetaRay   = serObj.FindProperty("m_RealTimeBetaRay");
		 	m_RealtimeBetaMie   = serObj.FindProperty("m_RealTimeBetaMie");
		 	m_AtmosphereSamples = serObj.FindProperty("m_AtmosphereSamples");
			m_AtmosphereModel   = serObj.FindProperty("m_AtmosphereModel");
			//===============================================================================

			// Rayleigh
			//-------------------------------------------------------------------------------
			m_WavelengthR = serObj.FindProperty("m_WavelengthR");
			m_WavelengthG = serObj.FindProperty("m_WavelengthG");
			m_WavelengthB = serObj.FindProperty("m_WavelengthB");
			//==============================================================================

		 	m_RayleighScattering = serObj.FindProperty("m_RayleighScattering");
		 	m_AtmosphereHaziness = serObj.FindProperty("m_AtmosphereHaziness");
		 	m_AtmosphereZenith   = serObj.FindProperty("m_AtmosphereZenith");
			//------------------------------------------------------------------------------
		
		 	m_SunBrightness     = serObj.FindProperty("m_SunBrightness");
		 	m_DayAtmosphereTint = serObj.FindProperty("m_DayAtmosphereTint");
			//------------------------------------------------------------------------------


		 	m_NightAtmosphereMode = serObj.FindProperty("m_NightRayleighMode");
		 	m_MoonBrightness      = serObj.FindProperty("m_MoonBrightness");
		 	m_NightAtmosphereTint = serObj.FindProperty("m_NightAtmosphereTint");
			//==============================================================================

			// Mie
			//------------------------------------------------------------------------------
		 	m_Mie = serObj.FindProperty("m_Mie");

			//------------------------------------------------------------------------------
		 	m_SunMieColor      = serObj.FindProperty("m_SunMieColor");
		 	m_SunMieAnisotropy = serObj.FindProperty("m_SunMieAnisotropy");
		 	m_SunMieScattering = serObj.FindProperty("m_SunMieScattering");
			//==============================================================================

			// Moon
			//------------------------------------------------------------------------------
		 	m_MoonMieColor      = serObj.FindProperty("m_MoonMieColor");
		 	m_MoonMieAnisotropy = serObj.FindProperty("m_MoonMieAnisotropy");
		 	m_MoonMieScattering = serObj.FindProperty("m_MoonMieScattering");
			//==============================================================================

			// Other
			//------------------------------------------------------------------------------
		 	m_AtmosphereExponentFade = serObj.FindProperty("m_AtmosphereExponentFade");
			//==============================================================================


		}


		protected virtual void Atmosphere()
		{

			R_EditorGUIUtility.ShurikenFoldoutHeader("Atmosphere", TextTitleStyle, ref m_AtmosphereFoldout);
            if(m_AtmosphereFoldout)
            {


				R_EditorGUIUtility.ShurikenHeader("Render", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_RenderAtmosphere, new GUIContent("Render Atmosphere"));
					R_EditorGUIUtility.Separator(2);

 					EditorGUILayout.PropertyField(m_AtmosphereLayerIndex, new GUIContent("Atmosphere Layer Index"));
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_AtmosphereModel, new GUIContent("Atmosphere Model"));
					R_EditorGUIUtility.Separator(2);

                EditorGUILayout.Separator();


				// Quality
				//----------------------------------------------------------------------------------------------------------

				R_EditorGUIUtility.ShurikenHeader("Quality Settings", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_AtmosphereMeshQuality, new GUIContent("Atmosphere Mesh Quality"));
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_AtmosphereShaderQuality, new GUIContent("Atmosphere Shader Quality"));
					R_EditorGUIUtility.Separator(2);

					
					if(m_AtmosphereModel.intValue == 0)
					{
						EditorGUILayout.PropertyField(m_RealtimeBetaRay, new GUIContent("Realtime BetaRay"));
						EditorGUILayout.PropertyField(m_RealtimeBetaMie, new GUIContent("Realtime BetaMie"));
						R_EditorGUIUtility.Separator(2);
					}
					else
					{
						EditorGUILayout.PropertyField(m_AtmosphereSamples, new GUIContent("Atmosphere Samples"));
						R_EditorGUIUtility.Separator(2);
					}

                EditorGUILayout.Separator();


				// Rayleigh
				//------------------------------------------------------------------------------------------------------

				R_EditorGUIUtility.ShurikenHeader("Rayleigh", TextSectionStyle, 20);
                EditorGUILayout.Separator();


 					EditorGUILayout.PropertyField(m_WavelengthR, new GUIContent("WavelengthR"));
					EditorGUILayout.PropertyField(m_WavelengthG, new GUIContent("WavelengthG"));
					EditorGUILayout.PropertyField(m_WavelengthB, new GUIContent("WavelengthB"));
					R_EditorGUIUtility.Separator(2);

					if(m_AtmosphereModel.intValue == 0 && !m_RealtimeBetaRay.boolValue)
					{
						if(GUILayout.Button("Compute Beta Ray", GUILayout.Height(25)))
							m_Target.GetBetaRay();
					}

					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_RayleighScattering, new GUIContent("Rayleigh Scattering"));
					EditorGUILayout.HelpBox("Evaluate time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();

					if(m_AtmosphereModel.intValue == 0)
					{
						EditorGUILayout.PropertyField(m_AtmosphereHaziness, new GUIContent("Atmosphere Haziness"));
						EditorGUILayout.PropertyField(m_AtmosphereZenith, new GUIContent("Atmosphere Zenith"));
					}

					R_EditorGUIUtility.Separator(2);


					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_SunBrightness, new GUIContent("Sun Brightness"));
					EditorGUILayout.HelpBox("Evaluate time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_DayAtmosphereTint, new GUIContent("Day Atmosphere Tint"));
					EditorGUILayout.HelpBox("Evaluate gradient by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();

					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_NightAtmosphereMode, new GUIContent("Night Rayleigh Mode"));
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.PropertyField(m_MoonBrightness, new GUIContent("Moon Brightness"));
					EditorGUILayout.PropertyField(m_NightAtmosphereTint, new GUIContent("Night Atmosphere Tint"));
					//EditorGUILayout.HelpBox("Evaluate Gradient time by moon above horizon cycle", MessageType.Info);
					R_EditorGUIUtility.Separator(2);


				// Mie
				//-----------------------------------------------------------------------------------------------------------
				R_EditorGUIUtility.ShurikenHeader("Mie", TextSectionStyle, 20);
                EditorGUILayout.Separator();


 					EditorGUILayout.PropertyField(m_Mie, new GUIContent(m_AtmosphereModel.intValue == 0 ? "Turbidity" : "Mie"));
					R_EditorGUIUtility.Separator(2);

					if(m_AtmosphereModel.intValue == 0 && !m_RealtimeBetaMie.boolValue)
					{
						if(GUILayout.Button("Compute Beta Mie", GUILayout.Height(25)))
							m_Target.GetBetaMie();
					}
					R_EditorGUIUtility.Separator(2);


					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_SunMieColor, new GUIContent("Sun Mie Color"));
					EditorGUILayout.HelpBox("Evaluate Gradient time by sun above horizon", MessageType.Info);
					EditorGUILayout.EndVertical();

					EditorGUILayout.PropertyField(m_SunMieAnisotropy, new GUIContent("Sun Mie Anisotropy"));

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_SunMieScattering, new GUIContent("Sun Mie Scattering"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
					EditorGUILayout.EndVertical();
					R_EditorGUIUtility.Separator(2);

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_MoonMieColor, new GUIContent("Moon Mie Color"));
					EditorGUILayout.HelpBox("Evaluate Gradient time by moon above horizon", MessageType.Info);
					EditorGUILayout.EndVertical();

					EditorGUILayout.PropertyField(m_MoonMieAnisotropy, new GUIContent("Moon Mie Anisotropy"));

					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_MoonMieScattering, new GUIContent("Moon Mie Scattering"));
					EditorGUILayout.HelpBox("Evaluate Curve time by full moon cycle", MessageType.Info);
					EditorGUILayout.EndVertical();
					R_EditorGUIUtility.Separator(2);

                EditorGUILayout.Separator();


				// Other settings.
				//-----------------------------------------------------------------------------------------------------------
				R_EditorGUIUtility.ShurikenHeader("Other Settings", TextSectionStyle, 20);
                EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(m_AtmosphereExponentFade, new GUIContent("Atmosphere Exponent Fade"));
					R_EditorGUIUtility.Separator(2);

                EditorGUILayout.Separator();

			}

		}


	}

}
