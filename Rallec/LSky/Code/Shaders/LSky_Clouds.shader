/////////////////////////////////////////////
/// LSKy
/// ================
///
/// Description:
/// ================
/// Simple clouds.
/////////////////////////////////////////////


Shader "Rallec/LSky/Clouds"
{

	Properties{}
	CGINCLUDE

		// Includes
		//--------------------------
		#include "LSky_Common.cginc"
		//==========================

		// Texture
		//----------------------
		sampler2D  _MainTex;
		float4     _MainTex_ST;
		//======================

		// Color
		//-------------------------
		uniform half  _Intensity;
		uniform half4 _Tint;
		//=========================

		// Noise
		//---------------------------------
		uniform half lsky_CloudsDensity;
		uniform half  lsky_CloudsCoverage;
		//=================================

		struct v2f
		{
			float2 texcoord : TEXCOORD0;
			half4  col      : TEXCOORD2;
			float4 vertex   : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert(appdata_base v)
		{

			// Init
			//--------------------------------------
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//======================================

			//
			//-------------------------------------------
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//===========================================

			// Position - coords - horizon fade.
			//--------------------------------------------------------
			o.vertex   = UnityObjectToClipPos(v.vertex);
			#ifdef UNITY_REVERSED_Z
	  		o.vertex.z = 1e-5f;
  			#else
	  		o.vertex.z = o.vertex.w - 1e-5f;
  			#endif
			//--------------------------------------------------------
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.col.rgb  = _Tint.rgb * _Intensity;
			o.col.a    = normalize(v.vertex.xyz).y * 10;
			//========================================================

			return o;
		}

		
		half4 frag(v2f i) : SV_Target
		{

			half noise = tex2D(_MainTex, i.texcoord).r;

			half4 col = half4(i.col.rgb * (1.0 - (noise-lsky_CloudsCoverage) * 0.5), saturate(  smoothstep(noise,0,lsky_CloudsCoverage) * lsky_CloudsDensity) * i.col.a );

			//half4 col = half4(i.col.rgb * (1.0 - (noise-lsky_CloudsCoverage) * 0.5), saturate((noise-lsky_CloudsCoverage) * lsky_CloudsDensity) * i.col.a );

			return saturate(col);
		}


	ENDCG


		SubShader
		{

			Tags{"Queue"="Background+1100" "RenderType"="Background" "IgnoreProjector"="True"}
			//==============================================================================================

			Pass
			{

				Cull Front
				//ZWrite Off
				//ZTest LEqual
				//Blend One One
				Blend SrcAlpha OneMinusSrcAlpha
				Fog{ Mode Off }
				//==========================================================================================

				CGPROGRAM

					#pragma target   2.0
					#pragma vertex   vert
					#pragma fragment frag

				ENDCG

				//==========================================================================================
			}

			
			


		}

}
