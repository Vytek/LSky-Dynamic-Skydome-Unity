///////////////////////////////////////////////////
/// Utility.
/// =========
///
/// Description: 
/// ==============
/// Range for AnimationCurve field.
///////////////////////////////////////////////////

using System; 
using UnityEngine;

namespace Rallec.Utility
{

	public class R_CurveRange : PropertyAttribute
	{
		public readonly float timeStart;
		public readonly float valueStart;
		public readonly float timeEnd; 
		public readonly float valueEnd; 

		public R_CurveRange(float _timeStart, float _valueStart, float _timeEnd, float _valueEnd)
		{
			this.timeStart  = _timeStart; 
			this.valueStart = _valueStart;
			this.timeEnd    = _timeEnd;   
			this.valueEnd   = _valueEnd;
		}
	}
}