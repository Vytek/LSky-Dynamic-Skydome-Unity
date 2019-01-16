////////////////////////////////////////
/// LSky 
/// ====================
///
/// Description:
/// ====================
/// Galaxy Background
////////////////////////////////////////

Shader "Rallec/LSky/Near Space/Near Space"
{
	Properties{}
	CGINCLUDE

		// Include
		//------------------------------
		#include "UnityCG.cginc"
		#include "LSky_Common.cginc"
		//==============================


		// Sun Disc Params
		//------------------------------------------
		uniform half  _SunSize;
		uniform half3 _SunTint;
		uniform half  _SunIntensity;
		//==========================================

		// Moon 
		//------------------------------------------
		uniform sampler2D _MoonRenderTex;
		uniform float4x4  _MoonMatrix;
		uniform float     _MoonSize;
		//==========================================


		// Structs
		//------------------------------------------
		struct appdata_t
		{
    		float4 vertex : POSITION;
    		UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
    		float4 vertex     : SV_POSITION;
    		float3 texcoord   : TEXCOORD0;
			float3 moonCoords : TEXCOORD1;
    		half2  fade       : TEXCOORD2;

   	 		UNITY_VERTEX_OUTPUT_STEREO
		};
		//==========================================

		// Vertex
		//---------------------------------------------------------------------------------------
		v2f vert (appdata_t v)
		{

  			// Init
			//-----------------------------------------------------------------------------------
 			v2f o; 
  			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//===================================================================================

			// Warning: No VR Tested
			//-----------------------------------------------------------------------------------
  			UNITY_SETUP_INSTANCE_ID(v);
  			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//===================================================================================

			// Position
			//-----------------------------------------------------------------------------------
  			o.vertex = UnityObjectToClipPos(v.vertex);

  			#ifdef UNITY_REVERSED_Z
	  		o.vertex.z = 1e-5f;
  			#else
	  		o.vertex.z = o.vertex.w - 1e-5f;
  			#endif
			//===================================================================================
  
			o.texcoord.xyz = v.vertex.xyz;
			//===================================================================================

			// Moon Position.
			#ifdef LSKY_ENABLE_MOON
			o.moonCoords.xyz = (mul((float3x3)_MoonMatrix, v.vertex.xyz) / _MoonSize + 0.5);
			#endif

			//===================================================================================

  			// Fade
			//-----------------------------------------------------------------------------------
			o.fade.x = 10*(normalize(mul((float3x3)unity_ObjectToWorld, (v.vertex.xyz))).y);

			#ifdef LSKY_ENABLE_MOON
			o.fade.y = dot(v.vertex.xyz, lsky_WorldMoonDirection.xyz); 
			#endif
			//===================================================================================

  			return o;
		}

		// Sun Disc
		//---------------------------------------------------------------------------------------
		half sunDisc(half size, float3 coords)
		{
			half dist = length(coords);
			half spot = (1.0 - step(size, dist));
			return spot;
		}
		//=======================================================================================
		
		half4 frag (v2f i) : SV_Target
		{

			half4 col = half4(0,0,0,1);

			// Get Moon Texture
			//------------------------------------------------------------------------------------
			#ifdef LSKY_ENABLE_MOON
			half4 moonCol = tex2D(_MoonRenderTex, float2(-i.moonCoords.x+1, i.moonCoords.y))*saturate(i.fade.y);
			#endif
			//====================================================================================

			// Compute sun
			//------------------------------------------------------------------------------------
			#ifdef LSKY_ENABLE_SUN

			half spot = sunDisc(_SunSize, (lsky_WorldSunDirection.xyz-i.texcoord.xyz));
			col.a = spot;
			#ifdef LSKY_ENABLE_MOON
			spot *= saturate(1.0-moonCol.a);
			col.a += moonCol.a;
			#endif

			col.rgb += spot * _SunTint.rgb * _SunIntensity;
			
			
			#endif

			#ifdef LSKY_ENABLE_MOON
			col.rgb += moonCol.rgb;
			//col.a = moonCol.a;
			#endif
			//=====================================================================================

			return saturate(col*i.fade.x);
		}
		//=========================================================================================


	ENDCG

	SubShader
	{

		Tags{ "Queue"="Background+15" "RenderType"="Transparent" "IgnoreProjector"="True" }
		//====================================================================================

		Pass
		{

			Cull Front
			ZWrite Off
			ZTest LEqual
			Blend One OneMinusSrcAlpha
			//Blend One SrcAlpha 
			//Blend SrcAlpha OneMinusSrcAlpha
			Fog{ Mode Off }
			//Offset 0
			//==========================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile __ LSKY_ENABLE_SUN
				#pragma multi_compile __ LSKY_ENABLE_MOON
				
			ENDCG
			//=========================
		}
	}
}
