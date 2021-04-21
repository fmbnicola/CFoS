using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CFoS.UI
{
    [Serializable]
    public class Zone
    {
        [HideInInspector]
        public string Name = "Zone";
        public float Min;
        public float Max;

        public Zone(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    public class UISliderZones : UISlider
    {

        [SerializeField]
        public List<Zone> Zones;
        private Zone currentZone;
        private Zone oldZone;

        [ExecuteAlways]
        protected override void OnValidate()
        {
            base.OnValidate();

            if(Zones == null || Zones.Count == 0)
            {
                Zones = new List<Zone>(){ new Zone(Min, Max) };
            }
            else
            {
                var first = Zones[0];
                first.Min = Min;

                var last = Zones[Zones.Count - 1];
                last.Max = Max;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            var zone_i = GetZoneIndexFromValue(Value);
            currentZone = Zones[zone_i];
            oldZone = currentZone;
        }

        protected override float HandleToValue(float handlePos)
        {
            // Determine in which zone handle is in
            float zoneSize = (Track.End.x / Zones.Count);
            int zone_i = Mathf.Clamp(Mathf.FloorToInt(handlePos / zoneSize), 0, Zones.Count-1);
            Zone zone = Zones[zone_i];

            //register current zone
            currentZone = zone;

            // Get Zone limits
            float zoneStart = zoneSize *  zone_i;
            float zoneEnd   = zoneSize * (zone_i + 1);

            // Remap value
            float value = ExtensionMethods.Remap(handlePos, zoneStart, zoneEnd, zone.Min, zone.Max);
            return Mathf.Clamp(value, zone.Min, zone.Max);
        }

        private int GetZoneIndexFromValue(float value)
        {
            Zone zone = null;
            for (var i = 0; i < Zones.Count; i++)
            {
                zone = Zones[i];
                if (value >= zone.Min && value <= zone.Max)
                {
                    return i;
                }
            }
            return -1;
        }

        protected override float ValueToHandle(float value)
        {
            // Determine in which zone the value is in
            float zoneSize = (Track.End.x / Zones.Count);

            int zone_i = GetZoneIndexFromValue(value);
            if (zone_i == -1) return 0;

            Zone zone = Zones[zone_i];

            // Get zone limits
            float zoneStart = zoneSize * zone_i;
            float zoneEnd   = zoneSize * (zone_i + 1);

            float handlePos = ExtensionMethods.Remap(value, zone.Min, zone.Max, zoneStart, zoneEnd);
            return Mathf.Clamp(handlePos, zoneStart, zoneEnd);
        }


        protected override void Update()
        {
            base.Update();

            // check zone change
            if (oldZone != currentZone)
            {
                if (selected) controller.SendHapticImpulse(0.8f, 0.02f);
            }
            oldZone = currentZone;
        }
    }
}
