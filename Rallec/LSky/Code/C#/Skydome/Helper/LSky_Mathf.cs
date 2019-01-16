///////////////////////////////
/// LSky
/// ====================
///
/// Description:
/// =============
/// Math for skydome.
///////////////////////////////

using UnityEngine;

namespace Rallec.LSky
{

	public struct LSky_Mathf
	{


		#region PI

		/// <summary>
		/// PI/2
		/// </summary>
		public const float k_HalfPI = 1.570796f;

        /// <summary>
        /// PI*2
        /// </summary>
        //public const float k_TAU = 6.283185f;

        /// <summary>
        /// 1 / (4*pi)
        /// </summary>
        public const float k_PI14 = 0.079577f;

        /// <summary>
        /// 3 / (16 * pi)
        /// </summary>
        public const float k_PI316 = 0.059683f;

		#endregion

		#region |Celestials|

		/// <summary>
        /// Convert sperical coordinates to cartesian coordinates.
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static Vector3 SphericalToCartesian(float theta, float pi)
        {

			// See: https://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates

			Vector3 result;
			//================

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPI    = Mathf.Sin(pi);
            float cosPI    = Mathf.Cos(pi);
			//====================================

            result.x = sinTheta * sinPI;
            result.y = cosTheta;
            result.z = sinTheta * cosPI;
			//====================================

            return result;
        }
		//==============================================================================


        /// <summary>
        /// Convert sperical coordinates to cartesian coordinates.
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="pi"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector3 SphericalToCartesian(float theta, float pi, float rad)
        {

			// See: https://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates

            rad = Mathf.Max(0.5f, rad);

           	Vector3 result;
			//================

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPI    = Mathf.Sin(pi);
            float cosPI    = Mathf.Cos(pi);
			//====================================

            result.x = rad * sinTheta * sinPI;
            result.y = rad * cosTheta;
            result.z = rad * sinTheta * cosPI;
			//====================================

            return result;
        }
		//===============================================================================


		#endregion

        #region |Generic Math|

        public static float Saturate(float x)
		{
			return Mathf.Max(0,Mathf.Min(1, x));
		}

        #endregion


	}


}
