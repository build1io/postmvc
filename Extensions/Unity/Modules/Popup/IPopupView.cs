using Build1.PostMVC.Extensions.Unity.Mediation;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public interface IPopupView : IUnityView
    {
        GameObject GameObject  { get; }
        PopupBase  Popup       { get; }

        void SetUp(PopupBase popup);
        void Close();
    }
}