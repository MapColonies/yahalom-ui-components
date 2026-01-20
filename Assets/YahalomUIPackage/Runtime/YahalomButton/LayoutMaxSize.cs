using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.YahalomButton
{
    [ExecuteAlways]
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutMaxSize : MonoBehaviour
    {
        [SerializeField] private float _maxWidth = 200f;
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private LayoutElement _layoutElement;

        private const float Threshold = 0.1f;

        private void Update()
        {
            float contentWidth = _tmpText.preferredWidth;
            float targetWidth = Mathf.Min(contentWidth, _maxWidth);

            if (!(Mathf.Abs(_layoutElement.preferredWidth - targetWidth) > Threshold))
            {
                return;
            }

            _layoutElement.preferredWidth = targetWidth;

            if (transform.parent != null)
            {
                LayoutRebuilder.MarkLayoutForRebuild(transform.parent as RectTransform);
            }
        }
    }
}
