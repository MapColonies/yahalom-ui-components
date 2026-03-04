using UnityEditor;
using UnityEditor.UI;

namespace YahalomUIPackage.Editor.YahalomCheckbox
{
    [CustomEditor(typeof(YahalomUIPackage.Runtime.YahalomCheckbox.YahalomCheckbox))]
    [CanEditMultipleObjects]
    public class YahalomCheckboxEditor : ToggleEditor
    {
        private SerializedProperty _checkedSprite;
        private SerializedProperty _uncheckedSprite;
        private SerializedProperty _disabledSprite;
        private SerializedProperty _partialSprite;
        private SerializedProperty childToggles;

        protected override void OnEnable()
        {
            base.OnEnable();

            _checkedSprite = serializedObject.FindProperty("_checkedSprite");
            _uncheckedSprite = serializedObject.FindProperty("_uncheckedSprite");
            _disabledSprite = serializedObject.FindProperty("_disabledSprite");
            _partialSprite = serializedObject.FindProperty("_partialSprite");
            childToggles = serializedObject.FindProperty("childToggles");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Yahalom Checkbox", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_checkedSprite);
            EditorGUILayout.PropertyField(_uncheckedSprite);
            EditorGUILayout.PropertyField(_disabledSprite);
            EditorGUILayout.PropertyField(_partialSprite);
            EditorGUILayout.PropertyField(childToggles);

            serializedObject.ApplyModifiedProperties();
        }
    }
}