////////////////////////////////////////
/// LSky 
/// ====================
///
/// Description:
/// ====================
/// Galaxy Background
////////////////////////////////////////

Shader "Rallec/LSky/Deep Space/Galaxy Background"
{
	Properties{}
	CGINCLUDE

		// Include
		//------------------------------
		#include "UnityCG.cginc"
		#include "LSky_DeepSpaceCommon.cginc"
		//==============================

	ENDCG

	SubShader
	{

		Tags{ "Queue"="Background+5" "RenderType"="Background" "IgnoreProjector"="True" }
		//====================================================================================

		Pass
		{

			Cull Front
			ZWrite Off
			ZTest LEqual
			Blend One One
			Fog{ Mode Off }
			//==========================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment lsky_galaxy_frag
				#pragma target 2.0

			ENDCG
			//=========================
		}
	}
}
