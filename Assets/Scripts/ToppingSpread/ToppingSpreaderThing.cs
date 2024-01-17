using PirateSoftwareGameJam.Client.ToppingSpread;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PirateSoftwareGameJam.Client
{
    public class ToppingSpreaderThing : MonoBehaviour
    {
        public ToppingSpreader toppingSpreader;

        private IToppingSpreaderLine _toppingSpreaderLine;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _toppingSpreaderLine = toppingSpreader.StartLine((Vector2)transform.position, 5, 5);
            }
            
            if (Input.GetMouseButton(0))
            {
                if(_toppingSpreaderLine == null) { return; }
                _toppingSpreaderLine.UpdatePosition((Vector2)transform.position);
            }
        }
    }
}
