//////////////////////////////////////////////////////////
/// LSky
/// =====================
///
///
/// Description:
/// =====================
/// Compute planetary positions.
///
/// All calculations are based on Paul Schlyter papers.
/// See: http://www.stjarnhimlen.se/comp/ppcomp.html
/// See: http://stjarnhimlen.se/comp/tutorial.html
/////////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Rallec.LSky
{


    [System.Serializable]
    public class LSky_CelestialsCalculations
    {




        [SerializeField, Range(-90f, 90f)] public float m_Latitude;
        [SerializeField, Range(-180f, 180f)] public float m_Longitude;
        [SerializeField, Range(-12f, 12f)] public float m_UTC;


        [HideInInspector]
        public System.DateTime dateTime;


        // Sun distance(r).
        internal float m_SunDistance;

        // True sun longitude.
        internal float m_TrueSunLongitude;

        // Mean sun longitude.
        internal float m_MeanSunLongitude;

        // Sideral time.
        internal float m_SideralTime;

        [HideInInspector]
        public float m_LST;


        /// <summary>
        /// Return Hour - UTC.
        /// </summary>
        public float Hour_UTC_Apply { get { return (float)dateTime.TimeOfDay.TotalHours - m_UTC; } }

        /// <summary>
        /// Return latitude in radians.
        /// </summary>
        public float Latitude_Rad { get { return Mathf.Deg2Rad * m_Latitude; } }


        /// <summary>
        /// Time Scale (d).
        /// </summary>
        private float TimeScale
        {
            get
            {
                return (367 * dateTime.Year - (7 * (dateTime.Year + ((dateTime.Month + 9) / 12))) / 4 +
                    (275 * dateTime.Month) / 9 + dateTime.Day - 730530) + (float)dateTime.TimeOfDay.TotalHours / 24;
            }
        }



        /// <summary>
        /// Obliquity of the ecliptic.
        /// </summary>
        private float Oblecl { get { return Mathf.Deg2Rad * (23.4393f - 3.563e-7f * TimeScale); } }

        public float sunAltitude, sunAzimuth;
        public float moonAltitude, moonAzimuth;

        public void ComputeSunCoords()
        {

            //Vector3 result;

            #region |Orbital Elements|

           // float N = 0;                                     // Longitude of the ascending node.
           // float i = 0;                                     // The Inclination to the ecliptic.
            float w = 282.9404f + 4.70935e-5f   * TimeScale; // Argument of perihelion.
           // float a = 0;                                     // Semi-major axis, or mean distance from sun.
            float e = 0.016709f - 1.151e-9f     * TimeScale; // Eccentricity.
            float M = 356.0470f + 0.9856002585f * TimeScale; // Mean anomaly.

            M = Rev(M);

            // Mean anomaly to radians.
            float M_Rad = M * Mathf.Deg2Rad;

            #endregion

            #region |Eccentric Anomaly|

            // Compute eccentric anomaly.
            float E = M + Mathf.Rad2Deg * e * Mathf.Sin(M_Rad) * (1 + e * Mathf.Cos(M_Rad));

            // Eccentric anomaly to radians.
            float E_Rad = Mathf.Deg2Rad * E;// Debug.Log(E);

            #endregion

            #region |Rectangular Coordinates|

            // Rectangular coordinates of the sun in the plane of the ecliptic.
            float xv = (Mathf.Cos(E_Rad) - e);// Debug.Log(xv);
            float yv = (Mathf.Sin(E_Rad) * Mathf.Sqrt(1 - e * e)); // Debug.Log(yv);

            // Convert to distance and true anomaly(r = radians, v = degrees).
            float r = Mathf.Sqrt(xv * xv + yv * yv);             // Debug.Log(r);
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);         //       Debug.Log(v);

            // Get sun distance.
            m_SunDistance = r;

            #endregion

            #region |True Longitude|

            // True sun longitude.
            float lonsun = v + w;

            // Rev sun longitude
            lonsun = Rev(lonsun); // Debug.Log(lonsun);

            // True sun longitude to radians.
            float lonsun_Rad = Mathf.Deg2Rad * lonsun;

            // Set true sun longitude(radians) for use in others celestials calculations.
            m_TrueSunLongitude = lonsun_Rad;

            #endregion

            #region |Ecliptic And Equatorial Coordinates|

            // Ecliptic rectangular coordinates(radians):
            float xs = r * Mathf.Cos(lonsun_Rad);
            float ys = r * Mathf.Sin(lonsun_Rad);

            // Ecliptic rectangular coordinates rotate these to equatorial coordinates(radians).
            float oblecl_Cos = Mathf.Cos(Oblecl);
            float oblecl_Sin = Mathf.Sin(Oblecl);

            float xe = xs;
            float ye = ys * oblecl_Cos - 0.0f * oblecl_Sin;
            float ze = ys * oblecl_Sin + 0.0f * oblecl_Cos;

            #endregion

            #region |Ascension And Declination|

            // Right ascension(degrees):
            float RA = Mathf.Rad2Deg * Mathf.Atan2(ye, xe) / 15;

            // Declination(radians).
            float Decl = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            #endregion

            #region |Mean Longitude|

            // Mean sun longitude(degrees).
            float L = w + M;

            // Rev mean sun longitude.
            L = Rev(L);

            // Set mean sun longitude for use in other celestials calculations.
            m_MeanSunLongitude = L;

            #endregion


            #region Sideral Time.

            // Sideral time(degrees).
            float GMST0 = /*(L + 180) / 15;*/  ((L / 15) + 12);

             m_SideralTime = GMST0 + Hour_UTC_Apply + m_Longitude / 15 + 15/15 ;
             m_LST = Mathf.Deg2Rad * m_SideralTime*15;

             // Hour angle(degrees).
            float HA = (m_SideralTime - RA)*15; //Debug.Log(HA);

            // Hour angle in radians.
            float HA_Rad = Mathf.Deg2Rad * HA;

            //Debug.Log(HA);


            #endregion


            #region |Hour Angle And Declination In Rectangular Coordinates|

            // HA anf Decl in rectangular coordinates(radians).
            float Decl_Cos = Mathf.Cos(Decl);

            // X axis points to the celestial equator in the south.
            float x = Mathf.Cos(HA_Rad) * Decl_Cos;// Debug.Log(x);

            // Y axis points to the horizon in the west.
            float y = Mathf.Sin(HA_Rad) * Decl_Cos; // Debug.Log(y);

            // Z axis points to the north celestial pole.
            float z = Mathf.Sin(Decl);// Debug.Log(z);

            // Rotate the rectangualar coordinates system along of the Y axis(radians).
            float sinLatitude = Mathf.Sin(Latitude_Rad);
            float cosLatitude = Mathf.Cos(Latitude_Rad);


            float xhor = x * sinLatitude - z * cosLatitude;  // Debug.Log(xhor);
            float yhor = y;
            float zhor = x * cosLatitude + z * sinLatitude;// Debug.Log(zhor);

            #endregion


            #region Azimuth, Altitude And Zenith[Radians].

            sunAzimuth = Mathf.Atan2(yhor, xhor) + Mathf.PI; // Azimuth.
            sunAltitude =LSky_Mathf.k_HalfPI - Mathf.Atan2 (zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor)); // Altitude.

            #endregion


            //return result;


        }


        public void ComputeMoonCoords()
        {

            //Vector3 result;

            #region |Orbital Elements|

            float N = 125.1228f - 0.0529538083f * TimeScale;  // Longitude of the ascending node.
            float i = 5.1454f;                                // The Inclination to the ecliptic.
            float w = 318.0634f + 0.1643573223f * TimeScale;  // Argument of perihelion.
            float a = 60.2666f;                               // Semi-major axis, or mean distance from sun.
            float e = 0.054900f;                              // Eccentricity.
            float M = 115.3654f + 13.0649929509f * TimeScale; // Mean anomaly.

            // Normalize elements.
            N = Rev(N);
            w = Rev(w);
            M = Rev(M);

            // Orbital elements in radians.
            float N_Rad = Mathf.Deg2Rad * N;
            float i_Rad = Mathf.Deg2Rad * i;
            float M_Rad = Mathf.Deg2Rad * M;

            #endregion

            #region |Eccentric Anomaly|

            // Compute eccentric anomaly.
            float E = M + Mathf.Rad2Deg * e * Mathf.Sin(M_Rad) * (1 + e * Mathf.Cos(M_Rad));

            // Eccentric anomaly to radians.
            float E_Rad = Mathf.Deg2Rad * E;

            #endregion

            #region |Rectangular Coordinates|

            // Rectangular coordinates of the sun in the plane of the ecliptic.
            float xv = a * (Mathf.Cos(E_Rad) - e); // Debug.Log(xv);
            float yv = a * (Mathf.Sin(E_Rad) * Mathf.Sqrt(1 - e * e)) * Mathf.Sin(E_Rad); // Debug.Log(yv);

            // Convert to distance and true anomaly(r = radians, v = degrees).
            float r = Mathf.Sqrt(xv * xv + yv * yv);         // Debug.Log(r);
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);   // Debug.Log(v);

            v = Rev(v);

            // Longitude in radians.
            float l = Mathf.Deg2Rad * (v + w);

            float Cos_l = Mathf.Cos(l);
            float Sin_l = Mathf.Sin(l);
            float Cos_N_Rad = Mathf.Cos(N_Rad);
            float Sin_N_Rad = Mathf.Sin(N_Rad);
            float Cos_i_Rad = Mathf.Cos(i_Rad);

            float xeclip = r * (Cos_N_Rad * Cos_l - Sin_N_Rad * Sin_l * Cos_i_Rad);
            float yeclip = r * (Sin_N_Rad * Cos_l + Cos_N_Rad * Sin_l * Cos_i_Rad);
            float zeclip = r * (Sin_l * Mathf.Sin(i_Rad));

            #endregion

            #region Geocentric Coordinates.

            // Geocentric position for the moon and Heliocentric position for the planets.
            float lonecl = Mathf.Rad2Deg * Mathf.Atan2(yeclip, xeclip);

            // Rev lonecl
            lonecl = Rev(lonecl);     // Debug.Log(lonecl);

            float latecl = Mathf.Rad2Deg * Mathf.Atan2(zeclip, Mathf.Sqrt(xeclip * xeclip + yeclip * yeclip));   // Debug.Log(latecl);

            // Get true sun longitude.
            // float lonSun = m_TrueSunLongitude;

            // Ecliptic longitude and latitude in radians.
            float lonecl_Rad = Mathf.Deg2Rad * lonecl;
            float latecl_Rad = Mathf.Deg2Rad * latecl;

            float nr = 1.0f;
            float xh = nr * Mathf.Cos(lonecl_Rad) * Mathf.Cos(latecl_Rad);
            float yh = nr * Mathf.Sin(lonecl_Rad) * Mathf.Cos(latecl_Rad);
            float zh = nr * Mathf.Sin(latecl_Rad);

            // Geocentric posisition.
            float xs = 0.0f;
            float ys = 0.0f;

            // Convert the geocentric position to heliocentric position.
            float xg = xh + xs;
            float yg = yh + ys;
            float zg = zh;

            #endregion

            #region |Equatorial Coordinates|

            // Convert xg, yg in equatorial coordinates.
            float oblecl_Cos = Mathf.Cos(Oblecl);
            float oblecl_Sin = Mathf.Sin(Oblecl);

            float xe = xg;
            float ye = yg * oblecl_Cos - zg * oblecl_Sin;
            float ze = yg * oblecl_Sin + zg * oblecl_Cos;

            #endregion

            #region |Ascension, Declination And Hour Angle|

            // Right ascension.
            float RA = Mathf.Rad2Deg * Mathf.Atan2(ye, xe); //Debug.Log(RA);

            // Normalize right ascension.
            RA = Rev(RA);  //Debug.Log(RA);

            // Declination.
            float Decl = Mathf.Rad2Deg * Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            // Declination in radians.
            float Decl_Rad = Mathf.Deg2Rad * Decl;

            // Hour angle.
            float HA = ((m_SideralTime * 15) - RA); //Debug.Log(HA);

            // Rev hour angle.
            HA = Rev(HA);     //Debug.Log(HA);

            // Hour angle in radians.
            float HA_Rad = Mathf.Deg2Rad * HA;

            #endregion

            #region |Declination in rectangular coordinates|

            // HA y Decl in rectangular coordinates.
            float Decl_Cos = Mathf.Cos(Decl_Rad);
            float xr = Mathf.Cos(HA_Rad) * Decl_Cos;
            float yr = Mathf.Sin(HA_Rad) * Decl_Cos;
            float zr = Mathf.Sin(Decl_Rad);

            // Rotate the rectangualar coordinates system along of the Y axis(radians).
            float sinLatitude = Mathf.Sin(Latitude_Rad);
            float cosLatitude = Mathf.Cos(Latitude_Rad);

            float xhor = xr * sinLatitude - zr * cosLatitude;
            float yhor = yr;
            float zhor = xr * cosLatitude + zr * sinLatitude;

            #endregion

            #region |Azimuth, Altitude And Zenith[Radians]|

            moonAzimuth = Mathf.Atan2(yhor, xhor) + Mathf.PI;
            moonAltitude = LSky_Mathf.k_HalfPI - Mathf.Atan2 (zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor)); // Altitude.

            #endregion

            //return result;
        }



        float Rev(float x)
        {
            return x - Mathf.Floor(x / 360f) * 360f;
        }


    }
}
