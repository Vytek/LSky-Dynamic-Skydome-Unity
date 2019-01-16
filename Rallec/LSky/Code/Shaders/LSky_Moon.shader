////////////////////////////////////
/// LSky
/// =========
///
/// Description:
/// =============
/// Simple moon shader.
////////////////////////////////////

Shader "Rallec/LSky/Moon"
{
	Properties{}

	CGINCLUDE

		// Includes.
		//-------------------------------
		#include "LSky_Common.cginc"
		//===============================

		// Texture
		//----------------------
		sampler2D _MainTex;
		float4    _MainTex_ST;
		//=====================

		// Color
		//------------------------
		uniform fixed _Intensity;
		uniform half4 _Tint;
		uniform half  _ExponentFade;
		//========================

		struct v2f
		{
			float2 texcoord : TEXCOORD0;
			float3 normal   : TEXCOORD1;
			half3  col      : TEXCOORD2;
			float4 vertex   : SV_POSITION;
			//UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert(appdata_base v)
		{

			// Initialize
			//----------------------------------
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//==================================

			// Warning: No VR Tested.
			//----------------------------------------
			//UNITY_SETUP_INSTANCE_ID(v);
			//UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//========================================

			// Position
			//------------------------------------------------------------------------------------
			o.vertex    = UnityObjectToClipPos(v.vertex);
			float yPos  = normalize(mul(lsky_WorldToObject, mul(unity_ObjectToWorld, v.vertex))).y;
			//====================================================================================

			// Normal and uv.
			//-----------------------------------------------------------------------
			o.normal    = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
			o.texcoord  = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
			//=======================================================================

			// Lighting and horizon fade.
			//-------------------------------------------------------------------------------------------------------
			o.col.rgb = _Tint.rgb * saturate(max(0.0, dot(lsky_WorldSunDirection.xyz, o.normal)) * 2.0) * _Intensity;
			//o.col.rgb *= saturate(yPos*10);
			//=======================================================================================================

			return o;
		}

		half4 frag(v2f i) : SV_Target
		{

			// Return final color
			//---------------------------------------------------------
			return half4(i.col.rgb * Exponent3(tex2D(_MainTex, i.texcoord).rgb, _ExponentFade), 1);
			//=========================================================

		}
	ENDCG

	SubShader
	{

		Tags{"Queue"="Background+15" "RenderType"="Background" "IgnoreProjector"="True"}
		//=================================================================================

		Pass
		{

			ZWrite Off
			ZTest LEqual
			Blend One One
			//Blend SrcAlpha OneMinusSrcAlpha
			Fog{ Mode Off }
			//================================

			CGPROGRAM

				#pragma target   2.0
				#pragma vertex   vert
				#pragma fragment frag

			ENDCG
			//===============================
		}
	}
}
