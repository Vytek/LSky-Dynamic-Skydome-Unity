///////////////////////////////////////////
/// LSky
/// =====================
///
/// Description:
/// ==============
/// Useful methods for time.
//////////////////////////////////////////


using System;
using UnityEngine;

namespace Rallec.LSky
{

	public class LSky_DateTimeHelper
	{


        #region |Timeline|

 		/// <summary>
        /// Retrun time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour)
        {
            return (float)hour;
        }

        /// <summary>
        /// Return time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour, int minute)
        {
            return (float)hour + ((float)minute / 60f);
        }

        /// <summary>
        /// Return time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour, int minute, int second)
        {
            return (float)hour + ((float)minute / 60f) + ((float)second / 3600f);
        }

        /// <summary>
        /// Retrun time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour, int minute, int second, int millisecond)
        {
            return (float)hour + (float)minute / 60f + (float)second / 3600f + (float)millisecond / 3600000f;
        }
        #endregion

		/*
        #region |String|
  		/// <summary>
        /// Hour and minute to string.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string TimeToString(int hour, int minute)
        {
            string h = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            string m = minute < 10 ? "0" + minute.ToString() : minute.ToString();

            return h + ":" + m;
        }

        /// <summary>
        /// Hour, minute and second to string.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string TimeToString(int hour, int minute, int second)
        {
            string h = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            string m = minute < 10 ? "0" + minute.ToString() : minute.ToString();
            string s = second < 10 ? "0" + second.ToString() : second.ToString();

            return h + ":" + m + ":" + s;
        }
        #endregion*/

	}

}
