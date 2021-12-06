using DG.Tweening;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup.Animation
{
    public abstract class PopupAnimation : ScriptableObject
    {
        public abstract void AnimateShow(IPopupView view, TweenCallback onComplete);
        public abstract void AnimateHide(IPopupView view, TweenCallback onComplete);
    }
}