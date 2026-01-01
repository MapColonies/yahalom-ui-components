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

        private float _currentVelocity;
        private float _smoothedAngle;
        private int _lastDisplayAngle = -1;

        private const string North = "N";
        private const string East = "E";
        private const string Degree = "°";

        private void UpdateRotation(float targetHeading)
        {
            float targetRotationZ = -targetHeading + _startAngle;
            _smoothedAngle = Mathf.SmoothDampAngle(_smoothedAngle, targetRotationZ, ref _currentVelocity, _smoothTime);
            _compassTransform.localEulerAngles = new Vector3(0, 0, _smoothedAngle);
        }

        private void UpdateAngleText(float heading)
        {
            float normalizedHeading = heading % 360;
            if (normalizedHeading < 0) normalizedHeading += 360;

            int currentDisplayAngle = Mathf.RoundToInt(normalizedHeading);

            if (currentDisplayAngle == _lastDisplayAngle)
            {
                return;
            }

            _lastDisplayAngle = currentDisplayAngle;
            _angleText.text = $"{currentDisplayAngle}" + Degree + North + East;
        }

        public void SetHeading(float heading)
        {
            UpdateRotation(heading);
            UpdateAngleText(heading);
        }

        public void SetCoordinates(Vector2 latLong)
        {
            string coordinatesInfo = $"{latLong.y:F4}{Degree}{North}\\n{latLong.x:F4}{Degree}{East}";
            _coordinatesText.text = coordinatesInfo;
        }
    }
}
