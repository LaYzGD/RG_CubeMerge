using UnityEditor;

namespace Game.Configs
{
    [CustomEditor(typeof(EntityConfig))]
    public class EntityConfigEditor : Editor
    {
        private SerializedProperty database;
        private int lastSize;

        private void OnEnable()
        {
            database = serializedObject.FindProperty("_entityDatabase");
            lastSize = database.arraySize;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(database, true);

            if (database.arraySize != lastSize)
            {
                ValueSequenceValidator.Validate(database, "Value");
                lastSize = database.arraySize;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}