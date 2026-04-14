using UnityEditor;

namespace Game.Configs
{
    public static class ValueSequenceValidator
    {
        public static void Validate(SerializedProperty arrayProp, string valueFieldName)
        {
            if (arrayProp.arraySize == 0)
                return;

            var first = arrayProp.GetArrayElementAtIndex(0);
            var firstValue = first.FindPropertyRelative(valueFieldName);

            if (firstValue.intValue == 0)
                firstValue.intValue = 2;

            for (int i = 1; i < arrayProp.arraySize; i++)
            {
                var prev = arrayProp.GetArrayElementAtIndex(i - 1);
                var current = arrayProp.GetArrayElementAtIndex(i);

                var prevValue = prev.FindPropertyRelative(valueFieldName);
                var currentValue = current.FindPropertyRelative(valueFieldName);

                currentValue.intValue = prevValue.intValue * 2;
            }
        }
    }
}