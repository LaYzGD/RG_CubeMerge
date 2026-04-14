using UnityEditor;

namespace Game.Configs
{
    [CustomEditor(typeof(SpawnConfig))]
    public class SpawnConfigEditor : Editor
    {
        private SerializedProperty _spawns;
        private int _lastSize;

        private void OnEnable()
        {
            _spawns = serializedObject.FindProperty("_spawnData");
            _lastSize = _spawns.arraySize;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_spawns, true);

            if (_spawns.arraySize != _lastSize)
            {
                ValueSequenceValidator.Validate(_spawns, "Value");
                _lastSize = _spawns.arraySize;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}