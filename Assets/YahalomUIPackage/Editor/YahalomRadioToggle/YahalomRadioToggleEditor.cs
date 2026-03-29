using UnityEditor;
using UnityEditor.UI;

namespace YahalomUIPackage.Editor.YahalomRadioToggle {

[CustomEditor(typeof(YahalomUIPackage.Runtime.YahalomRadioToggle.YahalomRadioToggle))]
[CanEditMultipleObjects]
public class YahalomRadioToggleEditor : ToggleEditor {
    private SerializedProperty _checkedSprite;
    private SerializedProperty _uncheckedSprite;
    private SerializedProperty _disabledCheckedSprite;
    private SerializedProperty _disabledUncheckedSprite;
    private SerializedProperty _toggleImage;

    protected override void OnEnable() {
        if (target == null) {
            return;
        }

        base.OnEnable();

        _checkedSprite = serializedObject.FindProperty("_checkedSprite");
        _uncheckedSprite = serializedObject.FindProperty("_uncheckedSprite");
        _disabledCheckedSprite = serializedObject.FindProperty("_disabledCheckedSprite");
        _disabledUncheckedSprite = serializedObject.FindProperty("_disabledUncheckedSprite");
        _toggleImage = serializedObject.FindProperty("_toggleImage");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(_toggleImage);
        EditorGUILayout.PropertyField(_checkedSprite);
        EditorGUILayout.PropertyField(_uncheckedSprite);
        EditorGUILayout.PropertyField(_disabledCheckedSprite);
        EditorGUILayout.PropertyField(_disabledUncheckedSprite);

        serializedObject.ApplyModifiedProperties();
    }
}

}
