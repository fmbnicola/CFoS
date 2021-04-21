using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI.Menus
{
    public class UICadget1D2 : UIMenu
    {
        [Header("Thumbnails")]
        public CFoS.Supershape.Supershape2DRenderer Thumbnail1;
        public CFoS.Supershape.Supershape2DRenderer Thumbnail2;
        public CFoS.Supershape.Supershape2DRenderer Thumbnail3;
        public CFoS.Supershape.Supershape2DRenderer Thumbnail4;
        public CFoS.Supershape.Supershape2DRenderer Thumbnail5;
        public CFoS.Supershape.Supershape2DRenderer Thumbnail6;
        public CFoS.Supershape.Supershape2DRenderer Thumbnail7;

        [Header("Zones")]
        public TMPro.TextMeshPro Starness;
        public TMPro.TextMeshPro Roundness;
        public TMPro.TextMeshPro Squareness;
        [Space(10)]
        public TMPro.TextMeshPro Concave;
        public TMPro.TextMeshPro Convex;
        [Space(10)]
        public TMPro.TextMeshPro Inscribed;
        public TMPro.TextMeshPro Circumscribed;
    }
}