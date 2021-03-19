using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Build1.PostMVC.Extensions.Unity.Components
{
    public sealed class FullScreenButton : Button
    {
        public readonly UnityEvent onFullScreen = new UnityEvent();
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            // Due to WebGL specifics we need to trigger full screen on pointer down event.
            onFullScreen.Invoke();
        }
    }
}