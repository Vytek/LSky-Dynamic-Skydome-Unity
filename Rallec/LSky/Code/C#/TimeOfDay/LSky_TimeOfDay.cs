//////////////////////////////////
/// LSky
/// =========
///
/// Description:
/// ==============
/// Time of day.
//////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rallec.LSky
{


    [ExecuteInEditMode]
    [RequireComponent(typeof(LSky_Skydome))]
    public class LSky_TimeOfDay : LSky_DateTimeManager
    {

        [SerializeField] private LSky_Skydome m_Skydome = null;

        public LSky_CelestialsCalculations celestialsCalculations = new LSky_CelestialsCalculations();


        public override bool IsDay
        {
            get{ return m_Skydome.IsDay; }
        }

        protected override void Awake()
        {
            base.Awake();
            m_Skydome = GetComponent<LSky_Skydome>();
        }

        protected override void Update()
        {
            base.Update();

            // Set DateTime.
            celestialsCalculations.dateTime = CurrentDateTime;
            {
                // Compute sun coordinates
                celestialsCalculations.ComputeSunCoords();
                m_Skydome.SunPI    = celestialsCalculations.sunAzimuth;
                m_Skydome.SunTheta = celestialsCalculations.sunAltitude;

                // Compute moon coordinates.
                celestialsCalculations.ComputeMoonCoords();
                m_Skydome.MoonPI    = celestialsCalculations.moonAzimuth;
                m_Skydome.MoonTheta = celestialsCalculations.moonAltitude;

                // Stars and Outer Space rotation.
                Quaternion starsRotation = Quaternion.Euler(90 - celestialsCalculations.m_Latitude, 0, 0) *
                    Quaternion.Euler(0, celestialsCalculations.m_Longitude, 0) * Quaternion.Euler(0, celestialsCalculations.m_LST * Mathf.Rad2Deg, 0);

                m_Skydome.GalaxyTransformRotation.localRotation = starsRotation;
                m_Skydome.StarsFieldTransformRotation.localRotation = starsRotation;
            }
        }

    }
}
