using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Utils.UI
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        [Header("Applicable Offsets")]
        [Tooltip("% of screen height that'll be added to top padding when save area applied.")]
        [SerializeField]
        private float applicableOffsetTopPercentage;

        [Tooltip("Offset that'll be added to top offset when save area applied.")]
        [SerializeField]
        private int applicableOffsetTopPixels;
        
        [Tooltip("% of screen height that'll be added to bottom padding when save area applied.")]
        [SerializeField]
        private float applicableOffsetBottomPercentage;

        [Tooltip("Offset that'll be added to bottom offset when save area applied.")]
        [SerializeField] 
        private int applicableOffsetBottomPixels;
        
        [Header("Unapplicable Offsets")]
        [Tooltip("% of screen height that'll be added to top padding when save area NOT applied.")]
        [SerializeField]
        private float unapplicableOffsetTopPercentage;
        
        [Tooltip("Offset that'll be added to top offset when save area NOT applied.")]
        [SerializeField]
        private int unapplicableOffsetTopPixels;
        
        [Tooltip("% of screen height that'll be added to bottom padding when save area NOT applied.")]
        [SerializeField]
        private float unapplicableOffsetBottomPercentage;
        
        [Tooltip("Offset that'll be added to bottom offset when save area NOT applied.")]
        [SerializeField] 
        private int unapplicableOffsetBottomPixels;
        
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();

            ApplySafeArea();
        }

        #if UNITY_EDITOR

        private Rect  _lastSafeArea;
        
        private float _appliedOffsetTopPercentage;
        private int   _appliedOffsetTopPixels;
        private float _appliedOffsetBottomPercentage;
        private int   _appliedOffsetBottomPixels;
        
        private float _offsetTopPercentage;
        private int   _offsetTopPixels;
        private float _offsetBottomPercentage;
        private int   _offsetBottomPixels;

        private void Update()
        {
            if (_lastSafeArea == Screen.safeArea &&
                _appliedOffsetTopPercentage == applicableOffsetTopPercentage &&
                _appliedOffsetTopPixels == applicableOffsetTopPixels &&
                _appliedOffsetBottomPercentage == applicableOffsetBottomPercentage &&
                _appliedOffsetBottomPixels == applicableOffsetBottomPixels &&
                _offsetTopPercentage == unapplicableOffsetTopPercentage && 
                _offsetTopPixels == unapplicableOffsetTopPixels && 
                _offsetBottomPercentage == unapplicableOffsetBottomPercentage && 
                _offsetBottomPixels == unapplicableOffsetBottomPixels)
                return;

            ApplySafeArea();
            
            _lastSafeArea = Screen.safeArea;
            
            _appliedOffsetTopPercentage = applicableOffsetTopPercentage;
            _appliedOffsetTopPixels = applicableOffsetTopPixels;
            _appliedOffsetBottomPercentage = applicableOffsetBottomPercentage;
            _appliedOffsetBottomPixels = applicableOffsetBottomPixels;

            _offsetTopPercentage = unapplicableOffsetTopPercentage;
            _offsetTopPixels = unapplicableOffsetTopPixels;
            _offsetBottomPercentage = unapplicableOffsetBottomPercentage;
            _offsetBottomPixels = unapplicableOffsetBottomPixels;
        }

        #endif

        private void ApplySafeArea()
        {
            var safeAreaRect = Screen.safeArea;
            
            var left = safeAreaRect.xMin;
            var right = -(Screen.width - safeAreaRect.xMax);

            var top = safeAreaRect.height + safeAreaRect.y - Screen.height;
            if (top != 0)
                top -= safeAreaRect.height * applicableOffsetTopPercentage + applicableOffsetTopPixels;
            else
                top -= safeAreaRect.height * unapplicableOffsetTopPercentage + unapplicableOffsetTopPixels;

            var bottom = safeAreaRect.y;
            if (bottom != 0)
                bottom += safeAreaRect.height * applicableOffsetBottomPercentage + applicableOffsetBottomPixels;
            else
                bottom = safeAreaRect.height * unapplicableOffsetBottomPercentage + unapplicableOffsetBottomPixels;
                         
            _rectTransform.offsetMin = new Vector2(left, bottom);
            _rectTransform.offsetMax = new Vector2(right, top);
        }
    }
}