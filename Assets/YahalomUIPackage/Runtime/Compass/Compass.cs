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
        private string _lastDirection = "";

        private const string Degree = "°";
        private const float SectorSize = 45f;

        private const int DirN = 0;
        private const int DirE = 2;
        private const int DirS = 4;
        private const int DirW = 6;

        private static readonly string[] Directions8 =
        {
            "N", "NE", "E", "SE", "S", "SW", "W", "NW"
        };

        private void UpdateRotation(float targetHeading)
        {
            float targetRotationZ = -targetHeading + _startAngle;
            _smoothedAngle = Mathf.SmoothDampAngle(_smoothedAngle, targetRotationZ, ref _currentVelocity, _smoothTime);
            _compassTransform.localEulerAngles = new Vector3(0, 0, _smoothedAngle);
        }

        private void UpdateAngleText(float heading)
        {
            float normalizedHeading = heading % 360f;
            if (normalizedHeading < 0f) normalizedHeading += 360f;

            int currentDisplayAngle = Mathf.RoundToInt(normalizedHeading);
            string direction = GetDirection8FromNormalized(normalizedHeading);

            if (currentDisplayAngle == _lastDisplayAngle && direction == _lastDirection)
                return;

            _lastDisplayAngle = currentDisplayAngle;
            _lastDirection = direction;

            _angleText.text = $"{currentDisplayAngle}{Degree}{direction}";
        }

        private static string GetDirection8FromNormalized(float heading0To360)
        {
            int index = Mathf.FloorToInt((heading0To360 + SectorSize / 2f) / SectorSize) % 8;
            return Directions8[index];
        }

        public void SetHeading(float heading)
        {
            UpdateRotation(heading);
            UpdateAngleText(heading);
        }

        public void SetCoordinates(Vector2 latLong)
        {
            float lat = latLong.y;
            float lon = latLong.x;

            string latHemisphere = lat >= 0f ? Directions8[DirN] : Directions8[DirS];
            string lonHemisphere = lon >= 0f ? Directions8[DirE] : Directions8[DirW];

            _coordinatesText.text =
                $"{Mathf.Abs(lat):F4}{Degree}{latHemisphere}\n" +
                $"{Mathf.Abs(lon):F4}{Degree}{lonHemisphere}";
        }
    }
}
