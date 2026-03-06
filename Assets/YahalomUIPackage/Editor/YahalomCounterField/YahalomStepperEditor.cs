using UnityEditor;

namespace YahalomUIPackage.Editor.YahalomStepper
{
    [CustomEditor(typeof(Runtime.YahalomStepper.YahalomStepper))]
    [CanEditMultipleObjects]
    public class YahalomStepperEditor : UnityEditor.Editor
    {
        private SerializedProperty _value;
        private SerializedProperty _min;
        private SerializedProperty _max;
        private SerializedProperty _incrementButton;
        private SerializedProperty _decrementButton;
        private SerializedProperty _standardInputField;
        private SerializedProperty _rangeInputField;
        private SerializedProperty _maxLabel;
        private SerializedProperty _onValueChanged;

        private void OnEnable() {
            _value = serializedObject.FindProperty("_value");
            _min = serializedObject.FindProperty("_min");
            _max = serializedObject.FindProperty("_max");
            _incrementButton = serializedObject.FindProperty("_incrementButton");
            _decrementButton = serializedObject.FindProperty("_decrementButton");
            _standardInputField = serializedObject.FindProperty("_standardInputField");
            _rangeInputField = serializedObject.FindProperty("_rangeInputField");
            _maxLabel = serializedObject.FindProperty("_maxLabel");
            _onValueChanged = serializedObject.FindProperty("_onValueChanged");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.LabelField("Counter Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_value);
            EditorGUILayout.PropertyField(_min);
            EditorGUILayout.PropertyField(_max);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_incrementButton);
            EditorGUILayout.PropertyField(_decrementButton);
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Input Fields", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_standardInputField);
            EditorGUILayout.PropertyField(_rangeInputField);
            EditorGUILayout.PropertyField(_maxLabel);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_onValueChanged);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
