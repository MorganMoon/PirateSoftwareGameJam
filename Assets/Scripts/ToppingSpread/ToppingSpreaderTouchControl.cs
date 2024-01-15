using UnityEngine;
using UnityEngine.EventSystems;

namespace PirateSoftwareGameJam.Client.ToppingSpread
{
    [RequireComponent(typeof(ToppingSpreader))]
    public class ToppingSpreaderTouchControl : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        private ToppingSpreader _toppingSpreader;
        private IToppingSpreaderLine _toppingSpreaderLine;

        private void Awake()
        {
            _toppingSpreader = GetComponent<ToppingSpreader>();    
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _toppingSpreaderLine = _toppingSpreader.StartLine(eventData.pointerCurrentRaycast.worldPosition, 5, 20);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_toppingSpreaderLine == null)
            {
                return;
            }

            _toppingSpreaderLine.UpdatePosition(eventData.pointerCurrentRaycast.worldPosition);
        }
    }
}