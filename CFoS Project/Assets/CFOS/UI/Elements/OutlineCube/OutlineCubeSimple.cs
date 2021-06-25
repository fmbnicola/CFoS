using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.UI
{
    public class OutlineCubeSimple : MonoBehaviour
    {
        [Space(10)]
        protected List<Shapes.Line> lines;

        protected MeshRenderer cube;
        protected MaterialPropertyBlock propBlock;

        private void Awake()
        {
            Init();
        }

        protected void Init()
        {
            //lines
            lines = new List<Shapes.Line>();

            var allLines = GetComponentsInChildren<Shapes.Line>();
            foreach (var line in allLines)
            {
                lines.Add(line);
            }

            //cube
            cube = GetComponent<MeshRenderer>();
            if (propBlock == null) propBlock = new MaterialPropertyBlock();
        }

        public void SetColor(Color col)
        {
            if(cube == null || lines == null) Init();

            if (propBlock == null) propBlock = new MaterialPropertyBlock();
            propBlock.SetColor("_HandleColor", col);
            cube.SetPropertyBlock(propBlock);
        }

        public void SetLinesColor(Color col)
        {
            if (lines == null) Init();
            
            foreach (var line in lines)
            {
                line.Color = col;
            }
        }

    }
}
