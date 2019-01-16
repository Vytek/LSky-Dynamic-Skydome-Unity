////////////////////////////////////////
/// LSky
/// ====================
///
/// Description:
/// ====================
/// Common for deep space shaders.
////////////////////////////////////////

#ifndef LSKY_DEEP_SPACE_COMMON_INCLUDED
#define LSKY_DEEP_SPACE_COMMON_INCLUDED

// Includes
//---------------------------
#include "LSky_Common.cginc"
//===========================


#ifndef LSKY_ENABLE_STARS_SCINTILLATION
#define LSKY_ENABLE_STARS_SCINTILLATION 0
#endif
//==========================================


//------------------------------------------------ Common ----------------------------------------------------
//============================================================================================================

// Cubemap
//-----------------------------------------------
uniform samplerCUBE _Cubemap;
half4 _Cubemap_HDR;

//===============================================

// Color.
//-----------------------------------------------
uniform half3 _Tint;
uniform half _Intensity;
uniform half _ExponentFade;
//===============================================

// Scintillation
//-----------------------------------------------
uniform samplerCUBE _NoiseCubemap;
uniform float4x4 _NoiseMatrix;
uniform half _Scintillation;
//===============================================


// Structs
//------------------------------------------
struct lsky_deepspace_appdata
{
    float4 vertex : POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct lsky_deepspace_v2f
{
    float4 vertex : SV_POSITION;

    float3 texcoord : TEXCOORD0;

    half3  col : TEXCOORD2;

    #ifdef LSKY_ENABLE_STARS_SCINTILLATION
    float3 texcoord2 : TEXCOORD3;
    #endif

    UNITY_VERTEX_OUTPUT_STEREO
};
//==========================================


// Vertex
//--------------------------------------------------------------------------
lsky_deepspace_v2f vert (lsky_deepspace_appdata v)
{

  // Init
	//------------------------------------------------------------------------
  lsky_deepspace_v2f o; 
  UNITY_INITIALIZE_OUTPUT(lsky_deepspace_v2f, o);
	//========================================================================

	// Warning: No VR Tested
	//------------------------------------------------------------------------
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	//========================================================================

	// Position
	//------------------------------------------------------------------------
  o.vertex = UnityObjectToClipPos(v.vertex);

  #ifdef UNITY_REVERSED_Z
	  o.vertex.z = 1e-5f;
  #else
	  o.vertex.z = o.vertex.w - 1e-5f;
  #endif
  
	o.texcoord = v.vertex.xyz;

  #ifdef LSKY_ENABLE_STARS_SCINTILLATION
    o.texcoord2 = mul((float3x3)_NoiseMatrix, v.vertex.xyz);
  #endif
	//=======================================================================

  // Tint
	//-----------------------------------------------------------------------
	o.col.rgb = _Tint.rgb * _Intensity *
	2*(normalize(mul((float3x3)unity_ObjectToWorld, (v.vertex.xyz))).y);
	//=======================================================================

  return o;
}
//=========================================================================


//------------------------------------------ Galaxy Background -----------------------------------------------
//============================================================================================================

// Galaxy Background Fragment
//------------------------------------------------------------------------------------------------------------
half4 lsky_galaxy_frag (lsky_deepspace_v2f i) : SV_Target
{

	// Return final color with horizon fade.
	//---------------------------------------------------------------------------------------
  return saturate(half4(Exponent3(texCUBE(_Cubemap, i.texcoord) * i.col.rgb, _ExponentFade), 1.0));
	//======================================================================================
}
//===========================================================================================================

half4 lsky_galaxy_frag_hdr (lsky_deepspace_v2f i) : SV_Target
{

  //------------------------------------------------------------
  half4 cube   = texCUBE(_Cubemap, i.texcoord);
  half3 color  = DecodeHDR(cube, _Cubemap_HDR);
  color       *= unity_ColorSpaceDouble.rgb;
  //============================================================

	// Return final color with horizon fade.
	//------------------------------------------------------------
  return saturate(half4(Exponent3(color.rgb, _ExponentFade) * i.col.rgb, 1.0));
	//============================================================
}
//===========================================================================================================



//--------------------------------------------- Stars Field --------------------------------------------------
//============================================================================================================



half4 lsky_starsfield_frag (lsky_deepspace_v2f i) : SV_Target
{

  // Get cubemap red channel
	//------------------------------------------------------------
  half3 tex = texCUBE(_Cubemap, i.texcoord).rgb;
  //============================================================


  #ifdef LSKY_ENABLE_STARS_SCINTILLATION
  // Get noise red channel.
	//------------------------------------------------------------
	half noise = texCUBE(_NoiseCubemap, i.texcoord2).r;
	//============================================================

  // Apply scintillation
	//------------------------------------------------------------
	tex = lerp(tex, tex*noise, _Scintillation);
	//============================================================
  #endif

	// Return final color with horizon fade.
	//------------------------------------------------------------
  return saturate(half4(tex * i.col.rgb, 1.0));
	//============================================================
}
//===========================================================================================================


#endif
