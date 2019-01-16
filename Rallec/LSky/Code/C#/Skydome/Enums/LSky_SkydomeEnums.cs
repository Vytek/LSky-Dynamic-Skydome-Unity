///////////////////////////////////////
/// LSky 
/// ====================
///
/// Description:
/// //============
/// Enums for skydome. 
///////////////////////////////////////

using System; 
using UnityEngine;

namespace Rallec.LSky
{

	#region |Quality|

	public enum LSky_Quality2
	{
		Low = 0,
		High = 1
	}

	public enum LSky_Quality3
	{
		Low = 0,
		Medium = 1,
		High = 2
	}

	public enum LSky_ShaderQuality
	{

		PerVertex = 0,
		PerPixel = 1
		
	}
	#endregion

	#region |Atmosphere|

	public enum LSky_AtmosphereModel 
	{
		Preetham = 0,
		Oneil = 1
	}

	public enum LSky_NightRayleighMode
	{
		OppositeSun = 0,
		Moon = 1,
		Off = 2
	}
	#endregion
}