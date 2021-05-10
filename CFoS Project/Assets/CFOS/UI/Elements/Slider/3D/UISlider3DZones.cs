using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace CFoS.UI
{
    public class UISlider3DZones : UISlider3D
    {
        [Space(10)]
        public UnityEvent ZoneChangedEvent;

        [SerializeField]
        public List<Zone> XZones;
        public List<Zone> YZones;
        public List<Zone> ZZones;

        private Zone oldXZone;
        private Zone currentXZone;
        private Zone oldYZone;
        private Zone currentYZone;
        private Zone oldZZone;
        private Zone currentZZone;
        public Vector3Int ZoneIndex { get; set; }

        [ExecuteAlways]
        protected override void OnValidate()
        {
            base.OnValidate();

            if (XZones == null || XZones.Count == 0)
            {
                XZones = new List<Zone>() { new Zone(XMin, XMax) };
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


            if (ZZones == null || ZZones.Count == 0)
            {
                ZZones = new List<Zone>() { new Zone(ZMin, ZMax) };
            }
            else
            {
                ZZones[0].Min = ZMin;
                ZZones[YZones.Count - 1].Max = ZMax;
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

            currentZZone = ZZones[ZoneIndex.z];
            oldZZone = currentZZone;
        }

        public override Vector3 WorldCoordsToValue(Vector3 coords)
        {
            Vector3 localPos = transform.InverseTransformPoint(coords);
            return HandleToValueAux(localPos.x, localPos.y, localPos.z, out Vector3Int zone_i);
        }


        private Vector3 HandleToValueAux(float handlePosX, float handlePosY, float handlePosZ, out Vector3Int zone_i)
        {
            float zoneSize, zoneStart, zoneEnd;
            int ix, iy, iz;
            float xx, yy, zz;

            // ==== XX ====
            // Get zone
            zoneSize = (Track.localScale.x / XZones.Count);
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
            zoneSize = (Track.localScale.y / YZones.Count);
            iy = Mathf.Clamp(Mathf.FloorToInt(handlePosY / zoneSize), 0, YZones.Count - 1);
            Zone yZone = XZones[iy];

            // Get Zone limits
            zoneStart = zoneSize * iy;
            zoneEnd = zoneSize * (iy + 1);

            // Remap val
            yy = ExtensionMethods.Remap(handlePosY, zoneStart, zoneEnd, yZone.Min, yZone.Max);
            yy = Mathf.Clamp(yy, yZone.Min, yZone.Max);

            // ==== ZZ ====
            // Get zone
            zoneSize = (Track.localScale.z / ZZones.Count);
            iz = Mathf.Clamp(Mathf.FloorToInt(handlePosZ / zoneSize), 0, ZZones.Count - 1);
            Zone zZone = ZZones[iz];

            // Get Zone limits
            zoneStart = zoneSize * iz;
            zoneEnd = zoneSize * (iz + 1);

            // Remap val
            zz = ExtensionMethods.Remap(handlePosZ, zoneStart, zoneEnd, zZone.Min, zZone.Max);
            zz = Mathf.Clamp(zz, zZone.Min, zZone.Max);

            zone_i = new Vector3Int(ix, iy, iz);
            return new Vector3(xx, yy, zz);
        }

        protected override Vector3 HandleToValue(float handleXPos, float handleYPos, float handleZPos)
        {
            var val = HandleToValueAux(handleXPos, handleYPos, handleZPos, out Vector3Int zone_i);

            //register current zone
            ZoneIndex = zone_i;
            currentXZone = XZones[ZoneIndex.x];
            currentYZone = YZones[ZoneIndex.y];
            currentZZone = ZZones[ZoneIndex.z];

            return val;
        }

        private Vector3Int GetZoneIndexFromValue(Vector3 value)
        {
            Zone xZone = null;
            Zone yZone = null;
            Zone zZone = null;
            for (var i = 0; i < XZones.Count; i++)
            {
                xZone = XZones[i];
                for (var j = 0; j < YZones.Count; j++)
                {
                    yZone = YZones[j];
                    for (var k = 0; k < ZZones.Count; k++)
                    {
                        zZone = ZZones[k];
                        if (value.x >= xZone.Min && value.x <= xZone.Max &&
                            value.y >= yZone.Min && value.y <= yZone.Max &&
                            value.z >= zZone.Min && value.z <= zZone.Max)
                        {
                            return new Vector3Int(i, j, k);
                        }
                    }
                }
            }
            return Vector3Int.zero;
        }

        protected override Vector3 ValueToHandle(Vector3 value)
        {
            float xx, yy, zz;
            float zoneSize, zoneStart, zoneEnd;

            // Determine in which zones the value is in
            Vector3Int zone_i = GetZoneIndexFromValue(value);
            Zone xZone = XZones[zone_i.x];
            Zone yZone = YZones[zone_i.y];
            Zone zZone = ZZones[zone_i.z];

            // Get zone limits
            // XX
            zoneSize = (Track.localScale.x / XZones.Count);
            zoneStart = zoneSize * zone_i.x;
            zoneEnd = zoneSize * (zone_i.x + 1);
            xx = ExtensionMethods.Remap(value.x, xZone.Min, xZone.Max, zoneStart, zoneEnd);
            xx = Mathf.Clamp(xx, zoneStart, zoneEnd);

            // YY
            zoneSize = (Track.localScale.y / YZones.Count);
            zoneStart = zoneSize * zone_i.y;
            zoneEnd = zoneSize * (zone_i.y + 1);
            yy = ExtensionMethods.Remap(value.y, yZone.Min, yZone.Max, zoneStart, zoneEnd);
            yy = Mathf.Clamp(yy, zoneStart, zoneEnd);

            // ZZ
            zoneSize = (Track.localScale.z / ZZones.Count);
            zoneStart = zoneSize * zone_i.z;
            zoneEnd = zoneSize * (zone_i.z + 1);
            zz = ExtensionMethods.Remap(value.z, zZone.Min, zZone.Max, zoneStart, zoneEnd);
            zz = Mathf.Clamp(zz, zoneStart, zoneEnd);

            return new Vector3(xx, yy, zz);
        }


        protected override void Update()
        {
            base.Update();

            // check zone change
            if (oldXZone != currentXZone ||
                oldYZone != currentYZone ||
                oldZZone != currentZZone )
            {
                ZoneChangedEvent.Invoke();
                if (selected) controller.SendHapticImpulse(0.4f, 0.02f);
            }
            oldXZone = currentXZone;
            oldYZone = currentYZone;
            oldZZone = currentZZone;
        }
    }
}