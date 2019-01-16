/////////////////////////////////////////////
/// LSky
/// ========
/// 
/// Description:
/// ==============
/// Common for custom inspectors. 
/// 
/////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using Rallec.Utility;

namespace Rallec.LSky
{


    public abstract class LSky_CommonEditor : Editor
    {


        // Serialized Object
        //---------------------------------------------------------------------------------
        protected SerializedObject serObj;
        //=================================================================================


        // Title Styles
        //----------------------------------------------------------------------------------
        protected virtual GUIStyle TextTitleStyle
        {

            get
            {

                GUIStyle style  = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                style.fontSize  = 20;

                return style;
            }
        }
        //===================================================================================

        protected virtual GUIStyle TextSectionStyle
        {

            get
            {

                GUIStyle style  = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                style.fontSize  = 10;

                return style;
            }
        }
        //===================================================================================


        // Title
        //-----------------------------------------------------------------------------------
        protected virtual string Title
        {
            get
            {
                return "New Class";
            }
        }
        //===================================================================================


        // Color
        //-----------------------------------------------------------------------------------
        protected Color green
        { 
            get { return EditorGUIUtility.isProSkin ? Color.green : Color.green * 0.7f; } 
        }

        protected Color red 
        { 
            get { return EditorGUIUtility.isProSkin ? Color.red : Color.red * 0.7f; } 
        }
        //====================================================================================


        // Initialize
        //------------------------------------------------------------------------------------
        protected virtual void OnEnable()
        {

            #region Target.

            serObj = new SerializedObject(target);
       
            #endregion

        }
        //======================================================================================


        // On Inspector GUI
        //--------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serObj.Update();

            R_EditorGUIUtility.ShurikenHeader(Title, TextTitleStyle, 30);

            _OnInspectorGUI();

            serObj.ApplyModifiedProperties();
        }

        protected abstract void _OnInspectorGUI();
        //=======================================================================================
       
    }
}
