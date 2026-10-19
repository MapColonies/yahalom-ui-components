using TMPro;
using UnityEngine;

namespace YahalomUIPackage.Runtime.Compass
{
    public class SpeedViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _speedTitle;
        [SerializeField] private TextMeshProUGUI _speedUnits;

        private int? _lastSpeed;

        public void Init(string title, string units)
        {
            _speedTitle.text = title;
            SetUnits(units);
        }

        public void SetUnits(string units)
        {
            if (_speedUnits.text != units)
                _speedUnits.text = units;
        }

        public void SetSpeed(int speed)
        {
            if (_lastSpeed == speed) return;

            _lastSpeed = speed;
            _speedText.text = speed.ToString();
        }
    }
}
