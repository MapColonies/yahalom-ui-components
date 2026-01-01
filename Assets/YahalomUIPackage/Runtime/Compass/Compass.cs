using TMPro;
using UnityEngine;

namespace YahalomUIPackage.Runtime.Compass
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private RectTransform _compassTransform;
        [SerializeField] private TMP_Text _angleText;
        [SerializeField] private TMP_Text _coordinatesText;

        [SerializeField] private float _startAngle;
        [SerializeField] private float _smoothTime = 0.1f;

        private Vector2 _lastDebugCoordinates;
        private float _currentVelocity;
        private float _smoothedAngle;
        private int _lastDisplayAngle = -1;

        private void UpdateRotation(float targetHeading)
        {
            float targetRotationZ = -targetHeading + _startAngle;
            _smoothedAngle = Mathf.SmoothDampAngle(_smoothedAngle, targetRotationZ, ref _currentVelocity, _smoothTime);
            _compassTransform.localEulerAngles = new Vector3(0, 0, _smoothedAngle);
        }

        private void SetCoordinatesText(string coordinatesInfo)
        {
            if (_coordinatesText != null)
            {
                _coordinatesText.text = coordinatesInfo;
            }
        }

        private void UpdateAngleText(float heading)
        {
            if (_angleText == null) return;

            float normalizedHeading = heading % 360;
            if (normalizedHeading < 0) normalizedHeading += 360;

            int currentDisplayAngle = Mathf.RoundToInt(normalizedHeading);

            if (currentDisplayAngle == _lastDisplayAngle)
            {
                return;
            }

            _lastDisplayAngle = currentDisplayAngle;
            _angleText.text = $"{currentDisplayAngle}°NE";
        }

        public void SetHeading(float heading)
        {
            UpdateRotation(heading);
            UpdateAngleText(heading);
        }

        public void SetCoordinates(Vector2 latLong)
        {
            SetCoordinatesText($"{latLong.y:F4}°N\\n{latLong.x:F4}°E");
        }
    }
}
