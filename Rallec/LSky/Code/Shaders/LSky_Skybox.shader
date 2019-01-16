////////////////////////////////////////////////////////////
/// LSKy
/// ================
///
/// Description:
/// =================
/// Atmbient skybox
////////////////////////////////////////////////////////////

Shader "Rallec/LSky/Ambient Skybox"
{
	Properties{}
	CGINCLUDE

		// Includes
		//-----------------------------------------
		#include "LSky_AtmosphericScattering.cginc"
		//=========================================

		// Ground.
		//---------------------------------
		uniform half3 lsky_GroundColor;
		//=================================

		struct appdata
		{
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{

			fixed3 color  : TEXCOORD0;
			float4 vertex : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert(appdata_base v)
		{

			// Init
			//-----------------------------------------------
			v2f o; 
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//===============================================

			//
			//-----------------------------------------------
	 		 UNITY_SETUP_INSTANCE_ID(v);
			 UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//===============================================

			// Position
			//-----------------------------------------------
			o.vertex       = UnityObjectToClipPos(v.vertex);
			float3 nvertex = normalize(v.vertex.xyz);
			//===============================================

			// Compute dot product of the sun and moon.
			//-------------------------------------------------------------------------------------------------------------------------
			float2 cosTheta = float2(dot(nvertex.xyz, lsky_WorldSunDirection.xyz), dot(nvertex.xyz, lsky_WorldMoonDirection.xyz));
			//=========================================================================================================================

			#ifdef LSKY_PREETHAM_ATMOSPHERE_MODEL

			// Compute Optical Depth.
			//-------------------------------------------------------------------------------------------------------------------------
			float2 srm;
			CustomOpticalDepth(nvertex.y, srm.xy);
			//=========================================================================================================================

			// Compute rayleigh Scattering.
			//-------------------------------------------------------------------------------------------------------------------------
			o.color.rgb =  RayleighScattering(srm.xy, cosTheta.xy);
			//=========================================================================================================================

			#else

			// Compute rayleigh scattering.
			//-------------------------------------------------------------------------------------------------------------------------
			half3 inscatter; half4 outscatter;

			// Get ray.
			float3 ray = float3(nvertex.x, saturate(nvertex.y), nvertex.z);

			// Compute atmospheric scattering.
			ComputeRayleighScattering(ray.xyz, inscatter);
		
			// Add inscattering to color.
			o.color = inscatter;
			//========================================================================================================================

			#endif

			fixed3 groundColor = lsky_GroundColor;

			// Apply color correction.
			AtmosphereColorCorrection(o.color.rgb, groundColor);

			o.color.rgb = lerp(o.color.rgb, groundColor, saturate(-nvertex.y*100));
			//========================================================================================================================

			
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			return fixed4(i.color.rgb, 1);
			//============================
		}
	ENDCG

	SubShader
	{

		Tags {"Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox"}
		//===================================================================================================

		Pass
		{

			Cull Off
			ZWrite Off
			//ZTest LEqual
			//Blend One One
			//Fog{ Mode Off }
			//===============================================================================================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0

				#pragma multi_compile __ LSKY_APPLY_FAST_TONEMAPING
				#pragma multi_compile __ LSKY_ENABLE_MOON_SCATTER
				#pragma multi_compile __ LSKY_PREETHAM_ATMOSPHERE_MODEL


			ENDCG
			//===============================================================================================
		}
	}
}
