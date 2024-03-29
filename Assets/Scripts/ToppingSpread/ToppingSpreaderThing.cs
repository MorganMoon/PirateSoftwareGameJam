using PirateSoftwareGameJam.Client.ToppingSpread;
using UnityEngine;

namespace PirateSoftwareGameJam.Client
{
    public class ToppingSpreaderThing : MonoBehaviour
    {
        public ToppingSpreader toppingSpreader;
        [SerializeField]
        private Transform _lineOriginPoint;
        [SerializeField]
        private int _brushWidth;
        [SerializeField]
        private int _brushHeight;

        private IToppingSpreaderLine _toppingSpreaderLine;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _toppingSpreaderLine = toppingSpreader.StartLine((Vector2)_lineOriginPoint.position, _brushWidth, _brushHeight, transform.parent.rotation.eulerAngles.z);
            }
            
            if (Input.GetMouseButton(0))
            {
                if(_toppingSpreaderLine == null) { return; }
                _toppingSpreaderLine.SetRotation(transform.parent.rotation.eulerAngles.z);
                _toppingSpreaderLine.UpdatePosition((Vector2)_lineOriginPoint.position);
            }
        }
    }
}
