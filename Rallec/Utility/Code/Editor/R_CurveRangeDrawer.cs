//////////////////////////////////////
/// Utility.
/// =========
///
/// Description: 
/// =============
/// Range for AnimationCurve Field.
//////////////////////////////////////

using UnityEngine; 
using UnityEditor;

namespace Rallec.Utility
{
	[CustomPropertyDrawer(typeof(R_CurveRange))] 
	public class R_CurveRangeDrawer : PropertyDrawer
	{

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			
			R_CurveRange attr = attribute as R_CurveRange;

			if(property.propertyType == SerializedPropertyType.AnimationCurve)
				EditorGUI.CurveField(rect, property, Color.white, new Rect(attr.timeStart, attr.valueStart, attr.timeEnd, attr.valueEnd));
			else
				EditorGUI.HelpBox(rect, "Only use with Animation Curves", MessageType.Warning);
			
		}
	}
}