using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;

#endif
// NOT MINE. Taken from Unity forums because it's great.
//https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html

namespace Project.Core.Scene
{ 
    [System.Serializable]
    public class SceneField
    {
        [SerializeField] Object sceneAsset;
        [SerializeField] string sceneName = "";

        public string SceneName => sceneName;
        public bool Loaded => SceneManager.GetSceneByName(SceneName).isLoaded;
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            SerializedProperty  assetProperty = property.FindPropertyRelative("sceneAsset");
            SerializedProperty nameProperty = property.FindPropertyRelative("sceneName");
            SerializedProperty numberProperty = property.FindPropertyRelative("sceneNumber");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (assetProperty == null) return;
            EditorGUI.BeginChangeCheck();
 
            Object asset = EditorGUI.ObjectField(
                position, 
                assetProperty.objectReferenceValue, 
                typeof(SceneAsset), 
                false);
            
            if (!EditorGUI.EndChangeCheck()) return;
            assetProperty.objectReferenceValue = asset;
            if (assetProperty.objectReferenceValue == null) return;
            
            nameProperty.stringValue = (assetProperty.objectReferenceValue as SceneAsset)?.name;
        }
        
        // 0 1 2 3 4, therefore, returns 5
        // makes array:
        // 0 1 2 3 4 _
        // inserting at position 5 ==
        // 
    }
#endif
}