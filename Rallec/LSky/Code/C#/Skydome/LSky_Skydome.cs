////////////////////////////////
/// LSky
/// ========
///
/// Description
/// ============
/// Skydome.
////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rallec.Utility;

namespace Rallec.LSky
{
	[ExecuteInEditMode] public class LSky_Skydome : MonoBehaviour 
	{


		//------------------------------------Params & Properties-----------------------------------
		//==========================================================================================


		#region |Resources|

		[SerializeField] private LSky_SkydomeResources m_Resources = null;

		public bool CheckResources
		{
			get
			{

				// Sphere LOD
				//----------------------------------------------------
				if(m_Resources.sphereLOD0 == null) return false;
				if(m_Resources.sphereLOD1 == null) return false;
				if(m_Resources.sphereLOD2 == null) return false;
				if(m_Resources.sphereLOD3 == null) return false;
				//====================================================

				// Atmosphere LOD
				//----------------------------------------------------
				if(m_Resources.atmosphereLOD0 == null) return false;
				if(m_Resources.atmosphereLOD1 == null) return false;
				if(m_Resources.atmosphereLOD2 == null) return false;
				//====================================================

				// Hemisphere LOD
				//----------------------------------------------------
				if(m_Resources.hemisphereLOD0 == null) return false;
				if(m_Resources.hemisphereLOD1 == null) return false;
				if(m_Resources.hemisphereLOD2 == null) return false;
				//====================================================

				// Deep Space-
				//------------------------------------------------------------
				if(m_Resources.galaxyBackgroundShader == null)   return false;
				if(m_Resources.starsFieldShader == null)         return false;
				if(m_Resources.galaxyBackgroundMaterial == null) return false;
				if(m_Resources.starsFieldMaterial == null)       return false;
				//============================================================

				// Near Space
				//----------------------------------------------------
				if(m_Resources.moonShader == null)        return false;
				if(m_Resources.nearSpaceShader == null)   return false;
				if(m_Resources.moonMaterial == null)      return false;
				if(m_Resources.nearSpaceMaterial == null) return false;

				//====================================================

				// Atmosphere
				//----------------------------------------------------
				if(m_Resources.atmosphereShader == null)   return false;
				if(m_Resources.skyboxShader == null)       return false;
				if(m_Resources.atmosphereMaterial == null) return false;
				if(m_Resources.skyboxMaterial == null)     return false;

				//====================================================

				// Clouds
				//----------------------------------------------------
				if(m_Resources.cloudsShader   == null) return false;
				if(m_Resources.cloudsMaterial == null) return false;
				//====================================================

				return true;
			}
		}

		#endregion

		#region |Components|

		private Transform m_Transform = null;

		// Deep Space
		//-----------------------------------------------------------------------------

		private LSky_EmptyObject m_GalaxyBackgroundTransform = new LSky_EmptyObject();
		private LSky_EmptyObject m_StarsFieldTransform = new LSky_EmptyObject();
		//=============================================================================

		// Near Space
		//-----------------------------------------------------------------------------
		private LSky_EmptyObject m_SunTransform = new LSky_EmptyObject();
		private LSky_EmptyObject m_MoonTransform = new LSky_EmptyObject();
		private LSky_RenderCamera m_MoonCamera = new LSky_RenderCamera();
		private LSky_EmptyObject  m_NearSpaceTransform = new LSky_EmptyObject();
		//=============================================================================


		// Atmosphere
		//-----------------------------------------------------------------------------
		private LSky_EmptyObject m_AtmosphereTransform = new LSky_EmptyObject();
		//=============================================================================


		// Clouds
		//-----------------------------------------------------------------------------
		private LSky_EmptyObject m_CloudsTransform = new LSky_EmptyObject();
		//=============================================================================

		// Light
		//-----------------------------------------------------------------------------
		private LSky_Light m_CelestialsLight = new LSky_Light();
		//=============================================================================

		public bool CheckComponents
		{
			get
			{	
				// Deep Space
				//-------------------------------------------------------------
				if(!m_GalaxyBackgroundTransform.CheckComponents) return false;
				if(!m_StarsFieldTransform.CheckComponents) return false;
				//=============================================================

				// Near Space
				//-------------------------------------------------------------
				if(!m_SunTransform.CheckComponents) return false;
				if(!m_MoonTransform.CheckComponents) return false;
				if(!m_MoonCamera.CheckComponents) return false;
				if(!m_NearSpaceTransform.CheckComponents) return false;
				if(!m_MoonCamera.CheckComponents) return false;
				//=============================================================

				// Atmosphere
				//-------------------------------------------------------------
				if(!m_AtmosphereTransform.CheckComponents) return false;
				//=============================================================

				// Clouds
				//-------------------------------------------------------------
				if(!m_CloudsTransform.CheckComponents) return false;
				//=============================================================

				// Light
				//-------------------------------------------------------------
				if(!m_CelestialsLight.CheckComponents) return false;
				//=============================================================

				return true;
			}
		}

		#endregion

		#region |General|

		// Dome radius.
		[SerializeField] private float m_DomeRadius = 2000f;

		// We apply a fast tonemaping if we are not using Color Grading IFX.
		[SerializeField] private bool m_ApplyFastTonemaping = true;

		// General exposure.
		[SerializeField] private float m_Exposure = 1.3f; 

		// Dome radius in xyz
		public Vector3 DomeRadius3D
		{
			get
			{
				return Vector3.one * m_DomeRadius;
			}
		}
	
		#endregion
	
		#region |DeepSpace|

		// Galaxy Background
		//--------------------------------------------------------------
		[SerializeField] private bool m_RenderGalaxy = false;
		[SerializeField] private int m_GalaxyLayerIndex = 1;
		//--------------------------------------------------------------

		[SerializeField] private Cubemap m_GalaxyCubemap = null;
		[SerializeField] private Color m_GalaxyColor = Color.white;

		[R_CurveRange(0,0,1,3)]
		[SerializeField] private AnimationCurve m_GalaxyIntensity = AnimationCurve.Linear(0,1,1,1);

		[SerializeField, Range(0,1)] 
		private float m_GalaxyExponentFade = 0.3f;
		//==============================================================

		// Stars Field
		//---------------------------------------------------------------
		[SerializeField] private bool m_RenderStarsField = false;
		[SerializeField] private int m_StarsFieldLayerIndex = 1;
		//----------------------------------------------------------------

		[SerializeField] private Cubemap m_StarsFieldCubemap = null;
		[SerializeField] private Cubemap m_StarsFieldNoiseCubemap = null;
		//----------------------------------------------------------------

		[SerializeField] private Color m_StarsFieldColor = Color.white;

		[R_CurveRange(0,0,1,5)]
		[SerializeField] private AnimationCurve m_StarsFieldIntensity = AnimationCurve.Linear(0,1,1,1);
		//----------------------------------------------------------------

		[SerializeField, Range(0.0f, 1.0f)] 
		private float m_StarsFieldScintillation = 1.0f;
		[SerializeField] private float m_StarsFieldScintillationSpeed = 0.7f;
		//-----------------------------------------------------------------

		private float m_StarsFieldNoiseXAngle;
		//=================================================================

		#endregion

		#region |NearSpace|

		// Render Near Space
		//------------------------------------------------------------------------------
		[SerializeField] private bool m_RenderNearSpace = true;
		[SerializeField] private int m_NearSpaceLayerIndex = 1;
		//==============================================================================

		// Sun Position
		//------------------------------------------------------------------------------
		[SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_SunPI    = 0.0f;
        [SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_SunTheta = 0.70f;
		//=============================================================================

		// Render Sun
		//-----------------------------------------------------------------------------
		[SerializeField] private bool m_RenderSun = true;
		//=============================================================================

		// Sun Params
		//-----------------------------------------------------------------------------
		[R_CurveRange(0,0,1,1)]
		[SerializeField] private AnimationCurve m_SunSize = AnimationCurve.Linear(0,0.1f,1,0.1f);
		[SerializeField] Gradient m_SunColor = new Gradient();
        [SerializeField] private float m_SunIntensity = 1.0f;
		//=============================================================================

		// Sun position
		//-----------------------------------------------------------------------------
		public Vector3 SunPosition
		{
			get{ return LSky_Mathf.SphericalToCartesian(m_SunTheta, m_SunPI, DomeRadius); }
		}

		public Vector3 SunDirection
		{
			//get{ return -(SunMatrix.rotation * Vector3.forward); }
			get{ return -m_SunTransform.transform.forward; }
		}
		//=============================================================================

		// Moon Position
		//-----------------------------------------------------------------------------
		[SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_MoonPI    = 0.0f;
        [SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_MoonTheta = 0.70f;
		//=============================================================================

		// Render Moon
		//-----------------------------------------------------------------------------
		[SerializeField] private bool m_RenderMoon = true;
		//[SerializeField] private int m_MoonLayerIndex = 1;
		[SerializeField] private RenderTexture m_MoonRenderTexture = null;
		[SerializeField] private int m_MoonRenderLayerIndex = 9;
		//=============================================================================

		// Moon Params
		//-----------------------------------------------------------------------------
		[SerializeField] private Texture2D m_MoonTexture = null;
		[SerializeField] private Vector2 m_MoonTextureOffset = Vector3.zero;

		[R_CurveRange(0,0,1,1)]
		[SerializeField] private AnimationCurve m_MoonSize = AnimationCurve.Linear(0,0.1f,1,0.1f);
		[SerializeField] Gradient m_MoonColor = new Gradient();
        [SerializeField] private float m_MoonIntensity = 1.0f;
		[SerializeField, Range(0,1)] private float m_MoonExponentFade = 0.5f;
		//=============================================================================

		// Moon Position
		//-----------------------------------------------------------------------------
		public Vector3 MoonPosition
		{
			get{ return LSky_Mathf.SphericalToCartesian(m_MoonTheta, m_MoonPI, 5); }
		}

		public Vector3 MoonDirection
		{
			//get{ return -(MoonMatrix.rotation * Vector3.forward); }
			get{ return -m_MoonTransform.transform.forward; }
		}
		//=============================================================================

		// Evaluate Time
		//-----------------------------------------------------------------------------

		/// <summary>
        /// Evaluate time for curves and gradients in full sun cycle.
        /// </summary>
        public float EvaluateTimeBySun{ get { return (1.0f - SunDirection.y) * 0.5f; } }

        /// <summary>
        /// Evaluate time for curves and gradients in above horizon sun cycle.
        /// </summary>
        public float EvaluateTimeBySunAboveHorizon { get { return (1.0f - SunDirection.y); } }


        /// <summary>
        /// Evaluate time for curves and gradient in bellow horizon sun cycle.
        /// </summary>
        public float EvaluateTimeBySunBelowHorizon { get { return (1.0f - (-SunDirection.y)); } }

 		/// <summary>
        /// Evaluate time for curves and gradients in full moon cycle.
        /// </summary>
        public float EvaluateTimeByMoon{ get { return (1.0f - MoonDirection.y) * 0.5f; } }

        /// <summary>
        /// Evaluate time for curves and gradients in above horizon moon cycle.
        /// </summary>
        public float EvaluateTimeByMoonAboveHorizon { get { return (1.0f - MoonDirection.y); } }

        /// <summary>
        /// Evaluate time for curves and gradient in bellow horizon moon cycle.
        /// </summary>
        public float EvaluateTimeByMoonBelowHorizon { get { return (1.0f - (-MoonDirection.y)); } }
		//==============================================================================

		#endregion

		#region |Atmosphere|

		// Render
		//-----------------------------------------------------------------------------------------------------------
		[SerializeField] private bool m_RenderAtmosphere = true;
        [SerializeField] private int m_AtmosphereLayerIndex = 1;
				// Model.
		[SerializeField] private LSky_AtmosphereModel m_AtmosphereModel = LSky_AtmosphereModel.Preetham;
		//===========================================================================================================

		// Quality
		//-----------------------------------------------------------------------------------------------------------
		[SerializeField] private LSky_Quality3 m_AtmosphereMeshQuality = LSky_Quality3.High;
		[SerializeField] private LSky_ShaderQuality m_AtmosphereShaderQuality = LSky_ShaderQuality.PerVertex;

		// Only in Preetham Model.
        [SerializeField] private bool m_RealTimeBetaMie = false, m_RealTimeBetaRay = false;

		[SerializeField] private int m_AtmosphereSamples = 4;
		//===========================================================================================================

		// Rayleigh.
		//-----------------------------------------------------------------------------------------------------------
        [SerializeField, Range(0.0f, 1000f)] private float m_WavelengthR = 650f; // = 650f; // = 680f;
        [SerializeField, Range(0.0f, 1000f)] private float m_WavelengthG = 570f; // = 570f; // = 550f;
        [SerializeField, Range(0.0f, 1000f)] private float m_WavelengthB = 475f; // = 475f; // = 440f;
		//==========================================================================================================

		[R_CurveRange(0,0,1,15)]
        [SerializeField] private AnimationCurve m_RayleighScattering = AnimationCurve.Linear(0,1f,1,1f);
        [SerializeField, Range(0, 1)] private float m_AtmosphereHaziness = 1.0f; // Only In Preetham Model.
        [SerializeField] private float m_AtmosphereZenith = 0.03f;
		//=========================================================================================================

		// Sun/Day.
		//---------------------------------------------------------------------------------------------------------
		[R_CurveRange(0,0,1,50)]
        [SerializeField] private AnimationCurve m_SunBrightness = AnimationCurve.Linear(0,1f,1,1f);
        [SerializeField] private Gradient m_DayAtmosphereTint = new Gradient();
		//=========================================================================================================

        // Moon/Night.
		//---------------------------------------------------------------------------------------------------------
        [SerializeField] private LSky_NightRayleighMode m_NightRayleighMode = LSky_NightRayleighMode.OppositeSun;

        [SerializeField, Range(0.0f, 1f)] private float m_MoonBrightness = 3f;
        [SerializeField] private Color m_NightAtmosphereTint = Color.white;
		//=========================================================================================================


		// Common
		//---------------------------------------------------------------------------------------------------------
		[SerializeField, Range(0.0f, 0.5f)] private float m_Mie = 0.001f;
		//=========================================================================================================

		// Sun
		//---------------------------------------------------------------------------------------------------------
        [SerializeField] private Gradient m_SunMieColor = new Gradient();
        [SerializeField, Range(0.0f, 0.999f)] private float m_SunMieAnisotropy = 0.85f;

		[R_CurveRange(0,0,1,25)]
        [SerializeField] private AnimationCurve m_SunMieScattering = AnimationCurve.Linear(0,1f,1,1f);
		//=========================================================================================================

        // Moon
		//---------------------------------------------------------------------------------------------------------
        [SerializeField] private Gradient m_MoonMieColor = new Gradient();
        [SerializeField, Range(0.0f, 0.999f)] private float m_MoonMieAnisotropy = 0.75f;

		[R_CurveRange(0,0,1,5)]
        [SerializeField] private AnimationCurve m_MoonMieScattering = AnimationCurve.Linear(0,1f,1,1f);
		//==========================================================================================================

		// Private Fields
		//----------------------------------------------------------------------------------------------------------
		private Vector3 m_BetaRayleigh, m_BetaMie;
		//==========================================================================================================


        // Color Correction
		//----------------------------------------------------------------------------------------------------------
        [SerializeField, Range(0,1)] private float m_AtmosphereExponentFade = 0.5f;
		//==========================================================================================================

		// Properties
		//----------------------------------------------------------------------------------------------------------
        public float DayIntensity
        {
            get
            {
                return LSky_Mathf.Saturate(SunDirection.y + 0.40f);
            }
        }

		public float NightIntensity
        {

            get
            {

				if(m_NightRayleighMode == LSky_NightRayleighMode.OppositeSun)
					return LSky_Mathf.Saturate(-SunDirection.y + 0.25f);

				if(m_NightRayleighMode == LSky_NightRayleighMode.Moon)
					return LSky_Mathf.Saturate(MoonDirection.y + 0.40f);

               return 0.0f;
            }
        }

		public float MoonPhasesIntensityMultiplier

		{
			get
			{
				return Mathf.Clamp01(Vector3.Dot(-SunDirection, MoonDirection)+ 0.45f);
			}
		}
		//-----------------------------------------------------------------------------------------------------------

		Vector3 Lambda
        {
            get
            {

               	Vector3 result;
				{

                	result.x = m_WavelengthR * 1e-9f;
                	result.y = m_WavelengthG * 1e-9f;
                	result.z = m_WavelengthB * 1e-9f;

				}
                return result;

            }
        }
		//========================================================================================================

		#endregion

		#region |Clouds|

		// Render.
		//------------------------------------------------------------------------------
		[SerializeField] private bool m_RenderClouds = true;
		[SerializeField] private int m_CloudsLayerIndex = 1;
		//==============================================================================

		// Size.
		//------------------------------------------------------------------------------
 		[SerializeField, Range(0f, 1f)] private float m_CloudsMeshHeightSize = 1f;
		//==============================================================================

		// Rotate.
		//------------------------------------------------------------------------------

		[SerializeField] private float m_CloudsRotationSpeed = 0.5f;

		// Textures.
		//------------------------------------------------------------------------------
		[SerializeField] private Texture2D m_CloudsTexture = null;
		[SerializeField] private Vector2 m_CloudsSize = Vector2.one;
		[SerializeField] private Vector2 m_CloudsTexOffset = Vector2.zero;
		//==============================================================================

		// Color.
		//------------------------------------------------------------------------------
        [SerializeField] Gradient m_CloudsColor = new Gradient();
		[SerializeField] Gradient m_CloudsMoonColor = new Gradient();
        [SerializeField, Range(0.0f, 20f)] private float m_CloudsIntensity = 1.0f;
		//==============================================================================


		// Density.
		//------------------------------------------------------------------------------
		[SerializeField, Range(0, 20f)] private float m_CloudsDensity = 0.3f;
		[SerializeField, Range(0, 1.0f)] private float m_CloudsCoverage = 0.5f;
		//==============================================================================

		private float m_CloudsYAngle;

		#endregion

		#region |Lighting|

		// Light.
		//-------------------------------------------------------------------------------------------------

		[R_CurveRange(0,0,1,10)]
		[SerializeField] private AnimationCurve m_SunLightIntensity = AnimationCurve.Linear(0,1,1,1);
		[SerializeField] private Gradient m_SunLightColor = new Gradient();
		//-----------------------------------------------------------------------------------------------

		[R_CurveRange(0,0,1,10)]
		[SerializeField] private AnimationCurve m_MoonLightIntensity = AnimationCurve.Linear(0,1,1,1);
		[SerializeField] private Gradient m_MoonLightColor = new Gradient();
		//-----------------------------------------------------------------------------------------------

		[R_CurveRange(0,0,1,1)]
		[SerializeField] private AnimationCurve m_SunMoonLightFade = AnimationCurve.Linear(0,1,1,1);
		//===============================================================================================

		// Ambient
		//-----------------------------------------------------------------------------------------------
		[SerializeField] private bool m_SendSkybox = false;
		//-----------------------------------------------------------------------------------------------
        [SerializeField] private float m_AmbientUpdateInterval = 15;
		private float m_AmbientRefreshTimer = 0.0f;
		//-----------------------------------------------------------------------------------------------
       
		[SerializeField]
        private Gradient m_AmbientGroundColor = new Gradient()
        {
            colorKeys = new GradientColorKey[]
           {
              new GradientColorKey(new Color(0.466f, 0.435f, 0.415f, 1.0f), 0.0f),
              new GradientColorKey(new Color(0.355f, 0.305f, 0.269f, 1.0f), 0.45f),
              new GradientColorKey(new Color(0.227f, 0.156f, 0.101f, 1.0f), 0.50f),
              new GradientColorKey(new Color(0.0f, 0.0f, 0.0f, 1.0f), 0.55f),
              new GradientColorKey(new Color(0.0f, 0.0f, 0.0f, 1.0f), 1.0f)
           },

           alphaKeys = new GradientAlphaKey[]
           {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
           }
        };
		//---------------------------------------------------------------------------------------

		// Fog
		//---------------------------------------------------------------------------------------

      	[SerializeField] private bool m_EnableUnityFog = false;
        [SerializeField] private FogMode m_UnityFogMode = FogMode.ExponentialSquared;
		[SerializeField] private Gradient m_UnityFogColor = new Gradient();
		[SerializeField] private Gradient m_UnityMoonFogColor = new Gradient();
		[SerializeField] private float m_UnityFogDensity = 0.001f;
        [SerializeField] private float m_UnityFogStartDistance = 0.0f;
        [SerializeField] private float m_UnityFogEndDistance = 300f;
		//---------------------------------------------------------------------------------------

		public bool IsDay
		{
			get
			{

				if(Mathf.Abs(m_SunTheta) > 1.7f)
					return false;

				return true;
			}
		}
		//--------------------------------------------------------------------------------------

		public bool DirLightEnabled
		{
			get
			{
				if(!IsDay && Mathf.Abs(m_MoonTheta) > 1.7f)
					return false;

				return true;
			}
		}
		//=====================================================================================


		#endregion

		#region |ShaderPropertyID|

		// Dome Matrices
		//-----------------------------------------------------------
		public int lsky_ObjectToWorldID{ get; private set; }
		public int lsky_WorldToObjectID{ get; private set; }
		//-----------------------------------------------------------

		// General settings
		//-----------------------------------------------------------
		public int lsky_ExposureID{ get; private set; }
		//-----------------------------------------------------------

		// Common celestials properties
		//-----------------------------------------------------------
		public int lsky_ExponentFadeID{ get; private set; }
		public int lsky_CubemapID{ get; private set; }
		public int lsky_TintID{ get; private set; }
		public int lsky_IntensityID{ get; private set; }
		public int lsky_MainTexID{ get; private set; }

		// Satars field
		//----------------------------------------------------------
		public int lsky_NoiseCubemapID{ get; private set; }
		public int lsky_ScintillationID{ get; private set; }
		public int lsky_NoiseMatrixID{ get; private set; }

		// Sun and moon.
		//---------------------------------------------------------
		public int lsky_WorldSunDirectionID{ get; private set; }
		public int lsky_LocalSunDirectionID{ get; private set; }
		public int lsky_WorldMoonDirectionID{ get; private set; }
		public int lsky_LocalMoonDirectionID{ get; private set; }
		public int lsky_SunSizeID{ get; private set; }
		public int lsky_SunIntensityID{ get; private set; }
		public int lsky_SunTintID{ get; private set; }
		public int lsky_MoonRTID{ get; private set; }
		public int lsky_MoonSizeID{ get; private set; }
		public int lsky_MoonMatrixID{ get; private set; }
		//=========================================================


		// Atmosphere
		//---------------------------------------------------------
		public int lsky_SunMieColorID{ get; private set; }
		public int lsky_PartialSunMiePhaseID{ get; private set; }
		public int lsky_SunMieScatteringID{ get; private set; }
		public int lsky_MoonMieColorID{ get; private set; }
		public int lsky_PartialMoonMiePhaseID{ get; private set; }
		public int lsky_MoonMieScatteringID{ get; private set; }
		public int lsky_SunAtmosphereTintID{ get; private set; }
		public int lsky_MoonAtmosphereTintID{ get; private set; }
		public int lsky_AtmosphereExponentFadeID{ get; private set; }
		public int lsky_NightRayleighModeID{ get; private set; }
		public int lsky_AtmosphereHazinessID{ get; private set; }
		public int lsky_AtmosphereZenithID{ get; private set; }
		public int lsky_HorizonColorFadeID{ get; private set; }
		public int lsky_BetaRayID{ get; private set; }
		public int lsky_BetaMieID{ get; private set; }
		public int lsky_SunEID{ get; private set; }
		public int lsky_MoonEID{ get; private set; }
		public int lsky_GroundColorID{ get; private set; }
		//---------------------------------------------------------

		// Oneil atmospheric scattering.
		/*public int lsky_kCameraHeightID{get; private set; }
		public int lsky_kCameraPosID{ get; private set; }
		//---------------------------------------------------------
		public int lsky_kInnerRadiusID{ get; private set; }
		public int lsky_kInnerRadius2ID{ get; private set; }
		public int lsky_kOuterRadiusID{ get; private set; }
		public int lsky_kOuterRadius2ID{get; private set; }
		//---------------------------------------------------------
		public int lsky_kScaleID{get; private set; }
		public int lsky_kScaleDepthID{get; private set; }
		public int lsky_kScaleOverScaleDepthID{get; private set; }
		//---------------------------------------------------------
		public int lsky_kKmESunID{get; private set; }
		public int lsky_kKm4PIID{get; private set; }
		//---------------------------------------------------------

		public int lsky_kKr4PIID{get; private set; }
		public int lsky_kKrESunID{get; private set; }
		public int lsky_InvWavelength{get; private set; }*/
		//=========================================================

		//
		
		//
		public int lsky_CloudsCoverageID{ get; private set; }
		public int lsky_CloudsDensityID{ get; private set; }

		#endregion


		//-----------------------------------------Methods------------------------------------------
		//==========================================================================================

		#region |Initialized| 

		private void Awake()
		{

			// Cache transform component.
			m_Transform = this.transform;
			//-------------------------------------------

			// Initialize components for dome elements.
			if(!CheckComponents)
			{
				BuildDome();
			} 
			//------------------------------------------

			// Check Resources
			if(!CheckResources)
			{
				//this.enabled = false;
				Debug.LogWarning("Missing resources, please check that all resources are assigned");
			}
			//----------------------------------------------------------------------------------------

		}

		private void Start()
		{
			
			// Initialize beta ray and beta mie.
			//-----------------------------------------------------
			if(m_AtmosphereModel == LSky_AtmosphereModel.Preetham)
			{
				if(!m_RealTimeBetaRay)
                	GetBetaRay();

				if(!m_RealTimeBetaMie)
                	GetBetaMie();
			}
			//=====================================================

			// Initialize shader property id.
			InitShaderPropertyID();

		}

		private void BuildDome()
		{

			// Deep Space
			//------------------------------------------------------------------------------------------------
			m_GalaxyBackgroundTransform.Instantiate(this.name, "Galaxy Background Transform");
			m_GalaxyBackgroundTransform.InitTransform(m_Transform, Vector3.zero);

			m_StarsFieldTransform.Instantiate(this.name, "Stars Field Transform");
			m_StarsFieldTransform.InitTransform(m_Transform, Vector3.zero);
			//================================================================================================

			// Near Space
			//------------------------------------------------------------------------------------------------
			m_SunTransform.Instantiate(this.name, "Sun Transform");
			m_SunTransform.InitTransform(m_Transform, Vector3.zero);
			//------------------------------------------------------------------------------------------------

			m_MoonTransform.Instantiate(this.name, "Moon Transform");
			m_MoonTransform.InitTransform(m_Transform, Vector3.zero);
			//------------------------------------------------------------------------------------------------

			m_NearSpaceTransform.Instantiate(this.name, "Near Space Transform");
			m_NearSpaceTransform.InitTransform(m_Transform, Vector3.zero);
			//------------------------------------------------------------------------------------------------

			m_MoonCamera.InstantiateCamera(this.name, m_MoonTransform.gameObject.name, "Moon Render Camera");
			m_MoonCamera.InitTransform(m_MoonTransform.transform, new Vector3(0,0,2));
			m_MoonCamera.transform.localEulerAngles  = new Vector3(0,180,0);
			InitMoonCamera();
			//=================================================================================================

			// Atmosphere
			//-------------------------------------------------------------------------------------------------
			m_AtmosphereTransform.Instantiate(this.name, "Atmosphere Transform");
			m_AtmosphereTransform.InitTransform(m_Transform, Vector3.zero);
			//=================================================================================================

			// Clouds
			//-------------------------------------------------------------------------------------------------
			m_CloudsTransform.Instantiate(this.name, "Clouds Transform");
			m_CloudsTransform.InitTransform(m_Transform, Vector3.zero);
			//=================================================================================================

			// Light
			//-------------------------------------------------------------------------------------------------
			m_CelestialsLight.InstantiateLight(this.name, "Celestials Light");
			m_CelestialsLight.InitTransform(m_Transform, Vector3.zero);
			m_CelestialsLight.light.type = LightType.Directional;
			//=================================================================================================
		}

		private void InitMoonCamera()
		{
			m_MoonCamera.camera.enabled = false;
			m_MoonCamera.camera.clearFlags = CameraClearFlags.SolidColor;
			m_MoonCamera.camera.backgroundColor = Color.clear;
			m_MoonCamera.camera.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(m_MoonRenderLayerIndex));
			m_MoonCamera.camera.orthographic = true;
			m_MoonCamera.camera.orthographicSize = 1.2f;
			m_MoonCamera.camera.nearClipPlane = 0.03f;
			m_MoonCamera.camera.farClipPlane = 4;
			m_MoonCamera.camera.targetTexture = m_MoonRenderTexture;
			m_MoonCamera.camera.Render();
		}

		private void InitShaderPropertyID()
		{
			// Dome Matrices
			//---------------------------------------------------------------------------------
			lsky_ObjectToWorldID = Shader.PropertyToID("lsky_ObjectToWorld");
			lsky_WorldToObjectID = Shader.PropertyToID("lsky_WorldToObject");
			//=================================================================================

			// General settings.
			//---------------------------------------------------------------------------------
			lsky_ExposureID      = Shader.PropertyToID("lsky_Exposure");
			//=================================================================================

			// Celestials common properties
			//---------------------------------------------------------------------------------
			lsky_ExponentFadeID  = Shader.PropertyToID("_ExponentFade");
			lsky_CubemapID       = Shader.PropertyToID("_Cubemap");
			lsky_TintID          = Shader.PropertyToID("_Tint");
			lsky_IntensityID     = Shader.PropertyToID("_Intensity");
			lsky_MainTexID       = Shader.PropertyToID("_MainTex");
			//================================================================================

			// Stars Field
			//--------------------------------------------------------------------------------
			lsky_NoiseCubemapID  = Shader.PropertyToID("_NoiseCubemap");
			lsky_ScintillationID = Shader.PropertyToID("_Scintillation");
			lsky_NoiseMatrixID   = Shader.PropertyToID("_NoiseMatrix");
			//================================================================================

			// Sun and Moon
			//--------------------------------------------------------------------------------
			lsky_WorldSunDirectionID   = Shader.PropertyToID("lsky_WorldSunDirection");
			lsky_LocalSunDirectionID   = Shader.PropertyToID("lsky_LocalSunDirection");
			lsky_WorldMoonDirectionID  = Shader.PropertyToID("lsky_WorldMoonDirection");
			lsky_LocalMoonDirectionID  = Shader.PropertyToID("lsky_LocalMoonDirection");
			lsky_SunIntensityID        = Shader.PropertyToID("_SunIntensity");
			lsky_SunTintID             = Shader.PropertyToID("_SunTint");
			lsky_SunSizeID             = Shader.PropertyToID("_SunSize");
			lsky_MoonRTID              = Shader.PropertyToID("_MoonRenderTex");
			lsky_MoonSizeID            = Shader.PropertyToID("_MoonSize");
			lsky_MoonMatrixID          = Shader.PropertyToID("_MoonMatrix");	
			//================================================================================


			// Atmosphere
			//--------------------------------------------------------------------------------
			lsky_SunMieColorID            = Shader.PropertyToID("lsky_SunMieColor");
			lsky_SunMieScatteringID       = Shader.PropertyToID("lsky_SunMieScattering");
			lsky_PartialSunMiePhaseID     = Shader.PropertyToID("lsky_PartialSunMiePhase");
			lsky_MoonMieColorID           = Shader.PropertyToID("lsky_MoonMieColor");
			lsky_MoonMieScatteringID      = Shader.PropertyToID("lsky_MoonMieScattering");
			lsky_PartialMoonMiePhaseID    = Shader.PropertyToID("lsky_PartialMoonMiePhase");
			lsky_SunAtmosphereTintID      = Shader.PropertyToID("lsky_SunAtmosphereTint");
			lsky_MoonAtmosphereTintID     = Shader.PropertyToID("lsky_MoonAtmosphereTint");
			lsky_AtmosphereExponentFadeID = Shader.PropertyToID("lsky_AtmosphereExponentFade");
			lsky_NightRayleighModeID      = Shader.PropertyToID("lsky_NightRayleighMode");
			lsky_AtmosphereHazinessID     = Shader.PropertyToID("lsky_AtmosphereHaziness");
			lsky_AtmosphereZenithID       = Shader.PropertyToID("lsky_AtmosphereZenith");
			lsky_HorizonColorFadeID       = Shader.PropertyToID("lsky_HorizonColorFade");
			lsky_BetaRayID                = Shader.PropertyToID("lsky_BetaRay");
			lsky_BetaMieID                = Shader.PropertyToID("lsky_BetaMie");
			
			lsky_SunEID                   = Shader.PropertyToID("lsky_SunE");
			lsky_MoonEID                  = Shader.PropertyToID("lsky_MoonE");
			lsky_GroundColorID            = Shader.PropertyToID("lsky_GroundColor");
			//===============================================================================

			// Clouds.
			//-------------------------------------------------------------------------------
			lsky_CloudsCoverageID     = Shader.PropertyToID("lsky_CloudsCoverage");
			lsky_CloudsDensityID      = Shader.PropertyToID("lsky_CloudsDensity");
			//===============================================================================



		}

		#endregion

		#region |Update|

		private void LateUpdate(){InternalUpdate();}

		public void InternalUpdate()
		{

			// General Settings
			//----------------------------------------------------------------------------

			// Set global matrices.
			Shader.SetGlobalMatrix(lsky_ObjectToWorldID, m_Transform.localToWorldMatrix);
			Shader.SetGlobalMatrix(lsky_WorldToObjectID, m_Transform.worldToLocalMatrix);
			//-----------------------------------------------------------------------------

			if (!m_ApplyFastTonemaping)
                Shader.DisableKeyword("LSKY_APPLY_FAST_TONEMAPING");
            else
                Shader.EnableKeyword("LSKY_APPLY_FAST_TONEMAPING");

        	Shader.SetGlobalFloat(lsky_ExposureID, m_Exposure);
			//============================================================================

			Celestials();
			//============================================================================

			ComputeAtmosphericScattering();
			//============================================================================

			Clouds();
			//============================================================================

			Lighting();
			//============================================================================

		}

		#endregion

		#region |Celestials| 

		private void Celestials()
		{

			#region |DeepSpace|

			// Galaxy Background
			//-----------------------------------------------------------------------------------------------------
			if(m_RenderGalaxy)
			{
				// Outer space dome size.
				m_GalaxyBackgroundTransform.transform.localScale = DomeRadius3D;

				// Set outer space shader.
				m_Resources.galaxyBackgroundMaterial.shader = m_Resources.galaxyBackgroundShader;

				// Set outer space cubemap to outer space material.
				m_Resources.galaxyBackgroundMaterial.SetTexture(lsky_CubemapID, m_GalaxyCubemap);

				// Set color params to outer space material.
				m_Resources.galaxyBackgroundMaterial.SetColor(lsky_TintID, m_GalaxyColor);
				m_Resources.galaxyBackgroundMaterial.SetFloat(lsky_IntensityID, m_GalaxyIntensity.Evaluate(EvaluateTimeBySun));
				m_Resources.galaxyBackgroundMaterial.SetFloat(lsky_ExponentFadeID, m_GalaxyExponentFade);

				// Draw outer space mesh, we use a low poly sphere since the calculations are made per pixel.
				Graphics.DrawMesh(m_Resources.sphereLOD3, m_GalaxyBackgroundTransform.transform.localToWorldMatrix,
					m_Resources.galaxyBackgroundMaterial, m_GalaxyLayerIndex);
			}

			//======================================================================================================

			// Stars field
			//------------------------------------------------------------------------------------------------------
			if(m_RenderStarsField)
			{
				// Stars field dome size.
				m_StarsFieldTransform.transform.localScale = DomeRadius3D;

				// Set stars field shader.
				m_Resources.starsFieldMaterial.shader = m_Resources.starsFieldShader;

				// Set stars field cubemap to stars field material.
				m_Resources.starsFieldMaterial.SetTexture(lsky_CubemapID, m_StarsFieldCubemap);
				m_Resources.starsFieldMaterial.SetTexture(lsky_NoiseCubemapID, m_StarsFieldNoiseCubemap);

				// Set color params to stars field material.
				m_Resources.starsFieldMaterial.SetColor(lsky_TintID, m_StarsFieldColor);
				m_Resources.starsFieldMaterial.SetFloat(lsky_IntensityID, m_StarsFieldIntensity.Evaluate(EvaluateTimeBySun));

				// Set stars scintillation to stars fiel material.
				m_Resources.starsFieldMaterial.SetFloat(lsky_ScintillationID, m_StarsFieldScintillation);

				// Scroll the X Axis of the noise cubemap.
				m_StarsFieldNoiseXAngle += Time.deltaTime * m_StarsFieldScintillationSpeed;
				m_StarsFieldNoiseXAngle = Mathf.Repeat(m_StarsFieldNoiseXAngle, 360);
				//=====================================================================================================

				Matrix4x4 starsNoiseMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(m_StarsFieldNoiseXAngle, 0, 0), Vector3.one);
				//=====================================================================================================

				m_Resources.starsFieldMaterial.SetMatrix(lsky_NoiseMatrixID, starsNoiseMatrix);
				//=====================================================================================================

				// Draw outer space mesh, we use a low poly sphere since the calculations are made per pixel.
				Graphics.DrawMesh(m_Resources.sphereLOD3, m_StarsFieldTransform.transform.localToWorldMatrix,
					m_Resources.starsFieldMaterial, m_StarsFieldLayerIndex);

			}
			//=========================================================================================================

			#endregion

			#region |Near Space|

			// Sun Position
			//------------------------------------------------------------------------------------------------------
			// Set sun position to sun transform.
			m_SunTransform.transform.localPosition = SunPosition;
			m_SunTransform.transform.LookAt(m_Transform, Vector3.forward);

			// Set sun direction in world space.
			Shader.SetGlobalVector(lsky_WorldSunDirectionID, SunDirection);

			// Set sun direction in local space.
			Shader.SetGlobalVector(lsky_LocalSunDirectionID, m_Transform.InverseTransformDirection(SunDirection));
			//======================================================================================================

			// Moon position
			//------------------------------------------------------------------------------------------------------

			// Set moon position to sun transform.
			m_MoonTransform.transform.localPosition = MoonPosition;
			m_MoonTransform.transform.LookAt(m_Transform, Vector3.down);

			// Set moon direction in world space.
			Shader.SetGlobalVector(lsky_WorldMoonDirectionID, MoonDirection);
			//======================================================================================================

			// Set sun direction in local space.
			Shader.SetGlobalVector(lsky_LocalMoonDirectionID, m_Transform.InverseTransformDirection(MoonDirection) );
			//======================================================================================================


			if(m_RenderNearSpace)
			{

				// Set near space dome size.
				m_NearSpaceTransform.transform.localScale = DomeRadius3D;

				// Set near space shader.
				m_Resources.nearSpaceMaterial.shader = m_Resources.nearSpaceShader;
 
				if(m_RenderSun)
				{

					// Enable sun in near space shader.
                	Shader.EnableKeyword("LSKY_ENABLE_SUN");

					// Set color params to sun material.
					m_Resources.nearSpaceMaterial.SetColor(lsky_SunTintID, m_SunColor.Evaluate(EvaluateTimeBySunAboveHorizon));
					m_Resources.nearSpaceMaterial.SetFloat(lsky_SunIntensityID, m_SunIntensity);
					m_Resources.nearSpaceMaterial.SetFloat(lsky_SunSizeID, m_SunSize.Evaluate(EvaluateTimeBySunAboveHorizon));

				}
				else
				{
					// Disable moon in near space shader.
					Shader.DisableKeyword("LSKY_ENABLE_SUN");
				}

				if(m_RenderMoon)
				{
					

					// Moon mesh size.
					m_MoonTransform.transform.localScale = Vector3.one;

					// Enable moon in near space shader.
                	m_Resources.nearSpaceMaterial.EnableKeyword("LSKY_ENABLE_MOON");

					// Set moon texture to moon material.
					m_Resources.moonMaterial.SetTexture(lsky_MainTexID, m_MoonTexture);
					m_Resources.moonMaterial.SetTextureOffset(lsky_MainTexID, m_MoonTextureOffset);

					// Set color params to moon material.
					m_Resources.moonMaterial.SetColor(lsky_TintID, m_MoonColor.Evaluate(EvaluateTimeByMoonAboveHorizon));
					m_Resources.moonMaterial.SetFloat(lsky_IntensityID, m_MoonIntensity);
					m_Resources.moonMaterial.SetFloat(lsky_ExponentFadeID, m_MoonExponentFade);

					// Set moon render texture to near space material.
					m_Resources.nearSpaceMaterial.SetTexture(lsky_MoonRTID, m_MoonRenderTexture);
					m_Resources.nearSpaceMaterial.SetFloat(lsky_MoonSizeID, m_MoonSize.Evaluate(EvaluateTimeByMoonAboveHorizon));
					m_Resources.nearSpaceMaterial.SetMatrix(lsky_MoonMatrixID, m_MoonTransform.transform.worldToLocalMatrix);
				
					// Draw moon mesh in moon camera.
					Graphics.DrawMesh(m_Resources.sphereLOD1, m_MoonTransform.transform.localToWorldMatrix,
						m_Resources.moonMaterial, m_MoonRenderLayerIndex, m_MoonCamera.camera);

					// Render moon camera.
					if(m_MoonCamera.camera != null)
						m_MoonCamera.camera.Render();

				}
				else
				{   
					// Disable moon in near space shader.
					m_Resources.nearSpaceMaterial.DisableKeyword("LSKY_ENABLE_MOON");
				}
				
				// Draw near space mesh, we use a low poly sphere since the calculations are made per pixel.
				Graphics.DrawMesh(m_Resources.sphereLOD2, m_NearSpaceTransform.transform.localToWorldMatrix,
					m_Resources.nearSpaceMaterial, m_NearSpaceLayerIndex);
				
			}

			#endregion

		}
		
		#endregion

		#region |Atmospheric Scattering|
		Mesh atmosphereDomeMesh;

		private void ComputeAtmosphericScattering()
		{


			if(m_RenderAtmosphere)
			{

				// General Atmospheric Scattering
				//------------------------------------------------------------------------------------------------------------

				// Set atmosphere dome scale.
            	m_AtmosphereTransform.transform.localScale = DomeRadius3D;

				// Set atmosphere shader.
				m_Resources.atmosphereMaterial.shader = m_Resources.atmosphereShader;
				//============================================================================================================

				// Set sun mie phase.
				//------------------------------------------------------------------------------------------------------------
            	Shader.SetGlobalColor(lsky_SunMieColorID, m_SunMieColor.Evaluate(EvaluateTimeBySunAboveHorizon));
            	Shader.SetGlobalVector(lsky_PartialSunMiePhaseID, PartialHenyeyGreenstein(m_SunMieAnisotropy, true));
            	Shader.SetGlobalFloat(lsky_SunMieScatteringID, m_SunMieScattering.Evaluate(EvaluateTimeBySun));
            	//============================================================================================================

				// Set moon mie phase.
				//------------------------------------------------------------------------------------------------------------
            	Shader.SetGlobalColor(lsky_MoonMieColorID, m_MoonMieColor.Evaluate(EvaluateTimeByMoonAboveHorizon));
            	Shader.SetGlobalVector(lsky_PartialMoonMiePhaseID, PartialHenyeyGreenstein(m_MoonMieAnisotropy, false));
            	Shader.SetGlobalFloat(lsky_MoonMieScatteringID, m_MoonMieScattering.Evaluate(EvaluateTimeByMoon)*MoonPhasesIntensityMultiplier);
            	//============================================================================================================

				// Set sun and moon tint.
				//------------------------------------------------------------------------------------------------------------
				Shader.SetGlobalColor(lsky_SunAtmosphereTintID, m_DayAtmosphereTint.Evaluate(EvaluateTimeBySunAboveHorizon));
				
				//============================================================================================================

				switch(m_NightRayleighMode)
				{
					case LSky_NightRayleighMode.OppositeSun:

					Shader.EnableKeyword("LSKY_ENABLE_NIGHT_RAYLEIGH");

					Shader.SetGlobalInt(lsky_NightRayleighModeID, 0);

					Shader.SetGlobalColor(lsky_MoonAtmosphereTintID, m_NightAtmosphereTint * m_MoonBrightness);

					break;

					case LSky_NightRayleighMode.Moon:

					Shader.EnableKeyword("LSKY_ENABLE_NIGHT_RAYLEIGH");

					Shader.SetGlobalInt(lsky_NightRayleighModeID, 1);

					Shader.SetGlobalColor(lsky_MoonAtmosphereTintID, m_NightAtmosphereTint * m_MoonBrightness * MoonPhasesIntensityMultiplier);

					break;

					case LSky_NightRayleighMode.Off:

					Shader.DisableKeyword("LSKY_ENABLE_NIGHT_RAYLEIGH");

					Shader.SetGlobalColor(lsky_MoonAtmosphereTintID, m_NightAtmosphereTint * m_MoonBrightness);

					break;
				}


				// Atmosphere quality
				//------------------------------------------------------------------------------------------------------------
				// Mesh quality.
				switch(m_AtmosphereMeshQuality)
				{

					case LSky_Quality3.Low    : atmosphereDomeMesh = m_Resources.atmosphereLOD2; break;
					case LSky_Quality3.Medium : atmosphereDomeMesh = m_Resources.atmosphereLOD1; break;
					case LSky_Quality3.High   : atmosphereDomeMesh = m_Resources.atmosphereLOD0; break;

				}

				// Shader quality
				switch(m_AtmosphereShaderQuality)
				{
					case LSky_ShaderQuality.PerVertex: Shader.DisableKeyword("LSKY_PER_PIXEL_ATMOSPHERE"); break;
					case LSky_ShaderQuality.PerPixel: Shader.EnableKeyword("LSKY_PER_PIXEL_ATMOSPHERE"); break;
				}
				//===========================================================================================================

				// General Settings.
				//-----------------------------------------------------------------------------------------------------------
				Shader.SetGlobalFloat(lsky_AtmosphereExponentFadeID, m_AtmosphereExponentFade);
				//===========================================================================================================

				// Atmosphere Models
				//-----------------------------------------------------------------------------------------------------------
				PartialAtmosphericScattering();
				//===========================================================================================================

				// Draw Atmosphere Mesh.
				//----------------------------------------------------------------------------------------------------------
				Graphics.DrawMesh(atmosphereDomeMesh, m_AtmosphereTransform.transform.localToWorldMatrix,
					m_Resources.atmosphereMaterial, m_AtmosphereLayerIndex);
				//==========================================================================================================

			}

		}

		private void PartialAtmosphericScattering()
		{


			switch(m_AtmosphereModel)
			{

				case LSky_AtmosphereModel.Preetham:

				// Enable Model In Shader
				//------------------------------------------------------------------------------------
				Shader.EnableKeyword("LSKY_PREETHAM_ATMOSPHERE_MODEL");
				//====================================================================================

				// Rayleigh params.
				//------------------------------------------------------------------------------------
            	Vector3 rayleighParams; // x = SunE, y = MoonE, z = sunset/dawn fade.
            	rayleighParams.x = DayIntensity;
            	rayleighParams.y = NightIntensity;
            	rayleighParams.z = LSky_Mathf.Saturate(Mathf.Clamp01(1.0f - (SunDirection.y)));
				//====================================================================================

				// Set optical depth params.
				//------------------------------------------------------------------------------------
            	Shader.SetGlobalFloat(lsky_AtmosphereHazinessID, m_AtmosphereHaziness);
            	Shader.SetGlobalFloat(lsky_AtmosphereZenithID, m_AtmosphereZenith);
				Shader.SetGlobalFloat(lsky_HorizonColorFadeID, rayleighParams.z);
            	//====================================================================================

				
				// Compute beta ray and beta mie.
				//------------------------------------------------------------------------------------
				if(m_RealTimeBetaRay)
                	GetBetaRay();

				if(m_RealTimeBetaMie)
                	GetBetaMie();
				//====================================================================================
            	
				// Set rayleigh and mie calculations.
				//------------------------------------------------------------------------------------
            	Shader.SetGlobalVector(lsky_BetaRayID, m_BetaRayleigh * m_RayleighScattering.Evaluate(EvaluateTimeBySun));
				Shader.SetGlobalVector(lsky_BetaMieID, m_BetaMie);
				//====================================================================================


				// Set sun and moon intensity.
				//------------------------------------------------------------------------------------
            	Shader.SetGlobalFloat(lsky_SunEID, (m_SunBrightness.Evaluate(EvaluateTimeBySun)*1.5f) * rayleighParams.x);
            	Shader.SetGlobalFloat(lsky_MoonEID, rayleighParams.y*1.5f);
            	//====================================================================================
 
				break;

				case LSky_AtmosphereModel.Oneil:

				// Enable Model In Shader
				//------------------------------------------------------------------------------------
				Shader.DisableKeyword("LSKY_PREETHAM_ATMOSPHERE_MODEL");
				//====================================================================================

				// Set samples
				//--------------------------------------------------------------------
				Shader.SetGlobalInt("lsky_kSamples", m_AtmosphereSamples);
				//====================================================================


				// Radius
				//--------------------------------------------------------------------
				float kInnerRadius  = 1.0f;
            	float kInnerRadius2 = 1.0f;//1.0f;
            	float kOuterRadius  = 1.025f; //1.025f;
            	float kOuterRadius2 = kOuterRadius * kOuterRadius;
				//--------------------------------------------------------------------
				Shader.SetGlobalFloat("lsky_kInnerRadius", kInnerRadius);
            	Shader.SetGlobalFloat("lsky_kInnerRadius2", kInnerRadius2);
            	Shader.SetGlobalFloat("lsky_kOuterRadius", kOuterRadius);
            	Shader.SetGlobalFloat("lsky_kOuterRadius2", kOuterRadius2);
				//====================================================================

				// Camera Position
				//--------------------------------------------------------------------
 				float kCameraHeight = 0.00001f;
            	Vector3 kCameraPos = new Vector3(0,kInnerRadius + kCameraHeight, 0);
				//--------------------------------------------------------------------
				Shader.SetGlobalFloat("lsky_kCameraHeight", kCameraHeight);
            	Shader.SetGlobalVector("lsky_kCameraPos", kCameraPos);
				//====================================================================

				// Scale
				//--------------------------------------------------------------------
				float kScale = (1.0f / (kOuterRadius - 1.0f));
				float kScaleDepth = 0.15f; //0.25f;
            	float kScaleOverScaleDepth = kScale / kScaleDepth;
				//---------------------------------------------------------------------
				Shader.SetGlobalFloat("lsky_kScale", kScale);
            	Shader.SetGlobalFloat("lsky_kScaleDepth", kScaleDepth);
            	Shader.SetGlobalFloat("lsky_kScaleOverScaleDepth", kScaleOverScaleDepth);
				//====================================================================

				// Mie
				//--------------------------------------------------------------------
   				float kSunBrightness = m_SunBrightness.Evaluate(EvaluateTimeBySun); // * EclipseMultiplier;
           		float kMie    = m_Mie;
            	float kKmESun = kMie * kSunBrightness;
            	float kKm4PI  = kMie * 4.0f * Mathf.PI;
				//--------------------------------------------------------------------
				Shader.SetGlobalFloat("lsky_kKmESun", kKmESun);
            	Shader.SetGlobalFloat("lsky_kKm4PI", kKm4PI);
				//====================================================================

				// Rayleigh
				//--------------------------------------------------------------------
				float kRayleigh = 0.0025f * m_RayleighScattering.Evaluate(EvaluateTimeBySun)*2;
            	float kKrESun   = kRayleigh * kSunBrightness;
            	float kKr4PI    = kRayleigh * 4.0f * Mathf.PI;
				//--------------------------------------------------------------------
				// Compute wavelength with reciprocal values.
				Vector3 InvWavelength = new Vector3()
            	{
                	x = 1.0f / Mathf.Pow(m_WavelengthR * 1e-3f, 4.0f),
                	y = 1.0f / Mathf.Pow(m_WavelengthG * 1e-3f, 4.0f),
                	z = 1.0f / Mathf.Pow(m_WavelengthB * 1e-3f, 4.0f)
            	};
				//--------------------------------------------------------------------
				Shader.SetGlobalFloat("lsky_kKr4PI", kKr4PI);
            	Shader.SetGlobalFloat("lsky_kKrESun", kKrESun);
            	Shader.SetGlobalVector("lsky_InvWavelength", InvWavelength);
				//====================================================================

				break;

			}

		}

		// Mie Phase
		//---------------------------------------------------------------
		public Vector3 PartialHenyeyGreenstein(float g, bool HQ)
        {

            Vector3 result;
            {
                float g2 = g * g;
                result.x = HQ ? (1.0f - g2) / (2.0f + g2) : 1.0f - g2;
                result.y = 1.0f + g2;
                result.z = 2.0f * g;
            }
            return result;

        }
		//===============================================================

		// Based on preetham and hoflman papers.
		//-----------------------------------------------------------------------------------------------------
		public void GetBetaRay()
        {
            m_BetaRayleigh = ComputeBetaRay();
        }

        public void GetBetaMie()
        {
            m_BetaMie = ComputeBetaMie();
        }

		private Vector3 ComputeBetaRay()
        {
            Vector3 result;
            //=================================================================================================

            // Wavelength.
            //--------------------------------
            Vector3 wl;
            wl.x = Mathf.Pow(Lambda.x, 4.0f);
            wl.y = Mathf.Pow(Lambda.y, 4.0f);
            wl.z = Mathf.Pow(Lambda.z, 4.0f);
            //=================================================================================================

            // Constant factors.
            //-------------------------------------------------------------------------------------------------
            const float n  = 1.0003f;   // Index of air refraction(n);
            const float N  = 2.545e25f; // Molecular density(N)
            const float pn = 0.035f;    // Depolatization factor for standart air.
            const float n2 = n * n;     // Molecular density exponentially squared.
            //=================================================================================================

            // Beta Rayleigh
            //-------------------------------------------------------------------------------------------------
            float ray = (8.0f * Mathf.Pow(Mathf.PI, 3.0f) * Mathf.Pow(n2 - 1.0f, 2.0f) * (6.0f + 3.0f * pn));
            Vector3 theta = 3.0f * N * wl * (6.0f - 7.0f * pn);
            //-------------------------------------------------------------------------------------------------
            result.x = (ray / theta.x);
            result.y = (ray / theta.y);
            result.z = (ray / theta.z);
            //=================================================================================================
            return result;
            //=================================================================================================
        }

		private Vector3 ComputeBetaMie()
        {
            Vector3 result;
            //==================================================================================

			float turbidity = m_Mie * 0.05f;
            Vector3 k = new Vector3(0.685f, 0.679f, 0.670f);
            float c = (0.2f * turbidity) * 10e-18f;
            float mieFactor = 0.434f * c * Mathf.PI;
            float v = 4.0f;
            //=================================================================================

            result.x = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / Lambda.x, v - 2.0f) * k.x);
            result.y = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / Lambda.y, v - 2.0f) * k.y);
            result.z = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / Lambda.z, v - 2.0f) * k.z);
            //==================================================================================

            return result;
			//==================================================================================
        }

	
		#endregion

		#region |Clouds|

		private void Clouds()
		{

			
			if(m_RenderClouds)
			{
				m_CloudsTransform.transform.localScale = new Vector3(DomeRadius, DomeRadius*m_CloudsMeshHeightSize, DomeRadius);
				m_Resources.cloudsMaterial.shader = m_Resources.cloudsShader;

				if(m_CloudsRotationSpeed != 0.0f)
				{
					m_CloudsYAngle += m_CloudsRotationSpeed * Time.deltaTime;
					m_CloudsYAngle = Mathf.Repeat(m_CloudsYAngle, 360);

					m_CloudsTransform.transform.localRotation = Quaternion.Euler(m_CloudsTransform.transform.localEulerAngles.x, m_CloudsYAngle, m_CloudsTransform.transform.localEulerAngles.z);
				}

				//cloudsMaterial.shader = m_Resources.cloudsShader;

				Color col = m_CloudsColor.Evaluate(EvaluateTimeBySun);

				if(m_NightRayleighMode == LSky_NightRayleighMode.Moon)
					col += m_CloudsMoonColor.Evaluate(EvaluateTimeByMoon)*MoonPhasesIntensityMultiplier;

				m_Resources.cloudsMaterial.SetColor(lsky_TintID, col);
				m_Resources.cloudsMaterial.SetFloat(lsky_IntensityID, m_CloudsIntensity);

				m_Resources.cloudsMaterial.SetTexture(lsky_MainTexID, m_CloudsTexture);

				m_Resources.cloudsMaterial.SetTextureScale(lsky_MainTexID, m_CloudsSize);
				m_Resources.cloudsMaterial.SetTextureOffset(lsky_MainTexID, m_CloudsTexOffset);

				m_Resources.cloudsMaterial.SetFloat(lsky_CloudsCoverageID, m_CloudsCoverage);
				m_Resources.cloudsMaterial.SetFloat(lsky_CloudsDensityID, m_CloudsDensity);

				Graphics.DrawMesh(m_Resources.hemisphereLOD2, m_CloudsTransform.transform.localToWorldMatrix, m_Resources.cloudsMaterial, m_CloudsLayerIndex);
			}



		}


		#endregion

		#region |Lighting|

		private void Lighting()
		{

			// Light
			//--------------------------------------------------------------------------------------------------------------

			if(IsDay)
			{

					m_CelestialsLight.transform.localPosition = SunPosition;
					m_CelestialsLight.transform.LookAt(m_Transform);

					m_CelestialsLight.light.intensity = m_SunLightIntensity.Evaluate(EvaluateTimeBySun);
					m_CelestialsLight.light.color = m_SunLightColor.Evaluate(EvaluateTimeBySun);
			}
			else
			{

				m_CelestialsLight.transform.localPosition = MoonPosition;
				m_CelestialsLight.transform.LookAt(m_Transform);

				m_CelestialsLight.light.intensity = m_MoonLightIntensity.Evaluate(EvaluateTimeByMoon) * NightIntensity * m_SunMoonLightFade.Evaluate(EvaluateTimeBySun);
				m_CelestialsLight.light.intensity *= MoonPhasesIntensityMultiplier;
				m_CelestialsLight.light.color = m_MoonLightColor.Evaluate(EvaluateTimeByMoon);

			}

			m_CelestialsLight.light.enabled = DirLightEnabled;

			//=============================================================================================================

			// Ambient
			//-------------------------------------------------------------------------------------------------------------

			if(m_SendSkybox)
			{
				m_Resources.skyboxMaterial.shader = m_Resources.skyboxShader;
				RenderSettings.skybox = m_Resources.skyboxMaterial;
			}

			RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;

			float updateRate = 1.0f / m_AmbientUpdateInterval;
			m_AmbientRefreshTimer += Time.deltaTime;

			if(m_AmbientRefreshTimer >= updateRate)
		    {

				if(Application.isPlaying)
                    DynamicGI.UpdateEnvironment();
			}

			Shader.SetGlobalColor(lsky_GroundColorID, m_AmbientGroundColor.Evaluate(EvaluateTimeBySun));
			//=============================================================================================================


			// Fog
			//-------------------------------------------------------------------------------------------------------------
			RenderSettings.fog = m_EnableUnityFog;

            if (m_EnableUnityFog)
            {
                RenderSettings.fogMode  = m_UnityFogMode;
                RenderSettings.fogColor = m_UnityFogColor.Evaluate(EvaluateTimeBySun);

				//if(m_NightRayleighMode != LSky_NightRayleighMode.Off)
				//	RenderSettings.fogColor *= NightIntensity;

				if(m_NightRayleighMode == LSky_NightRayleighMode.Moon)
				{
					RenderSettings.fogColor += m_UnityMoonFogColor.Evaluate(EvaluateTimeByMoonAboveHorizon)*MoonPhasesIntensityMultiplier;
					
				}

                if (m_UnityFogMode == FogMode.Linear)
                {
                    RenderSettings.fogStartDistance = m_UnityFogStartDistance;
                    RenderSettings.fogEndDistance   = m_UnityFogEndDistance;
                }
                else
                {
                    RenderSettings.fogDensity = m_UnityFogDensity;
                }
            }
			//============================================================================================================
		}


		#endregion
		//----------------------------------------Accessors-----------------------------------------
		//==========================================================================================


		#region |Accessors|Dome|

		public float DomeRadius
		{
			get{ return m_DomeRadius;  }
			set{ m_DomeRadius = value; }
		}

		#endregion

		#region |Accessors|General|

		public bool ApplyFastTonemaping
		{
			get{ return m_ApplyFastTonemaping;  }
			set{ m_ApplyFastTonemaping = value; }
		}

		public float Exposure
		{
			get{ return m_Exposure;  }
			set{ m_Exposure = value; }
		}

		#endregion

		#region |Accessors|Deep Space|

		public float SunPI
		{

			get{ return m_SunPI; }
			set{ m_SunPI = value; }
		}

		public float SunTheta
		{
			
			get{ return m_SunTheta; }
			set{ m_SunTheta = value;}
		}

		
		public float MoonPI
		{

			get{ return m_MoonPI; }
			set{ m_MoonPI = value; }
		}

		public float MoonTheta
		{
			
			get{ return m_MoonTheta; }
			set{ m_MoonTheta = value;}
		}

		public Transform GalaxyTransformRotation
		{

			get{ return m_GalaxyBackgroundTransform.transform; }
			set
			{

				m_GalaxyBackgroundTransform.transform.rotation = value.rotation;
				m_GalaxyBackgroundTransform.transform.localRotation = value.localRotation;

			}

		}

		public Transform StarsFieldTransformRotation
		{

			get{ return m_StarsFieldTransform.transform; }
			set
			{

				m_StarsFieldTransform.transform.rotation = value.rotation;
				m_StarsFieldTransform.transform.localRotation = value.localRotation;

			}

		}

		#endregion

	}


}
