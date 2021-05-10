using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CFoS.UI
{
    public class OutlineCube : MonoBehaviour
    {
        [Space(10)][SerializeField]
        public List<Shapes.Line> Front;
        public List<Shapes.Line> Back;
        public List<Shapes.Line> Left;
        public List<Shapes.Line> Right;
        public List<Shapes.Line> Top;
        public List<Shapes.Line> Bottom;

        private readonly float val = 0.2f;

        protected void SetLinesFront(List<Shapes.Line> lines)
        {
            foreach (var line in lines)
            {
                line.Dashed = false;
            }
        }

        protected void SetLinesBack(List<Shapes.Line> lines)
        {
            foreach(var line in lines)
            {
                line.Dashed = true;
            }
        }

        protected void SetAllBackLines()
        {
            SetLinesBack(Front);
            SetLinesBack(Back);
            SetLinesBack(Left);
            SetLinesBack(Right);
            SetLinesBack(Top);
            SetLinesBack(Bottom);
        }

        private void Update()
        {
            var cameraPos = Camera.main.transform.position;
            var lookAt    = cameraPos - transform.position;

            SetAllBackLines();

            // front
            if (Vector3.Dot(-transform.forward, lookAt) > val)
            {
                SetLinesFront(Front);
            }

            // back
            if (Vector3.Dot(transform.forward, lookAt) > val)
            {
                SetLinesFront(Back);
            }

            // left
            if (Vector3.Dot(-transform.right, lookAt) > val)
            {
                SetLinesFront(Left);
            }

            // right
            if (Vector3.Dot(transform.right, lookAt) > val)
            {
                SetLinesFront(Right);
            }

            // top
            if (Vector3.Dot(transform.up, lookAt) > val)
            {
                SetLinesFront(Top);
            }

            // bottom
            if (Vector3.Dot(-transform.up, lookAt) > val)
            {
                SetLinesFront(Bottom);
            }
        }
    }
}
