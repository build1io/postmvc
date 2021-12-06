using Build1.PostMVC.Extensions.Unity.Mediation;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public interface IPopupView : IUnityView
    {
        PopupBase  Popup      { get; }
        GameObject GameObject { get; }

        GameObject    Overlay { get; }
        RectTransform Content { get; }

        void SetUp(PopupBase popup);
        void Close();
    }
}