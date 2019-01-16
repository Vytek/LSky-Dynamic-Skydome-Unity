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

	[CustomEditor(typeof(LSky_Skydome))]
	public partial class LSky_SkydomeEditor : LSky_CommonEditor
	{


		#region |Target|
		LSky_Skydome m_Target = null;

		#endregion

		#region |General Settings|


		// Color Correction
		//--------------------------------------------
		SerializedProperty m_DomeRadius;
        SerializedProperty m_ApplyFastTonemaping;
        SerializedProperty m_Exposure;
		//============================================

        #endregion

		#region |Foldouts|

		bool m_GeneralSettingsFoldout;

		#endregion

		#region |Title|
		protected override string Title
        {
            get
            {
                return "Skydome";
            }
        }
		//============================================

		#endregion


		protected override void OnEnable()
        {

            base.OnEnable();

            // Get target class.
			 m_Target = (LSky_Skydome)target;

			// Find resources fields
			InitResources();

			// Find general settings fields
			InitGeneralSettings();

			// Find celestials fields
			InitCelestials();
			
			// Find atmosphere fields
			InitAtmosphere();

			// Find clouds fields
			InitClouds();

			// Find lighting fields
			InitLighting();

        }

		protected virtual void InitGeneralSettings()
		{

		
			// Color correction
			//-------------------------------------------------------------------------
			m_DomeRadius          = serObj.FindProperty("m_DomeRadius");
			m_ApplyFastTonemaping = serObj.FindProperty("m_ApplyFastTonemaping");
			m_Exposure            = serObj.FindProperty("m_Exposure");
			//=========================================================================

		}

		protected override void _OnInspectorGUI()
        {

		   // Resources and components.
		   Resources();

		   // General Settings.
           GeneralSettings();

		   // Celestials
		   Celestials();

		   // Atmosphere
		   Atmosphere();

		   // Clouds
		   Clouds();

		   // Lighting
		   Lighting();

        }

		protected void GeneralSettings()
		{

			R_EditorGUIUtility.ShurikenFoldoutHeader("General Settings", TextTitleStyle, ref m_GeneralSettingsFoldout);
            if (m_GeneralSettingsFoldout)
            {


				// Color Correction
				// =================

                R_EditorGUIUtility.ShurikenHeader("Color Correction", TextSectionStyle, 20);
                EditorGUILayout.Separator();

				  	EditorGUILayout.PropertyField(m_DomeRadius, new GUIContent("Dome Radius"));
                    R_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    //GUI.backgroundColor = m_HDR.boolValue ? green : red;
                    EditorGUILayout.PropertyField(m_ApplyFastTonemaping, new GUIContent("Apply Fast Tonemaping"));
                   // GUI.backgroundColor = Color.white;
                    R_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_Exposure, new GUIContent("Exposure"));
					//EditorGUILayout.HelpBox("Evaluate Curve time by full sun cycle", MessageType.Info);
                    R_EditorGUIUtility.Separator(2);
                EditorGUILayout.Separator();
				//===========================================================================================================

            }

		}

	}

}