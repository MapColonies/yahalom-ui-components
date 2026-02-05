using TMPro.EditorUtilities;
using UnityEditor;

namespace YahalomUIPackage.Editor.YahalomInputField
{
    [CustomEditor(typeof(Runtime.YahalomInputField.YahalomInputField))]
    [CanEditMultipleObjects]
    public class YahalomInputFieldEditor : TMP_InputFieldEditor
    {
        private SerializedProperty _closeButton;
        private SerializedProperty _normalTextColor;
        private SerializedProperty _highlightedTextColor;
        private SerializedProperty _pressedTextColor;
        private SerializedProperty _selectedTextColor;
        private SerializedProperty _disabledTextColor;

        private SerializedProperty _errorTextColor;
        private SerializedProperty _errorImageColor;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _closeButton = serializedObject.FindProperty("_closeButton");
            _normalTextColor = serializedObject.FindProperty("_normalTextColor");
            _highlightedTextColor = serializedObject.FindProperty("_highlightedTextColor");
            _pressedTextColor = serializedObject.FindProperty("_pressedTextColor");
            _selectedTextColor = serializedObject.FindProperty("_selectedTextColor");
            _disabledTextColor = serializedObject.FindProperty("_disabledTextColor");
            _errorTextColor = serializedObject.FindProperty("_errorTextColor");
            _errorImageColor = serializedObject.FindProperty("_errorImageColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(_closeButton);
            EditorGUILayout.LabelField("Image Colors", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_errorImageColor);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Text Colors", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_normalTextColor);
            EditorGUILayout.PropertyField(_highlightedTextColor);
            EditorGUILayout.PropertyField(_pressedTextColor);
            EditorGUILayout.PropertyField(_selectedTextColor);
            EditorGUILayout.PropertyField(_disabledTextColor);
            EditorGUILayout.PropertyField(_errorTextColor);
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
