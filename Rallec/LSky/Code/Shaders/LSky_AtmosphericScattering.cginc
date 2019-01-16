////////////////////////////////////////
/// LSKY Dynamic SKy
/// ====================
///
/// Description:
/// =============
/// Atmospheric Scattering Calculations.
/// 2 classics models.
////////////////////////////////////////

#ifndef LSKY_ATMOSPHERIC_SCATTERING
#define LSKY_ATMOSPHERIC_SCATTERING

// Includes
//------------------------------
#include "LSky_Common.cginc"
//==============================


//------------------------------------------------ Constants -------------------------------------------------
//============================================================================================================

// PI
//-------------------------------------------------
#define lsky_PI14  0.079577f  // 1 / (4*pi).
#define lsky_PI316 0.059683f  // 3 / (16 * pi).
//=================================================



//------------------------------------------------ Mie Phase -------------------------------------------------
//============================================================================================================


// Sun Mie.
//-----------------------------------------
uniform float3 lsky_PartialSunMiePhase;
uniform half   lsky_SunMieScattering;
uniform half3  lsky_SunMieColor;
//=========================================

// Moon Mie.
//----------------------------------------
uniform half3 lsky_PartialMoonMiePhase;
uniform half  lsky_MoonMieScattering;
uniform half3 lsky_MoonMieColor;
//========================================

// Mie Phase functions.
//------------------------------------------------------------------------------------------------------------

// Simplified mie phase function for moon.
inline half3 PartialMiePhaseSimplified(float cosTheta, half3 partialMiePhase, half scattering)
{
	return(lsky_PI14 * (partialMiePhase.x / (partialMiePhase.y - (partialMiePhase.z * cosTheta)))) *
		scattering;
}
//============================================================================================================

// Henyey Greenstein phase function based on Cornette Sharks with small changes.
//------------------------------------------------------------------------------------------------------------
inline half3 PartialMiePhase(float cosTheta, half3 partialMiePhase, half scattering)
{
	return
	(
		1.5 * partialMiePhase.x * ((1.0 + cosTheta * cosTheta) *
		pow(partialMiePhase.y - (partialMiePhase.z * cosTheta), -1.5))
	) * scattering;
}
//============================================================================================================


//------------------------------------------ General Atmosphere  ---------------------------------------------
//============================================================================================================


uniform half3 lsky_SunAtmosphereTint;
uniform half3 lsky_MoonAtmosphereTint;
//=====================================

uniform half lsky_AtmosphereExponentFade;
//=====================================

uniform int lsky_NightRayleighMode;
//=====================================

uniform half lsky_ExponentFade;
//=====================================


// Color Correction.
//------------------------------------------------------------------------------------------------------------

inline void AtmosphereColorCorrection(inout half3 col)
{

  	// HDR.
  	//--------------------------------------------------------------
  	#if defined(LSKY_APPLY_FAST_TONEMAPING)
  	col.rgb = fastTonemaping(col.rgb, lsky_Exposure);
  	#else
  	col.rgb *= lsky_Exposure;
  	#endif
  	//==============================================================

	// Color exponent
  	//---------------------------------------------------------------
  	col.rgb = Exponent2(col.rgb, lsky_AtmosphereExponentFade);
  	//===============================================================


  	// Color space
  	// -------------------------------------------------------------
  	#if defined(UNITY_COLORSPACE_GAMMA)
	col.rgb = GAMMA_TO_LINEAR(col.rgb);
	#endif
  	//--------------------------------------------------------------
}


inline void AtmosphereColorCorrection(inout half3 col, inout half3 groundCol)
{

  	// HDR.
  	//--------------------------------------------------------------
  	#if defined(LSKY_APPLY_FAST_TONEMAPING)
  	col.rgb = fastTonemaping(col.rgb, lsky_Exposure);
  	#else
  	col.rgb *= lsky_Exposure;
  	#endif
  	//==============================================================

	// Color exponent
  	//---------------------------------------------------------------
  	col.rgb = Exponent2(col.rgb, lsky_AtmosphereExponentFade);
  	//===============================================================


  	// Color space
  	// -------------------------------------------------------------
  	#if defined(UNITY_COLORSPACE_GAMMA)
	col.rgb = GAMMA_TO_LINEAR(col.rgb);
	#else
	groundCol *= groundCol;
	#endif
  	//==============================================================
}


//-------------------------------------------  Atmosphere Model ----------------------------------------------
//============================================================================================================

// Compute rayleigh phase
//-----------------------------------------------------
inline half RayleighPhase(float cosTheta)
{

	#ifndef LSKY_PREETHAM_ATMOSPHERE_MODEL
	return 0.75 + 0.75 * (1.0 + cosTheta * cosTheta);
	#else
	return lsky_PI316 * (1.0 + (cosTheta * cosTheta));
	//return (3.0/4.0) * (1.9+(cosTheta*cosTheta));
	#endif
}
//=====================================================


#ifdef LSKY_PREETHAM_ATMOSPHERE_MODEL

// This Atmosphere Model is based on Preetham and Hoflman papers.
// ===========================================================================================================

//-----------------------------------------
uniform float  lsky_AtmosphereZenith;
uniform half   lsky_AtmosphereHaziness;
uniform float3 lsky_BetaMie;
uniform float3 lsky_BetaRay;
uniform half   lsky_HorizonColorFade;
uniform half   lsky_SunE;
uniform half   lsky_MoonE;
//=========================================


// Optical depth.
//-----------------------------------------------------------------------------------------------

#define LSKY_RAYLEIGH_ZENITH_LENGTH 8.4e3
#define LSKY_MIE_ZENITH_LENGTH 1.25e3


// Defautl Optical Depth.
inline void OpticalDepth(float y, inout float2 srm)
{

	y = saturate(y);
	//------------------------------------------------------------------------------------------
	float3 zenith = acos(y);
	zenith        = (cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180.0) / UNITY_PI), -1.253));
	zenith        = 1.0/zenith;
	//------------------------------------------------------------------------------------------

	srm.x = LSKY_RAYLEIGH_ZENITH_LENGTH*zenith;
	srm.y = LSKY_MIE_ZENITH_LENGTH*zenith;
	//------------------------------------------------------------------------------------------
}


// Based in Nielsen paper, See Documentation: References.
inline void NielsenOpticalDepth(float y, inout float2 srm)
{
	y = saturate(y);
	//-----------------------------------------------------------

	float f = pow(y, lsky_AtmosphereHaziness); // h 1 / 5.0 0.25
	float t = (1.05 - f) * 190000;
	//-----------------------------------------------------------

	srm.x = t + f * (LSKY_RAYLEIGH_ZENITH_LENGTH - t);
	srm.y = t + f * (LSKY_MIE_ZENITH_LENGTH - t);
	//-----------------------------------------------------------
}

// Custom optical depth for more control.
inline void CustomOpticalDepth(float y, inout half2 srm)
{

	y = saturate(y * lsky_AtmosphereHaziness);
	y = 1.0 / (y + lsky_AtmosphereZenith); // zenith.
	//-----------------------------------------------------
	srm.x = LSKY_RAYLEIGH_ZENITH_LENGTH*y;
	srm.y = LSKY_MIE_ZENITH_LENGTH*y;
}
//=============================================================================================


inline half3 RayleighScattering(float2 srm,  float2 cosTheta)
{

	// Compute combine extinction factor.
	// ---------------------------------------------------------------------------------------------------------

	// Combined extinction factor.
	half3 fex = exp(-(lsky_BetaRay * srm.x + lsky_BetaMie * srm.y)); // No orange color.

	// Final combined extinction factor with orange color to sunset/dawn.
	half3 combExcFac = saturate(lerp((1.0 - fex), (1.0 - fex) * (fex), lsky_HorizonColorFade));
	//-==========================================================================================================

	// Sun/Day.
	//----------------------------------------------------------------------------------------------------------
	half3 sunBRT   = lsky_BetaRay * RayleighPhase(cosTheta.x);
	half3 sun_BRMT = sunBRT / lsky_BetaRay;
	half3 sunScatter = lsky_SunE * (sun_BRMT * combExcFac) * lsky_SunAtmosphereTint;
	//==========================================================================================================

	// Moon/Night.
	//----------------------------------------------------------------------------------------------------------
	#ifdef LSKY_ENABLE_NIGHT_RAYLEIGH

	// It has been simplified to make it more optimal.
	half3 moonScatter = lsky_MoonE * ((1.0 - fex)) * lsky_MoonAtmosphereTint;
	return sunScatter + moonScatter;

	#else

	return sunScatter;

	#endif
	//===========================================================================================================

}

inline float3 AtmosphericScattering(float2 srm, float2 cosTheta)
{
	

	// Compute combine extinction factor.
	// ---------------------------------------------------------------------------------------------------------

	// Combined extinction factor.
	float3 fex = exp(-(lsky_BetaRay * srm.x + lsky_BetaMie * srm.y)); // No orange color.

	// Final combined extinction factor with orange color to sunset/dawn.
	float3 combExcFac = saturate(lerp((1.0 - fex), (1.0 - fex) * (fex), lsky_HorizonColorFade));
	//==========================================================================================================

	// Sun/Day.
	//----------------------------------------------------------------------------------------------------------
	float3 sunBRT = lsky_BetaRay * RayleighPhase(cosTheta.x);
	float3 sunBMT = PartialMiePhase(cosTheta.x, lsky_PartialSunMiePhase, lsky_SunMieScattering) * lsky_SunMieColor;
	sunBMT *= lsky_BetaMie;
	//===========================================================================================================

	float3 sun_BRMT = (sunBRT + sunBMT) / (lsky_BetaRay + lsky_BetaMie);
	float3 sunScatter = lsky_SunE * (sun_BRMT * combExcFac) * lsky_SunAtmosphereTint;
	//===========================================================================================================


	// Moon/Night.
	//----------------------------------------------------------------------------------------------------------
	#ifdef LSKY_ENABLE_NIGHT_RAYLEIGH

	// It has been simplified to make it more optimal.
	half3 moonScatter = lsky_MoonE * ((1.0 - fex)) * lsky_MoonAtmosphereTint;
	moonScatter += PartialMiePhaseSimplified(cosTheta.y, lsky_PartialMoonMiePhase, lsky_MoonMieScattering) * lsky_MoonMieColor;

	return sunScatter + moonScatter;

	#else

	return sunScatter+PartialMiePhaseSimplified(cosTheta.y, lsky_PartialMoonMiePhase, lsky_MoonMieScattering) * lsky_MoonMieColor;

	#endif

}


#else


// This Atmosphere Model is based on Sean O'neil(gpu gems2) and Unity Procedural Skybox.
// ===========================================================================================================


//-------------------------------------------
//uniform float3 lsky_Traslate;
uniform float  lsky_kCameraHeight;
uniform float3 lsky_kCameraPos;
uniform float  lsky_kInnerRadius;
uniform float  lsky_kInnerRadius2;
uniform float  lsky_kOuterRadius;
uniform float  lsky_kOuterRadius2;
uniform float  lsky_kScale;
uniform float  lsky_kScaleOverScaleDepth;
uniform float  lsky_kKmESun;
uniform float  lsky_kKm4PI;
uniform float  lsky_kKrESun;
uniform float  lsky_kKr4PI;
uniform float3 lsky_InvWavelength;
uniform int    lsky_kSamples;
//=========================================

float Scale(float cos)
{
	float x = 1.0 - cos;
	return 0.25 * exp(-0.00287 + x * (0.459 + x * (3.83 + x * (-6.80 + x * 5.25))));
}

inline void ComputeRayleighScattering(float3 ray, out half3 inScatter)
{

	
	// Calculate the ray's start and end positions in the atmosphere, then calculate its scattering offset.
	//-------------------------------------------------------------------------------------------------------------------------
	float startHeight = lsky_kInnerRadius + lsky_kCameraHeight;
	float startAngle  = dot(ray, lsky_kCameraPos) / startHeight;
	float startDepth  = exp(lsky_kScaleOverScaleDepth * -lsky_kCameraHeight);
	float startOffset = startDepth * Scale(startAngle);
	//-------------------------------------------------------------------------------------------------------------------------

	// Same as in the unity procedural skybox.
	float far  = (sqrt(lsky_kOuterRadius2 + lsky_kInnerRadius2 * ray.y * ray.y - lsky_kInnerRadius2) - lsky_kInnerRadius * ray.y);
	float3 pos = lsky_kCameraPos + far * ray;
	//-------------------------------------------------------------------------------------------------------------------------

	// Initialize the scattering loop variables.
	//-------------------------------------------------------------------------------------------------------------------------
	float  samples      = lsky_kSamples; 
	float  sampleLength = far / samples;
	float  scaledLength = sampleLength * lsky_kScale;
	float3 sampleRay    = ray * sampleLength;
	float3 samplePoint  = lsky_kCameraPos + sampleRay * 0.5;
	//-------------------------------------------------------------------------------------------------------------------------

	// Now loop through the sample points
	//-------------------------------------------------------------------------------------------------------------------------
	float3 frontColor = 0.0; 
	//-------------------------------------------------------------------------------------------------------------------------

	for (int i = 0; i < int(samples); i++)
	{

		float height    = length(samplePoint);
		float invHeight = 1.0 / height; // reciprocal.
		//--------------------------------------------------------------------------------------

		float  depth       = exp(lsky_kScaleOverScaleDepth * (lsky_kInnerRadius - height));
		float  lightAngle  = dot(lsky_WorldSunDirection.xyz, 0.5*samplePoint) * invHeight;
		float  cameraAngle  = dot(ray, samplePoint) * invHeight;
		float3 partialAtten = (lsky_InvWavelength * lsky_kKr4PI) + lsky_kKm4PI;
		//--------------------------------------------------------------------------------------

		float  scatter   = startOffset + depth * Scale(lightAngle) - Scale(cameraAngle);
		float3 attenuate = exp(-scatter * partialAtten);
		//--------------------------------------------------------------------------------------

		// Day.
		//-------------------------------------------------------------------------------------
		float3 dayColor  = attenuate * (depth * scaledLength) * (lsky_SunAtmosphereTint);
		//-------------------------------------------------------------------------------------

		#ifndef LSKY_ENABLE_NIGHT_RAYLEIGH

		frontColor += dayColor;
	
		#else


		// Night
		//------------------------------------------------------------------------------------------------------------------
		float nightLightAngle = lerp(-lightAngle, dot(lsky_WorldMoonDirection.xyz, samplePoint) * invHeight, lsky_NightRayleighMode);
		//------------------------------------------------------------------------------------------------------------------

		float  nightScatter   = startOffset + depth * (Scale(nightLightAngle) - Scale(cameraAngle));

		float3 nightAttenuate = exp(-nightScatter* partialAtten);
		//------------------------------------------------------------------------------------------------------------------

		float3 nightColor = nightAttenuate * (depth * scaledLength) * lsky_MoonAtmosphereTint.rgb;
		//------------------------------------------------------------------------------------------------------------------
		
		frontColor += dayColor + nightColor;
		//------------------------------------------------------------------------------------------------------------------

		#endif

		samplePoint  += sampleRay;

	}

	float cosTheta = dot(ray, lsky_WorldSunDirection.xyz);
	inScatter      = ((frontColor * (lsky_InvWavelength * lsky_kKrESun))) * RayleighPhase(cosTheta);

}


inline void ComputeAtmosphericScattering(float3 ray, out half3 inScatter, out half4 outScatter)
{

	//float3 cameraPos = _WorldSpaceCameraPos - V3Translate; 	
	//float cameraHeight = length(v3CameraPos);					
	//float cameraHeight2 = cameraHeight*cameraHeight;			
	//float3 v3Pos = mul(unity_ObjectToWorld, v.vertex).xyz - v3Translate;		
	//float3 v3Ray = v3Pos - v3CameraPos;
	//float far = length(ray);
	//ray /= far;
	//=========================================================================


	// Calculate the ray's start and end positions in the atmosphere, then calculate its scattering offset.
	//-------------------------------------------------------------------------------------------------------------------------
	float startHeight = lsky_kInnerRadius + lsky_kCameraHeight;
	float startAngle  = dot(ray, lsky_kCameraPos) / startHeight;
	float startDepth  = exp(lsky_kScaleOverScaleDepth * -lsky_kCameraHeight);
	float startOffset = startDepth * Scale(startAngle);
	//-------------------------------------------------------------------------------------------------------------------------

	// Same as in the unity procedural skybox.
	float far  = (sqrt(lsky_kOuterRadius2 + lsky_kInnerRadius2 * ray.y * ray.y - lsky_kInnerRadius2) - lsky_kInnerRadius * ray.y);
	float3 pos = lsky_kCameraPos + far * ray;
	//-------------------------------------------------------------------------------------------------------------------------

	// Initialize the scattering loop variables.
	//-------------------------------------------------------------------------------------------------------------------------
	float  samples      = lsky_kSamples; 
	float  sampleLength = far / samples;
	float  scaledLength = sampleLength * lsky_kScale;
	float3 sampleRay    = ray * sampleLength;
	float3 samplePoint  = lsky_kCameraPos + sampleRay * 0.5;
	//-------------------------------------------------------------------------------------------------------------------------

	// Now loop through the sample points
	//-------------------------------------------------------------------------------------------------------------------------
	float3 frontColor = 0.0; float4 outColor = 0.0; 
	//-------------------------------------------------------------------------------------------------------------------------

	for (int i = 0; i < int(samples); i++)
	{

		float height    = length(samplePoint);
		float invHeight = 1.0 / height; // reciprocal.
		//--------------------------------------------------------------------------------------

		float  depth       = exp(lsky_kScaleOverScaleDepth * (lsky_kInnerRadius - height));
		float  lightAngle  = dot(lsky_WorldSunDirection.xyz, 0.5*samplePoint) * invHeight;
		float  cameraAngle  = dot(ray, samplePoint) * invHeight;
		float3 partialAtten = (lsky_InvWavelength * lsky_kKr4PI) + lsky_kKm4PI;
		//--------------------------------------------------------------------------------------

		float  scatter   = startOffset + depth * Scale(lightAngle) - Scale(cameraAngle);
		float3 attenuate = exp(-scatter * partialAtten);
		//--------------------------------------------------------------------------------------

		// Day.
		//-------------------------------------------------------------------------------------
		float3 dayColor  = attenuate * (depth * scaledLength) * (lsky_SunAtmosphereTint);
		//-------------------------------------------------------------------------------------

		#ifndef LSKY_ENABLE_NIGHT_RAYLEIGH

		frontColor += dayColor;
		outColor.a = 1;


		#else


		// Night
		//------------------------------------------------------------------------------------------------------------------
		float nightLightAngle = lerp(-lightAngle, dot(lsky_WorldMoonDirection.xyz, samplePoint) * invHeight, lsky_NightRayleighMode);
		//------------------------------------------------------------------------------------------------------------------

		float  nightScatter   = startOffset + depth * (Scale(nightLightAngle) - Scale(cameraAngle));

		float3 nightAttenuate = exp(-nightScatter* partialAtten);
		//------------------------------------------------------------------------------------------------------------------

		float3 nightColor = nightAttenuate * (depth * scaledLength) * lsky_MoonAtmosphereTint.rgb;
		outColor.a += nightAttenuate * (depth * scaledLength);
		//------------------------------------------------------------------------------------------------------------------
		
		frontColor += dayColor + nightColor;
		//------------------------------------------------------------------------------------------------------------------

		#endif

		outColor.rgb += dayColor;
		samplePoint  += sampleRay;

	}

	float cosTheta = dot(ray, lsky_WorldSunDirection.xyz);
	inScatter      = ((frontColor * (lsky_InvWavelength * lsky_kKrESun))) * RayleighPhase(cosTheta);
	outScatter     = outColor * lsky_kKmESun;
	outScatter.a  *= 30;

	//outscatter.a = RGB_TO_GRAYSCALE(outColor.aaa);

}

#endif


#endif // Atmospheric Scattering Include
