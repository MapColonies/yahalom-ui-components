using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.YahalomButton
{
    [ExecuteAlways]
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutMaxSize : MonoBehaviour
    {
        public float maxWidth = 200f;

        private TMP_Text _tmpText;
        private LayoutElement _layoutElement;

        private void OnEnable()
        {
            _tmpText = GetComponent<TMP_Text>();
            _layoutElement = GetComponent<LayoutElement>();
        }

        private void Update()
        {
            if (_tmpText == null || _layoutElement == null) return;

            float contentWidth = _tmpText.preferredWidth;
            float targetWidth = Mathf.Min(contentWidth, maxWidth);

            if (!(Mathf.Abs(_layoutElement.preferredWidth - targetWidth) > 0.1f))
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
