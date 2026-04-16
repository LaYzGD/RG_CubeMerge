using Game.Features.Score;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Systems.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreTMP;

        private IScoreService _scoreService;

        [Inject]
        public void Construct(IScoreService scoreService)
        {
            _scoreService = scoreService;
            _scoreTMP.text = "0";
            _scoreService.OnScoreChanged += ChangeScoreUI;
        }

        private void ChangeScoreUI(int score)
        {
            _scoreTMP.text = $"{score}";
        }

        private void OnDestroy()
        {
            _scoreService.OnScoreChanged -= ChangeScoreUI;
        }
    }
}