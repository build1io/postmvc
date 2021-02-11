using System;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Utils.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        [Header("Applicable Params")]
        [SerializeField] private Rect applicableOffset;
        
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;
        
        private Rect _lastSafeArea;
        
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentRectTransform = GetComponentInParent<RectTransform>();
            ApplySafeArea();
        }

        private void Update()
        {
            if (_lastSafeArea != Screen.safeArea)
                ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            var safeAreaRect = Screen.safeArea;

            if (Math.Abs(safeAreaRect.width - Screen.width) > 0.001 || Math.Abs(safeAreaRect.height - Screen.height) > 0.001)
            {
                var scaleRatio = _parentRectTransform.rect.width / Screen.width;

                var left = safeAreaRect.xMin * scaleRatio + safeAreaRect.width * applicableOffset.x;
                var right = -(Screen.width - safeAreaRect.xMax) * scaleRatio - safeAreaRect.width * applicableOffset.width;
                var top = -safeAreaRect.yMin * scaleRatio - safeAreaRect.height * applicableOffset.y;
                var bottom = (Screen.height - safeAreaRect.yMax) * scaleRatio + safeAreaRect.height * applicableOffset.height;

                _rectTransform.offsetMin = new Vector2(left, bottom);
                _rectTransform.offsetMax = new Vector2(right, top);
            }

            _lastSafeArea = Screen.safeArea;
        }
    }
}