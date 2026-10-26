using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace YahalomUIPackage.Runtime.YahalomSliderToggle
{
    public class YahalomSliderToggle : Slider, IPointerClickHandler
    {
        public bool CurrentValue { get; private set; }

        [Header("Animation")]
        [SerializeField, Range(0, .25f)] private float animationDuration = 0.035f;
        [SerializeField]
        private AnimationCurve slideEase =
            AnimationCurve.EaseInOut(0, 0, 1, 1);

        private CancellationTokenSource _animationCts;

        [Header("Colors")]
        [SerializeField] private Image targetImage;
        [SerializeField] private Color offColor = new Color(0x66 / 255f, 0x66 / 255f, 0x66 / 255f);
        [SerializeField] private Color onColor  = new Color(0x16 / 255f, 0xC0 / 255f, 0xA2 / 255f);

        public Action<bool> onToggleValueChanged;

        protected Action transitionEffect;

        protected void OnValidate() {
            ConfigureSlider();
        }

        private void ConfigureSlider() {
            interactable = false;
            var sliderColors = colors;
            sliderColors.disabledColor = Color.white;
            colors = sliderColors;
            transition = Transition.None;
        }

        protected override void Awake() {
            base.Awake();
            ConfigureSlider();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            _animationCts?.Cancel();
            _animationCts?.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData) {
            Toggle();
        }

        private void Toggle() {
            SetStateAndStartAnimation(!CurrentValue);
        }

        public void ToggleByGroupManager(bool valueToSetTo) {
            SetStateAndStartAnimation(valueToSetTo);
        }

        private void SetStateAndStartAnimation(bool state) {
            if (CurrentValue != state) {
                onToggleValueChanged?.Invoke(state);
            }

            CurrentValue = state;

            _animationCts?.Cancel();
            _animationCts?.Dispose();
            _animationCts = new CancellationTokenSource();
            AnimateSlider(_animationCts.Token).Forget();
        }

        private async UniTaskVoid AnimateSlider(CancellationToken cancellationToken) {
            float startValue = value;
            float endValue = CurrentValue ? 1 : 0;
            Color startColor = targetImage != null ? targetImage.color : offColor;
            Color endColor = CurrentValue ? onColor : offColor;

            float time = 0;
            if (animationDuration > 0) {
                while (time < animationDuration) {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    time += Time.deltaTime;

                    float lerpFactor = slideEase.Evaluate(time / animationDuration);
                    value = Mathf.Lerp(startValue, endValue, lerpFactor);

                    if (targetImage != null)
                        targetImage.color = Color.Lerp(startColor, endColor, lerpFactor);

                    transitionEffect?.Invoke();

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }
            }

            if (!cancellationToken.IsCancellationRequested) {
                value = endValue;
                if (targetImage != null)
                    targetImage.color = endColor;
            }
        }
    }
}
