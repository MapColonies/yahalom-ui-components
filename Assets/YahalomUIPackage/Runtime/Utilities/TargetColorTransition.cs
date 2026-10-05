using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.Utilities
{
    public class TargetColorTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Graphic[] _targets;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _hoveredColor = Color.white;

        private void Start() => SetColor(_defaultColor);

        public void OnPointerEnter(PointerEventData eventData) => SetColor(_hoveredColor);
        public void OnPointerExit(PointerEventData eventData) => SetColor(_defaultColor);

        private void SetColor(Color color)
        {
            foreach (Graphic target in _targets)
            {
                if (target == null) return;
                target.color = color;
            }
        }
    }
}
