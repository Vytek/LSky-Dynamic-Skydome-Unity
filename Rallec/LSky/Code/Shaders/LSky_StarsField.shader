////////////////////////////////////
/// LSky
/// ========
///
/// Description:
/// =============
/// Stars field shader.
////////////////////////////////////

Shader "Rallec/LSky/Deep Space/Stars Field Cubemap"
{
	//Properties{}
	CGINCLUDE

		// Includes
		//------------------------------------
		#include "UnityCG.cginc"
		#include "LSky_DeepSpaceCommon.cginc"
		//====================================

		#define LSKY_ENABLE_STARS_SCINTILLATION 1

		
	ENDCG

	SubShader
	{

		Tags{ "Queue"="Background+10" "RenderType"="Background" "IgnoreProjector"="True" }
		//====================================================================================

		Pass
		{

			Cull Front
			ZWrite Off
			ZTest LEqual
			Blend One One
			Fog{ Mode Off }
			//=========================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment lsky_starsfield_frag
				#pragma target 2.0

			ENDCG
			//=========================
		}
	}
}
