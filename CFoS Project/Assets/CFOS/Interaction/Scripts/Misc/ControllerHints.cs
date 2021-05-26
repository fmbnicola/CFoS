using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CFoS.Interaction
{
    public class ControllerHints : MonoBehaviour
    {
        public MeshRenderer CotrollerRenderer;
        public Material ControllerMaterial;
        public Material SelectMaterial;
        public Material GrabMaterial;

        protected void SwapMaterial(int materialIndex, Material material)
        {
            List<Material> materials = new List<Material>();
            CotrollerRenderer.GetSharedMaterials(materials);
            materials[materialIndex] = material;

            CotrollerRenderer.sharedMaterials = materials.ToArray();
        }

        public void GrabHighlight(bool val)
        {
            if (val) SwapMaterial(6, GrabMaterial);
            else SwapMaterial(6, ControllerMaterial);
        }

        public void TriggerHighlight(bool val)
        {
            if(val) SwapMaterial(5, SelectMaterial);
            else SwapMaterial(5, ControllerMaterial);
        }

        public void JoystickHighlight(bool val)
        {
            if (val) SwapMaterial(4, SelectMaterial);
            else SwapMaterial(4, ControllerMaterial);
        }

    }
}


