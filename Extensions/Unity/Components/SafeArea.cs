using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Components
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        [Header("General"), SerializeField] private Vector2Int referenceResolution;
        
        [Header("Applicable Offsets"), SerializeField] private Rect    applicableOffsetPercentage;
        [SerializeField]                               private RectInt applicableOffsetPixels;

        [Header("Unapplicable Offsets"), SerializeField] private Rect    unapplicableOffsetPercentage;
        [SerializeField]                                 private RectInt unapplicableOffsetPixels;
        
        private void Start()
        {
            ApplySafeArea();
        }

        #if UNITY_EDITOR

        private Rect       _lastSafeArea;
        private Vector2Int _referenceResolution;
        
        private Rect    _applicableOffsetPercentage;
        private RectInt _applicableOffsetPixels;
        
        private Rect    _unapplicableOffsetPercentage;
        private RectInt _unapplicableOffsetPixels;

        private void Update()
        {
            if (_lastSafeArea == Screen.safeArea &&
                _referenceResolution == referenceResolution &&
                _applicableOffsetPercentage == applicableOffsetPercentage &&
                _applicableOffsetPixels.Equals(applicableOffsetPixels)  &&
                _unapplicableOffsetPercentage == unapplicableOffsetPercentage &&
                _unapplicableOffsetPixels.Equals(unapplicableOffsetPixels))
                return;

            ApplySafeArea();

            _lastSafeArea = Screen.safeArea;
            _referenceResolution = referenceResolution;

            _applicableOffsetPercentage = applicableOffsetPercentage;
            _applicableOffsetPixels = applicableOffsetPixels;

            _unapplicableOffsetPercentage = unapplicableOffsetPercentage;
            _unapplicableOffsetPixels = unapplicableOffsetPixels;
        }

        #endif

        private void ApplySafeArea()
        {
            var safeArea = Screen.safeArea;
            var screenResolution = Screen.currentResolution;

            var scaleHeight = (float)referenceResolution.y / screenResolution.height;
            var scaleWidth = (float)referenceResolution.x / screenResolution.width;
            
            var topPixel = screenResolution.height - (safeArea.y + safeArea.height);
            var bottomPixels = safeArea.y;
            var leftPixels = safeArea.xMin;
            var rightPixels = -(screenResolution.width - safeArea.xMax);
            
            var topUnits = topPixel * scaleHeight * 1.2f;
            if (topPixel != 0)
                topUnits += applicableOffsetPixels.y + screenResolution.height * applicableOffsetPercentage.y;
            else
                topUnits += unapplicableOffsetPixels.y + screenResolution.height * unapplicableOffsetPercentage.y;
            
            var bottomUnits = bottomPixels * scaleHeight * 1.2F;
            if (bottomPixels != 0)
                bottomUnits += applicableOffsetPixels.height + screenResolution.height * applicableOffsetPercentage.height;
            else
                bottomUnits += unapplicableOffsetPixels.height + screenResolution.height * unapplicableOffsetPercentage.height;
            
            var leftUnits = leftPixels * scaleWidth;
            if (leftPixels != 0)
                leftUnits += applicableOffsetPixels.x + screenResolution.width * applicableOffsetPercentage.x;
            else
                leftUnits += unapplicableOffsetPixels.x + screenResolution.width * unapplicableOffsetPercentage.x;
            
            var rightUnits = rightPixels * scaleWidth;
            if (rightPixels != 0)
                rightUnits -= applicableOffsetPixels.width + screenResolution.width * applicableOffsetPercentage.width;
            else
                rightUnits -= unapplicableOffsetPixels.width + screenResolution.width * unapplicableOffsetPercentage.width;;
            
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(leftUnits, bottomUnits);
            rectTransform.offsetMax = new Vector2(rightUnits, -topUnits);
        }
    }
}