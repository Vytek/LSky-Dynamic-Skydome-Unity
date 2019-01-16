
/////////////////////////////////////////
/// LSky
/// =====================
///
/// Description:
/// =============
/// Editor for DateTime manager.
/////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using Rallec.Utility;

namespace Rallec.LSky
{

	[CustomEditor(typeof(LSky_DateTimeManager))]
    public class LSky_DateTimeManagerEditor : LSky_CommonEditor
    {

	    #region target.
        LSky_DateTimeManager tar;

        #endregion

	    #region Time.


        // Timeline
        // =========

        SerializedProperty m_AllowProgressTime;
        SerializedProperty m_Timeline;
        //==========================================


        // Time Length
        // ============

        SerializedProperty m_EnableDayNightLength;
        SerializedProperty m_DayRange;
        SerializedProperty m_DayLength;
        SerializedProperty m_NightLength;
        //===========================================


        // Time
        // ========

        SerializedProperty m_Hour;
        SerializedProperty m_Minute;
        SerializedProperty m_Second;
        SerializedProperty m_Milliseconds;
        //============================================
    
        #endregion
		
		#region Date.
        SerializedProperty m_Day;
        SerializedProperty m_Month;
        SerializedProperty m_Year;

        #endregion

        #region System.
        SerializedProperty m_SyncWithSystem;

        #endregion

		#region Unity Events.
        SerializedProperty OnHourChanged;
        SerializedProperty OnMinuteChanged;
        SerializedProperty OnDayChanged;
        SerializedProperty OnMonthChanged;
        SerializedProperty OnYearChanged;

        #endregion

		#region Foldouts.

        bool
        m_TimeFoldout,
        m_DateFoldout,
        m_OptionsFoldout,
        m_ValueEventsFoldout,
        m_TimeEventsFoldout,
        m_DateEventsFoldout;

        #endregion


		protected override string Title
        {
            get
            {
                return "Date Time Manager";
            }
        }

		protected virtual bool OverrideDayRange
        {
            get { return false; }
        }


        #region |Initialize|
		protected override void OnEnable()
        {

            base.OnEnable();

            #region Target.

            tar = (LSky_DateTimeManager)target;

            #endregion

            #region Time.


            // Timeline.
			//============

            m_AllowProgressTime = serObj.FindProperty("m_AllowProgressTime");
            m_Timeline          = serObj.FindProperty("m_Timeline");
			//======================================================================


            // Length.
			//===========

            m_EnableDayNightLength = serObj.FindProperty("m_EnableDayNightLength");
            m_DayRange          = serObj.FindProperty("m_DayRange");
            m_DayLength         = serObj.FindProperty("m_DayLength");
            m_NightLength       = serObj.FindProperty("m_NightLength");
			//======================================================================


            // Time.
			//=========

            m_Hour   = serObj.FindProperty("m_Hour");
            m_Minute = serObj.FindProperty("m_Minute");
            m_Second = serObj.FindProperty("m_Second");
            m_Milliseconds = serObj.FindProperty("m_Milliseconds");
			//======================================================================

            #endregion

            #region Date

            m_Day   = serObj.FindProperty("m_Day");
            m_Month = serObj.FindProperty("m_Month");
            m_Year  = serObj.FindProperty("m_Year");

            #endregion

            #region System.

            m_SyncWithSystem = serObj.FindProperty("m_SyncWithSystem");

            #endregion

            #region Unity Events.


            // DateTime.
			//===========

            OnHourChanged   = serObj.FindProperty("OnHourChanged");
            OnMinuteChanged = serObj.FindProperty("OnMinuteChanged");
            OnDayChanged    = serObj.FindProperty("OnDayChanged");
            OnMonthChanged  = serObj.FindProperty("OnMonthChanged");
            OnYearChanged   = serObj.FindProperty("OnYearChanged");

            #endregion

        }
        #endregion


        #region |OnInspectorGUI|
		protected override void _OnInspectorGUI()
		{

			#region Time.

			R_EditorGUIUtility.ShurikenFoldoutHeader("Time", TextTitleStyle, ref m_TimeFoldout);

            if(m_TimeFoldout)
            {

                R_EditorGUIUtility.ShurikenHeader("Timeline", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    GUI.backgroundColor = m_AllowProgressTime.boolValue ? green : red;

                    EditorGUILayout.PropertyField(m_AllowProgressTime, new GUIContent("Allow Progress Time"));

                    EditorGUILayout.BeginVertical();
                        EditorGUILayout.PropertyField(m_Timeline, new GUIContent("Timeline"));
                    EditorGUILayout.EndVertical();

                    GUI.backgroundColor = Color.white;

                EditorGUILayout.Separator();


                R_EditorGUIUtility.ShurikenHeader("Time Length", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    if (!m_SyncWithSystem.boolValue)
                    {


                        GUI.backgroundColor = m_EnableDayNightLength.boolValue ? green : red;

                        EditorGUILayout.PropertyField(m_EnableDayNightLength, new GUIContent("Enable Day Night Length"));

                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        if (m_EnableDayNightLength.boolValue)
                        {

                            
                            if (!OverrideDayRange)
                            {

                                // Day Range.
                                float min = m_DayRange.vector2Value.x;
                                float max = m_DayRange.vector2Value.y;

                                R_EditorGUIUtility.ShurikenHeader("Day Range", TextSectionStyle, 20);
                                
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.MinMaxSlider(ref min, ref max, 0, 24);

                                    m_DayRange.vector2Value = new Vector2(min, max);

                                    EditorGUILayout.PropertyField(m_DayRange, new GUIContent(""));
                                }
                                EditorGUILayout.EndHorizontal();
								
                                EditorGUILayout.Separator();
                            }
                            R_EditorGUIUtility.Separator(2);
                            EditorGUILayout.PropertyField(m_DayLength, new GUIContent("Day In Minutes"));
                            EditorGUILayout.PropertyField(m_NightLength, new GUIContent("Night In Minutes"));
                            
                        }
                        else
                        {

                            EditorGUILayout.PropertyField(m_DayLength, new GUIContent("Day In Minutes"));

                        }
                        EditorGUILayout.Separator();
                        EditorGUILayout.EndVertical();


                        R_EditorGUIUtility.ShurikenHeader("Set Time", TextSectionStyle, 20);

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            EditorGUILayout.PropertyField(m_Hour, new GUIContent("Hour"));
                            EditorGUILayout.PropertyField(m_Minute, new GUIContent("Minute"));
                            EditorGUILayout.PropertyField(m_Second, new GUIContent("Second"));
                            EditorGUILayout.PropertyField(m_Milliseconds, new GUIContent("Milliseconds"));

                            GUI.backgroundColor = green;
                            if (GUILayout.Button("Set Time", GUILayout.MinHeight(30)))
                            {

                                tar.SetHour(m_Hour.intValue);
                                tar.SetMinute(m_Minute.intValue);
                                tar.SetSecond(m_Second.intValue);
                                tar.SetMillisecond(m_Milliseconds.intValue);

                            }
                            GUI.backgroundColor = Color.white;
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Separator();
                    }
                EditorGUILayout.Separator();

            }


			#endregion

            #region Date

            R_EditorGUIUtility.ShurikenFoldoutHeader("Date", TextTitleStyle, ref m_DateFoldout);
            if(m_DateFoldout)
            {


                EditorGUILayout.PropertyField(m_Day, new GUIContent("Day"));
                EditorGUILayout.PropertyField(m_Month, new GUIContent("Month"));
                EditorGUILayout.PropertyField(m_Year, new GUIContent("Year"));

            }

            #endregion

            #region System.

            R_EditorGUIUtility.ShurikenFoldoutHeader("System", TextTitleStyle, ref m_OptionsFoldout);
            if (m_OptionsFoldout)
            {

                GUI.backgroundColor = m_SyncWithSystem.boolValue ? Color.green : Color.red;
                EditorGUILayout.Separator();
                    EditorGUILayout.PropertyField(m_SyncWithSystem, new GUIContent("Synchronize With System"));
                EditorGUILayout.Separator();
                GUI.backgroundColor = Color.white;

            }
            #endregion

			LastUpdateInspector();

		}

		protected virtual void LastUpdateInspector()
        {


            #region |Unity Events|

            R_EditorGUIUtility.ShurikenFoldoutHeader("Time Events", TextTitleStyle, ref m_TimeEventsFoldout);

            if (m_TimeEventsFoldout)
            {
                EditorGUILayout.PropertyField(OnHourChanged, new GUIContent("On Hour Changed"));
                EditorGUILayout.PropertyField(OnMinuteChanged, new GUIContent("On Minute Changed"));
            }
			//----------------------------------------------------------------------------------------------------

            R_EditorGUIUtility.ShurikenFoldoutHeader("Date Events", TextTitleStyle, ref m_DateEventsFoldout);
            if (m_DateEventsFoldout)
            {
                EditorGUILayout.PropertyField(OnDayChanged, new GUIContent("On Day Changed"));
                EditorGUILayout.PropertyField(OnMonthChanged, new GUIContent("On Month Changed"));
                EditorGUILayout.PropertyField(OnYearChanged, new GUIContent("On Year Changed"));
            }

            #endregion

        }

        #endregion


	}


}