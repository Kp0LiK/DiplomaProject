#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;


    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var sceneObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

                if (ReferenceEquals(sceneObject, null) && !string.IsNullOrWhiteSpace(property.stringValue))
                    sceneObject = GetBuildSettingsSceneObject(property.stringValue);

                if (ReferenceEquals(sceneObject, null) && !string.IsNullOrWhiteSpace(property.stringValue))
                    Debug.LogError(
                        $"Could not find scene {property.stringValue}");

                var scene =
                    (SceneAsset) EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);

                property.stringValue = AssetDatabase.GetAssetPath(scene);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
            }
        }

        private SceneAsset GetBuildSettingsSceneObject(string sceneName)
        {
            return EditorBuildSettings.scenes
                .Select(buildScene => AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path))
                .FirstOrDefault(sceneAsset => !ReferenceEquals(sceneAsset, null) && sceneAsset.name == sceneName);
        }
    }

#endif