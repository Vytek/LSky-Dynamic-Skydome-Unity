/////////////////////////////////////////
/// LSky.
/// =========
///
/// Description
/// ============
/// Date Time class.
/////////////////////////////////////////

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rallec.LSky
{

    [ExecuteInEditMode][AddComponentMenu("Rallec/LSky/DateTime/DateTime Manager")]
    public class LSky_DateTimeManager : MonoBehaviour
    {

        #region |Fields|

        // Timeline.
        //--------------------------------------------------------------------------
        [SerializeField] protected bool m_AllowProgressTime = true;
        [SerializeField] protected float m_Timeline = 10.5f;
        //==========================================================================

        // Length.
        //--------------------------------------------------------------------------
        // Use to set the length of the day and night.
		[SerializeField] protected bool m_EnableDayNightLength = true;

        // Start and finish hour of the day.
		[SerializeField] protected Vector2 m_DayRange = new Vector2(6.0f, 19f);

        // Day in real minutes.
		[SerializeField] protected float m_DayLength = 15f;

        // Night in real minutes.
        [SerializeField] protected float m_NightLength = 7.5f;
        //=========================================================================

        // Date.
        //-------------------------------------------------------------------------
        // Day of the year.
        [SerializeField, Range(1, 31)] protected int m_Day = 1; 

        // Month of the year.
        [SerializeField, Range(1, 12)] protected int m_Month = 1; 

        // Year. 
        [SerializeField, Range(1, 9999)] protected int m_Year  = 2019; 
        //========================================================================

        // System.
        //------------------------------------------------------------------------
        // Synchronize with the system date and time.
        [SerializeField] protected bool m_SyncWithSystem = false; 
        //========================================================================

        // Constants.
        //------------------------------------------------------------------------
        // Timeline legth.
        public const float k_TimelineLength = 24; 
        //========================================================================

        #endregion

        #region |Properties|

        // DateTime
        //----------------------------------------------------------------------------
        public DateTime CurrentDateTime
        {
            get
            {
                if(m_SyncWithSystem)
                {

                    DateTime systemDateTime = DateTime.Now;

                    // Get system time in timeline.
                    if (m_AllowProgressTime)
                        m_Timeline = (float)systemDateTime.TimeOfDay.TotalHours;

                    m_Year  = systemDateTime.Year;   // Get system year.
                    m_Month = systemDateTime.Month;  // Get system month.
                    m_Day   = systemDateTime.Day;    // Get system day.

                    return systemDateTime;
                }
                else
                {

                    DateTime dateTime = new DateTime(0, DateTimeKind.Utc);

                    // Repeat full date cycle for prevent exccess.
                    RepeatDateTimeCycle(); 

                    // Add date and time in DateTime.
                    dateTime = dateTime.AddYears(m_Year - 1).AddMonths(m_Month - 1).AddDays(m_Day - 1).AddHours(m_Timeline);

                    
                    // Get date.
                    m_Year  = dateTime.Year;
                    m_Month = dateTime.Month;
                    m_Day   = dateTime.Day;

                    // Time To Float(Get time in timeline);
                    m_Timeline = (float)dateTime.Hour + (float)dateTime.Minute / 60f + (float)dateTime.Second / 3600f + (float)dateTime.Millisecond / 3600000f;


                    return dateTime;
                }

            }
        }

        // Length.
        //----------------------------------------------------------------------------
        /// <summary>
        /// Indicates if it is day.
        /// </summary>
        public virtual bool IsDay
        {
            get{ return (m_Timeline >= m_DayRange.x && m_Timeline < m_DayRange.y); }
        }
        
        /// <summary>
        /// Cycle duration in minutes.
        /// </summary>
        public float DurationCycle
        {
            get
            {
                if(m_EnableDayNightLength)
                    return IsDay ? 60*m_DayLength*2 : 60*m_NightLength*2;

                return m_DayLength*60;
            }
        }

        //============================================================================

        #endregion
        
        #region |UnityEvents|

		// They are triggered when the values of time and date change.
        //=============================================================

        [SerializeField] protected UnityEvent OnHourChanged;
        [SerializeField] protected UnityEvent OnMinuteChanged;
        [SerializeField] protected UnityEvent OnDayChanged;
        [SerializeField] protected UnityEvent OnMonthChanged;
        [SerializeField] protected UnityEvent OnYearChanged;
        //=============================================================


		// They are used to trigger events.
        //-------------------------------------
        protected int
            m_LastHour, m_LastMinute,
            m_LastDay, m_LastMonth,
            m_LastYear;
        //=====================================

		#endregion

        #region |Methods|Initialize|

        protected virtual void Awake()
        {
            // Initialize Timeline.
			//----------------------------------------------------------------------------------------
            m_Timeline = m_SyncWithSystem ? (float)CurrentDateTime.TimeOfDay.TotalHours : m_Timeline;

             // Initialize Time.
			//----------------------------------------------------------------------------------------
            SetHour(CurrentDateTime.Hour);
            SetMinute(CurrentDateTime.Minute);
            SetSecond(CurrentDateTime.Second);
            SetMillisecond(CurrentDateTime.Millisecond);
            //========================================================================================

             // Initialize last date
            //----------------------------------------------------------------------------------------
            m_LastYear   = CurrentDateTime.Year;
            m_LastMonth  = CurrentDateTime.Month;
            m_LastDay    = CurrentDateTime.Day;
            m_LastHour   = CurrentDateTime.Hour;
            m_LastMinute = CurrentDateTime.Minute;
            //========================================================================================
        }

        #endregion

        #region |Update|

        protected virtual void Update()
        {
             // Progress Time.
            if(m_AllowProgressTime)
                m_Timeline += (DurationCycle != 0 && Application.isPlaying) ? (Time.deltaTime / DurationCycle) * k_TimelineLength : 0.0f;

            CheckUnityEvents();    
        }

        #endregion

        #region |SetTime|

        
        public void SetHour(int value)
        {
            if (value >= 0 && value < 25)
                   m_Timeline = LSky_DateTimeHelper.TimeToFloat(value, CurrentDateTime.Minute, CurrentDateTime.Second, CurrentDateTime.Millisecond);
        }
        public void SetMinute(int value)
        {
            if (value >= 0 && value < 61)
                   m_Timeline = LSky_DateTimeHelper.TimeToFloat(CurrentDateTime.Hour, value, CurrentDateTime.Second, CurrentDateTime.Millisecond);
        }

        public void SetSecond(int value)
        {
            if (value >= 0 && value < 61)
                   m_Timeline = LSky_DateTimeHelper.TimeToFloat(CurrentDateTime.Hour, CurrentDateTime.Minute, value, CurrentDateTime.Millisecond);
        }

        public void SetMillisecond(int value)
        {
            if(value >= 0 && value < 1001)
                 m_Timeline = LSky_DateTimeHelper.TimeToFloat(CurrentDateTime.Hour, CurrentDateTime.Minute, CurrentDateTime.Second, value);
        }

        /// <summary<
        /// Set time to timeline.
        /// </summary>
		public void SetTime(int hours, int minutes, int seconds)
        {

            m_Timeline = LSky_DateTimeHelper.TimeToFloat(hours, minutes, seconds, CurrentDateTime.Millisecond);
        }
        //=========================================================================================

        /// <summary<
        /// Set time to timeline.
        /// </summary>
        public void SetTime(int hours, int minutes, int seconds, int milliseconds)
        {

            m_Timeline = LSky_DateTimeHelper.TimeToFloat(hours, minutes, seconds, milliseconds);
        }
        //====================================================================================================================

        #endregion

        #region |Methods|Timecycle|

        // The cycle is repeated to avoid excesses in the DateTime.
	    private void RepeatDateTimeCycle()
        {

            // Prevent fordward excesses.
            //----------------------------------------------------------------------------------
            if (m_Year == 9999 && m_Month == 12 && m_Day == 31 && m_Timeline >= 23.999999f)
            {

                m_Year  = 1;
                m_Month = 1;
                m_Day   = 1;

                m_Timeline = 0.0f;
            }
            //===================================================================================

            // Prevent backward excesses.
            //-----------------------------------------------------------------------------------
            if (m_Year == 1 && m_Month == 1 && m_Day == 1 && m_Timeline < 0.0f)
            {
               
                m_Year  = 9999;
                m_Month = 12;
                m_Day   = 31;

                m_Timeline = 23.999999f;
            }
            //===================================================================================
        }
        #endregion

        #region |Methods|Events|
        protected void CheckUnityEvents()
        {

            if(m_LastHour != CurrentDateTime.Hour)
            {
                
                OnHourChanged.Invoke();
                //Debug.Log("OnHour");
                m_LastHour = CurrentDateTime.Hour;
            }
            //================================

            if(m_LastMinute != CurrentDateTime.Minute)
            {
                OnMinuteChanged.Invoke();
                //Debug.Log("OnMinute");
                m_LastMinute = CurrentDateTime.Minute;
            }
            //================================

            if(m_LastDay != CurrentDateTime.Day)
            {
                OnDayChanged.Invoke();
                //Debug.Log("OnDay");
                m_LastDay = CurrentDateTime.Day;
            }
            //================================

            if(m_LastMonth != CurrentDateTime.Month)
            {
                OnMonthChanged.Invoke();
                //Debug.Log("OnMonth");
                m_LastMonth = CurrentDateTime.Month;
            }
            //================================

            if(m_LastYear != CurrentDateTime.Year)
            {
                OnYearChanged.Invoke();
                //Debug.Log("OnYear");
                m_LastYear = CurrentDateTime.Year;
            }
            //================================

        }
        #endregion
    
        #region |Accessors|

        // Timeline
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Allow progress time.
        /// </summary>
		public bool AllowProgressTime
		{ 
			get{ return m_AllowProgressTime;  }
			set{ m_AllowProgressTime = value; }
		}
		

        /// <summary>
        /// Time in float value.
        /// </summary>
        public float Timeline
        {
            get{ return m_Timeline; }
            set
			{
                if(value > 0.0f && value < 24.000001f && !m_SyncWithSystem)
                    m_Timeline = value;
            }
        }
	    //===========================================================================


        // Length
        //---------------------------------------------------------------------------
		
        /// <summary>
        /// Enable day/night length.
        /// </summary>
		public bool EnableDayNightLength
		{
			get{ return m_EnableDayNightLength;  }
			set{ m_EnableDayNightLength = value; }
		}
	
        /// <summary>
        /// Day range.
        /// </summary>
		public Vector2 DayRange
		{
			get{ return m_DayRange; }
			set{ m_DayRange = value; }
		} 

        /// <summary>
        /// Duration day in minutes.
        /// </summary>
		public float DayLength
		{
			get{ return m_DayLength; }
			set{ m_DayLength = value; }
		}


        /// <summary>
        /// Duration night in minutes.
        /// </summary>
		public float NightLength
		{
			get{ return m_NightLength; }
			set{ m_NightLength = value; }
		}
		//===========================================================================

		
        // Date
        //---------------------------------------------------------------------------

		/// <summary>
        /// Day of the year Range: [1-31].
        /// </summary>
        public int Day
        {
            get{ return m_Day; }
            set
            {
                if (value > 0 && value < 32 && !m_SyncWithSystem)
                    m_Day = value;
            }
        }
       
		
		/// <summary>
        /// Month Range: [1-12].
        /// </summary>
        public int Month
        {
            get{ return m_Month; }
            set
            {
                if (value > 0 && value < 13 && !m_SyncWithSystem)
                    m_Month = value;
            }
        }


		/// <summary>
        /// Year Range: [1-9999].
        /// </summary>
        public int Year
        {
            get{ return m_Year; }
            set
            {
                if (value > 0 && value < 10000 && !m_SyncWithSystem)
                    m_Year = value;
            }
        }
        //===========================================================================

        // System
        //---------------------------------------------------------------------------

        /// <summary>
        /// Syncronize with the system date time.
        /// </summary>
		public bool SyncWithSystem
		{
			get{ return m_SyncWithSystem; }
			set{ m_SyncWithSystem = value; }
		}

        #endregion

        #region |Custom Inspector|

        #if UNITY_EDITOR

        [SerializeField, Range(0,24)] protected int m_Hour;
        [SerializeField, Range(0, 60)] protected int m_Minute;
        [SerializeField, Range(0, 60)] protected int m_Second;
        [SerializeField, Range(0, 1000)] protected int m_Milliseconds;

        #endif

        #endregion

    }
}
