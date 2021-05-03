using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.UI
{
    public class UISlider2DZones : UISlider2D
    {
        [Space(10)]
        public UnityEvent ZoneChangedEvent;

        [SerializeField]
        public List<Zone> XZones;
        public List<Zone> YZones;

        private Zone oldXZone;
        private Zone currentXZone;
        private Zone oldYZone;
        private Zone currentYZone;
        public Vector2Int ZoneIndex { get; set; }

        [ExecuteAlways]
        protected override void OnValidate()
        {
            base.OnValidate();

            if(XZones == null || XZones.Count == 0)
            {
                XZones = new List<Zone>(){ new Zone(XMin, XMax) };
            }
            else
            {
                XZones[0].Min = XMin;
                XZones[XZones.Count - 1].Max = XMax;
            }

            if (YZones == null || YZones.Count == 0)
            {
                YZones = new List<Zone>() { new Zone(YMin, YMax) };
            }
            else
            {
                YZones[0].Min = YMin;
                YZones[YZones.Count - 1].Max = YMax;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            ZoneIndex = GetZoneIndexFromValue(Value);

            currentXZone = XZones[ZoneIndex.x];
            oldXZone = currentXZone;

            currentYZone = YZones[ZoneIndex.y];
            oldYZone = currentYZone;
        }

        public override Vector2 WorldCoordsToValue(Vector3 coords)
        {
            Vector3 localPos = transform.InverseTransformPoint(coords);
            return HandleToValueAux(localPos.x, localPos.y, out Vector2Int zone_i);
        }


        private Vector2 HandleToValueAux(float handlePosX, float handlePosY, out Vector2Int zone_i)
        {
            float zoneSize, zoneStart, zoneEnd;
            int ix, iy;
            float xx, yy;

            // ==== XX ====
            // Get zone
            zoneSize = (Track.Width / XZones.Count);
            ix = Mathf.Clamp(Mathf.FloorToInt(handlePosX / zoneSize), 0, XZones.Count - 1);
            Zone xZone = XZones[ix];

            // Get Zone limits
            zoneStart = zoneSize * ix;
            zoneEnd = zoneSize * (ix + 1);

            // Remap val
            xx = ExtensionMethods.Remap(handlePosX, zoneStart, zoneEnd, xZone.Min, xZone.Max);
            xx = Mathf.Clamp(xx, xZone.Min, xZone.Max);

            // ==== YY ====
            // Get zone
            zoneSize = (Track.Height / YZones.Count);
            iy = Mathf.Clamp(Mathf.FloorToInt(handlePosY / zoneSize), 0, YZones.Count - 1);
            Zone yZone = XZones[iy];

            // Get Zone limits
            zoneStart = zoneSize * iy;
            zoneEnd = zoneSize * (iy + 1);

            // Remap val
            yy = ExtensionMethods.Remap(handlePosY, zoneStart, zoneEnd, yZone.Min, yZone.Max);
            yy = Mathf.Clamp(yy, yZone.Min, yZone.Max);

            zone_i = new Vector2Int(ix, iy);
            return new Vector2(xx, yy);
        }

        protected override Vector2 HandleToValue(float handleXPos, float handleYPos)
        {
            var val = HandleToValueAux(handleXPos, handleYPos, out Vector2Int zone_i);

            //register current zone
            ZoneIndex = zone_i;
            currentXZone = XZones[ZoneIndex.x];
            currentYZone = YZones[ZoneIndex.y];

            return val;
        }

        private Vector2Int GetZoneIndexFromValue(Vector2 value)
        {
            Zone xZone = null;
            Zone yZone = null;
            for (var i = 0; i < XZones.Count; i++)
            {
                xZone = XZones[i];
                for (var j = 0; j < YZones.Count; j++)
                {
                    yZone = YZones[j];
                    if (value.x >= xZone.Min && value.x <= xZone.Max && 
                        value.y >= yZone.Min && value.y <= yZone.Max  )
                    {
                        return new Vector2Int(i,j);
                    }
                }
            }
            return Vector2Int.zero;
        }

        protected override Vector2 ValueToHandle(Vector2 value)
        {
            float xx, yy;
            float zoneSize, zoneStart, zoneEnd;

            // Determine in which zones the value is in
            Vector2Int zone_i = GetZoneIndexFromValue(value);
            Zone xZone = XZones[zone_i.x];
            Zone yZone = YZones[zone_i.y];

            // Get zone limits
            // XX
            zoneSize = (Track.Width / XZones.Count);
            zoneStart = zoneSize * zone_i.x;
            zoneEnd   = zoneSize * (zone_i.x + 1);
            xx = ExtensionMethods.Remap(value.x, xZone.Min, xZone.Max, zoneStart, zoneEnd);
            xx = Mathf.Clamp(xx, zoneStart, zoneEnd);

            // YY
            zoneSize = (Track.Height / YZones.Count);
            zoneStart = zoneSize * zone_i.y;
            zoneEnd = zoneSize * (zone_i.y + 1);
            yy = ExtensionMethods.Remap(value.y, yZone.Min, yZone.Max, zoneStart, zoneEnd);
            yy = Mathf.Clamp(yy, zoneStart, zoneEnd);

            return new Vector2(xx, yy);
        }


        protected override void Update()
        {
            base.Update();

            // check zone change
            if (oldXZone != currentXZone || 
                oldYZone != currentYZone  )
            {
                ZoneChangedEvent.Invoke();
                if (selected) controller.SendHapticImpulse(0.4f, 0.02f);
            }
            oldXZone = currentXZone;
            oldYZone = currentYZone;
        }
    }
}
