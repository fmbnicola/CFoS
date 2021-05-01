using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public class UISlider1DZones : UISlider1D
    {
        [Space(10)]
        public UnityEvent ZoneChangedEvent;

        [SerializeField]
        public List<Zone> Zones;

        private Zone oldZone;
        private Zone currentZone;
        public int ZoneIndex { get; set; }

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
                Zones[0].Min = Min;
                Zones[Zones.Count - 1].Max = Max;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            ZoneIndex = GetZoneIndexFromValue(Value);
            currentZone = Zones[ZoneIndex];
            oldZone = currentZone;
        }

        public override float WorldCoordsToValue(Vector3 coords)
        {
            Vector3 localPos = transform.InverseTransformPoint(coords);
            return HandleToValueAux(localPos.x, out int i);
        }


        private float HandleToValueAux(float handlePos, out int zone_i)
        {
            // Determine in which zone handle is in
            float zoneSize = (Track.End.x / Zones.Count);
            zone_i = Mathf.Clamp(Mathf.FloorToInt(handlePos / zoneSize), 0, Zones.Count - 1);
            Zone zone = Zones[zone_i];

            // Get Zone limits
            float zoneStart = zoneSize * zone_i;
            float zoneEnd = zoneSize * (zone_i + 1);

            // Remap value
            float value = ExtensionMethods.Remap(handlePos, zoneStart, zoneEnd, zone.Min, zone.Max);
            return Mathf.Clamp(value, zone.Min, zone.Max);
        }

        protected override float HandleToValue(float handlePos)
        {
            var val = HandleToValueAux(handlePos, out int zone_i);

            //register current zone
            ZoneIndex = zone_i;
            currentZone = Zones[ZoneIndex];

            return val;
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
            return 0;
        }

        protected override float ValueToHandle(float value)
        {
            // Determine in which zone the value is in
            int zone_i = GetZoneIndexFromValue(value);
            Zone zone = Zones[zone_i];

            // Get zone limits
            float zoneSize = (Track.End.x / Zones.Count);
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
                ZoneChangedEvent.Invoke();
                if (selected) controller.SendHapticImpulse(0.8f, 0.02f);
            }
            oldZone = currentZone;
        }
    }
}
