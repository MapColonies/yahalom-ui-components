using UnityEditor;
using UnityEditor.UI;

namespace YahalomUIPackage.Editor.YahalomSliderToggle
{
    [CustomEditor(typeof(Runtime.YahalomSliderToggle.YahalomSliderToggle))]
    [CanEditMultipleObjects]
    public class YahalomSliderToggleEditor : SliderEditor
    {
        private SerializedProperty _animationDuration;
        private SerializedProperty _slideEase;
        private SerializedProperty _targetImage;
        private SerializedProperty _offColor;
        private SerializedProperty _onColor;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (target == null) return;

            _animationDuration = serializedObject.FindProperty("animationDuration");
            _slideEase = serializedObject.FindProperty("slideEase");
            _targetImage = serializedObject.FindProperty("targetImage");
            _offColor = serializedObject.FindProperty("offColor");
            _onColor = serializedObject.FindProperty("onColor");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_animationDuration);
            EditorGUILayout.PropertyField(_slideEase);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Toggle Colors", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_targetImage);
            EditorGUILayout.PropertyField(_offColor);
            EditorGUILayout.PropertyField(_onColor);
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
