using UnityEditor;
using UnityEditor.UI;

namespace YahalomUIPackage.Editor.YahalomButton
{
    [CustomEditor(typeof(Runtime.YahalomButton.YahalomButton))]
    [CanEditMultipleObjects]
    public class YahalomButtonEditor : ButtonEditor
    {
        private SerializedProperty _textMesh;
        private SerializedProperty _icon;
        
        private SerializedProperty _normalIconColor;
        private SerializedProperty _highlightedIconColor;
        private SerializedProperty _pressedIconColor;
        private SerializedProperty _selectedIconColor;
        private SerializedProperty _disabledIconColor;

        private SerializedProperty _normalTextColor;
        private SerializedProperty _highlightedTextColor;
        private SerializedProperty _pressedTextColor;
        private SerializedProperty _selectedTextColor;
        private SerializedProperty _disabledTextColor;

        private SerializedProperty _fadeDuration;

        protected override void OnEnable()
        {
            base.OnEnable();

            _textMesh = serializedObject.FindProperty("_textMesh");
            _icon = serializedObject.FindProperty("_icon");

            _normalIconColor = serializedObject.FindProperty("_normalIconColor");
            _highlightedIconColor = serializedObject.FindProperty("_highlightedIconColor");
            _pressedIconColor = serializedObject.FindProperty("_pressedIconColor");
            _selectedIconColor = serializedObject.FindProperty("_selectedIconColor");
            _disabledIconColor = serializedObject.FindProperty("_disabledIconColor");

            _normalTextColor = serializedObject.FindProperty("_normalTextColor");
            _highlightedTextColor = serializedObject.FindProperty("_highlightedTextColor");
            _pressedTextColor = serializedObject.FindProperty("_pressedTextColor");
            _selectedTextColor = serializedObject.FindProperty("_selectedTextColor");
            _disabledTextColor = serializedObject.FindProperty("_disabledTextColor");

            _fadeDuration = serializedObject.FindProperty("_fadeDuration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Main Button Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_textMesh);
            EditorGUILayout.PropertyField(_icon);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("State Icons", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_normalIconColor);
            EditorGUILayout.PropertyField(_highlightedIconColor);
            EditorGUILayout.PropertyField(_pressedIconColor);
            EditorGUILayout.PropertyField(_selectedIconColor);
            EditorGUILayout.PropertyField(_disabledIconColor);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Text Colors", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_normalTextColor);
            EditorGUILayout.PropertyField(_highlightedTextColor);
            EditorGUILayout.PropertyField(_pressedTextColor);
            EditorGUILayout.PropertyField(_selectedTextColor);
            EditorGUILayout.PropertyField(_disabledTextColor);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_fadeDuration);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
