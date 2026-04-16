using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/AutoMergeConfig", fileName = "AutoMergeConfig")]
    public class AutoMergeConfig : ScriptableObject
    {
        [field: SerializeField] public float LiftHeight { get; private set; } = 1.5f;
        [field: SerializeField] public float LiftDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float SwingAmplitude { get; private set; } = 0.5f;
        [field: SerializeField] public float SwingDuration { get; private set; } = 0.2f;
        [field: SerializeField] public float MergeDuration { get; private set; } = 0.2f;
    }
}