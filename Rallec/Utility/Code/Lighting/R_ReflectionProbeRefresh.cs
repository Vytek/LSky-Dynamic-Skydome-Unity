///////////////////////////////////////////
/// Utility
/// =================
///
/// Description:
/// ============
/// Refresh reflection probe.
///////////////////////////////////////////


using System; 
using UnityEngine; 
using UnityEngine.Rendering;

namespace Rallec.Utility
{


	[AddComponentMenu("Rallec/Utility/Rendering/Reflection Probe Refresh")]
	[RequireComponent(typeof(ReflectionProbe))]	
	public class R_ReflectionProbeRefresh : MonoBehaviour
	{

		#region |Fields|

		// Show Fields
		// ============
		[SerializeField] protected float m_UpdateInterval  = 15f;
		[SerializeField] protected bool m_SetRenderTexture = false; 
		[SerializeField] protected RenderTexture m_RenderTexture = null;


		// Hide vars.
		// ===========
		protected ReflectionProbe m_Probe = null;
		protected float m_Timer; 

		#endregion


		#region |Methods|

		protected void Awake()
		{
			
			// Initialize Reflection Probe.
			//----------------------------------------------------------------
			m_Probe              = GetComponent<ReflectionProbe>();
			m_Probe.mode         = ReflectionProbeMode.Realtime;
			m_Probe.refreshMode  = ReflectionProbeRefreshMode.ViaScripting;
			
		}
		//=================================================================================

		protected void Update()
		{

			float rate = 1.0f / m_UpdateInterval;

			m_Timer += Time.deltaTime;

			if(m_Timer >= rate)
			{
				if(m_SetRenderTexture)
					m_Probe.RenderProbe(m_RenderTexture);
				else
					m_Probe.RenderProbe(null);

				m_Timer = 0;
			}

		}	
		//=================================================================================

		#endregion


		#region |Accessors|

		public float updateInterval 
		{
			get{ return m_UpdateInterval;  }
			set{ m_UpdateInterval = value; }
		}
		//=================================================================================


		public bool SetRenderTexture
		{
			get{ return m_SetRenderTexture; }
			set{ m_SetRenderTexture = value;}
		}
		//=================================================================================


		public RenderTexture renderTexture 
		{
			get{ return m_RenderTexture;  }
			set{ m_RenderTexture = value; }
		}
		//=================================================================================

		#endregion

	
	}
}
