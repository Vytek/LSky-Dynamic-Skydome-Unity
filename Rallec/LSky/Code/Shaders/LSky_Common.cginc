////////////////////////////////////////
/// LSky 
/// ====================
///
/// Description:
/// ====================
/// Common for LSky shaders.
////////////////////////////////////////

#ifndef LSKY_COMMON_INCLUDED
#define LSKY_COMMON_INCLUDED

// Includes
//-----------------------
#include "UnityCG.cginc"
//=======================


//----------------------------------------------- Matrices ---------------------------------------------------
//============================================================================================================

// 4x4 Matrices.
//--------------------------------------
uniform float4x4 lsky_WorldToObject;
uniform float4x4 lsky_ObjectToWorld;
//======================================

// Celestials Directions.
//-----------------------------------------
uniform float3 lsky_LocalSunDirection;
uniform float3 lsky_LocalMoonDirection;
uniform float3 lsky_WorldSunDirection;
uniform float3 lsky_WorldMoonDirection;
//=========================================


//------------------------------------------ Color Correction ------------------------------------------------
//============================================================================================================

// Color Space
//---------------------------------------------------------
#define GAMMA_TO_LINEAR(color) pow(color, 0.454545)


// HDR
//---------------------------------------------------------
uniform half lsky_Exposure; // General Exposure.
//============================

#define FAST_TONEMAPING(col) 1.0 - exp(exposure * -col.rgb)

inline float3 fastTonemaping(float3 col, half exposure)
{
  return 1.0 - exp(exposure * -col.rgb);
}
//=========================================================

// Exponent.
//---------------------------------------------------------
inline half Exponent2(half x, half fade)
{

  return lerp(x, x*x, fade);

}

inline half Exponent3(half x, half fade)
{

  return lerp(x, x*x*x, fade);

}

inline half3 Exponent2(half3 x, half fade)
{

  return lerp(x, x*x, fade);

}

inline half3 Exponent3(half3 x, half fade)
{

  return lerp(x, x*x*x, fade);

}
//========================================================


#endif
