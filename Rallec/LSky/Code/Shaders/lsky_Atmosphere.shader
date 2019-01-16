

////////////////////////////////////////////////////////////
/// LSKy
/// ================
///
/// Description:
/// =================
/// Atmosphere shader.
////////////////////////////////////////////////////////////

Shader "Rallec/LSky/Atmosphere"
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
			
			
			float3 nvertex  : TEXCOORD0;

			#ifndef LSKY_PER_PIXEL_ATMOSPHERE

			half3  color : TEXCOORD1;

			#endif
			//----------------------------------------------------------------------------------

			
			#if !defined(LSKY_PREETHAM_ATMOSPHERE_MODEL) && defined(LSKY_PER_PIXEL_ATMOSPHERE)

			half3 inscatter  : TEXCOORD1;
			half4 outscatter : TEXCOORD2;

			#endif
			//----------------------------------------------------------------------------------

			float4 vertex : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO
		};


		v2f vert(appdata v)
		{

			// Initialized.
			//--------------------------------------
			v2f o; 
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//======================================

			// Warning: No VR Tested
			//-------------------------------------------
	 		UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//===========================================

			// Position
			//------------------------------------------
			o.vertex = UnityObjectToClipPos(v.vertex);
			//------------------------------------------

			#ifdef UNITY_REVERSED_Z
	  		o.vertex.z = 1e-5f;
  			#else
	  		o.vertex.z = o.vertex.w - 1e-5f;
  			#endif
			//------------------------------------------
			o.nvertex.xyz = normalize(v.vertex.xyz);
			//==========================================

			// Compute dot product of the sun and moon.
			//-------------------------------------------------------------------------------------------------------------------------
			float2 cosTheta = float2(dot(o.nvertex.xyz, lsky_WorldSunDirection.xyz), dot(o.nvertex.xyz, lsky_WorldMoonDirection.xyz));
			//=========================================================================================================================


			#ifdef LSKY_PREETHAM_ATMOSPHERE_MODEL

			#ifndef LSKY_PER_PIXEL_ATMOSPHERE

			// Compute Optical Depth.
			//-------------------------------------------------------------------------------------------------------------------------
			float2 srm;
			CustomOpticalDepth(o.nvertex.y, srm.xy);
			//=========================================================================================================================

			// Compute Atmospheric Scattering.
			//-------------------------------------------------------------------------------------------------------------------------
			o.color.rgb =  AtmosphericScattering(srm.xy, cosTheta.xy);
			//=========================================================================================================================
	
			#endif

			#else
			
			// Oneil.
			#ifndef LSKY_PER_PIXEL_ATMOSPHERE

			// Compute atmospheric scattering.
			//---------------------------------------------------------------------------------------------------------------------------------------------
			half3 inscatter; half4 outscatter;

			// Get ray.
			float3 ray = float3(o.nvertex.x, saturate(o.nvertex.y), o.nvertex.z);

			// Compute atmospheric scattering.
			ComputeAtmosphericScattering(ray.xyz, inscatter, outscatter);
		
			// Add inscattering to color.
			o.color = inscatter;

			// Add sun mie phase and multiply for outScattering rgb channels.
			o.color += PartialMiePhase(cosTheta.x, lsky_PartialSunMiePhase, lsky_SunMieScattering) * lsky_SunMieColor.rgb * outscatter.rgb;

			// Add moon mie phase and multiply for outScattering a channel.
			o.color += PartialMiePhaseSimplified(cosTheta.y, lsky_PartialMoonMiePhase, lsky_MoonMieScattering) * lsky_MoonMieColor.rgb * outscatter.a;
			//=============================================================================================================================================

			#endif


			#endif

			#ifndef LSKY_PER_PIXEL_ATMOSPHERE

			// Apply color correction.
			AtmosphereColorCorrection(o.color.rgb);
			
			#endif

			return o;
		}

		half4 frag(v2f i) : SV_Target
		{

			half4 color = half4(0,0,0,1);


			#ifdef LSKY_PER_PIXEL_ATMOSPHERE

			// Compute dot product of the sun and moon.
			//-------------------------------------------------------------------------------------------------------------------------
			float2 cosTheta = float2(dot(i.nvertex.xyz, lsky_WorldSunDirection.xyz), dot(i.nvertex.xyz, lsky_WorldMoonDirection.xyz));
			//=========================================================================================================================


			#ifdef LSKY_PREETHAM_ATMOSPHERE_MODEL

			// Compute Optical Depth.
			//-----------------------------------------------------------------------------------------------------------------
			float2 srm;
			CustomOpticalDepth(i.nvertex.y, srm.xy);
			//=================================================================================================================

			// Compute Atmospheric Scattering.
			//-----------------------------------------------------------------------------------------------------------------
			color.rgb = AtmosphericScattering(srm.xy, cosTheta.xy);
			//=================================================================================================================

			#else

			// Compute Atmospheric Scattering.
			//----------------------------------------------------------------------------------------------------------------------------------------

			// Get ray.
			float3 ray = float3(i.nvertex.x, saturate(i.nvertex.y), i.nvertex.z);
		
			
			// Compute atmospheric scattering.
			ComputeAtmosphericScattering(ray, i.inscatter, i.outscatter);

			// Add inscattering to final color.
			//if (i.nvertex.y > 0)
			color.rgb = i.inscatter;

			// Add sun mie phase and multiply for outScattering rgb channels.
			color.rgb += PartialMiePhase(cosTheta.x, lsky_PartialSunMiePhase, lsky_SunMieScattering)* lsky_SunMieColor * i.outscatter.rgb;

			// Add moon mie phase and multiply for outScattering a channel.
			color.rgb += PartialMiePhaseSimplified(cosTheta.y, lsky_PartialMoonMiePhase, lsky_MoonMieScattering) * lsky_MoonMieColor * i.outscatter.a;
			//=========================================================================================================================================

			#endif

			// Apply color correction.
			AtmosphereColorCorrection(color.rgb);

			#else
			
			// Get final color.
			//if (i.nvertex.y > 0)
			color.rgb = i.color.rgb;

			#endif

			//color.rgb *= saturate(i.nvertex.y*100);

			return color;
		}


	ENDCG

	SubShader
	{

		Tags {"Queue"="Background+1050" "RenderType"="Background" "IgnoreProjector"="True"}
		//===================================================================================================

		Pass
		{

			Cull Front
			ZWrite Off
			ZTest LEqual
			Blend One One
			Fog{ Mode Off }
			//===============================================================================================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0

				#pragma multi_compile __ LSKY_APPLY_FAST_TONEMAPING
				#pragma multi_compile __ LSKY_ENABLE_NIGHT_RAYLEIGH
				#pragma multi_compile __ LSKY_PER_PIXEL_ATMOSPHERE
				#pragma multi_compile __ LSKY_PREETHAM_ATMOSPHERE_MODEL
			

			ENDCG
			//===============================================================================================
		}
	}
}
