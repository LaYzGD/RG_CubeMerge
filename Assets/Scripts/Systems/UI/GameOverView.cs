using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Systems.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private TextMeshProUGUI _finalScore;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private float _scaleDuration = 0.4f;

        private SignalBus _bus;
        private bool _isShown;

        [Inject]
        public void Construct(SignalBus bus)
        {
            _bus = bus;
            _bus.Subscribe<GameOverSignal>(ShowScoreView);
        }

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _panel.localScale = Vector3.zero;
            _view.SetActive(false);
        }

        private void ShowScoreView(GameOverSignal signal)
        {
            if (_isShown) return;

            _isShown = true;
            _finalScore.text = $"{signal.FinalScore}";

            ShowAnimation().Forget();
        }

        private async UniTaskVoid ShowAnimation()
        {
            _view.SetActive(true);

            Sequence seq = DOTween.Sequence();

            seq.Join(_canvasGroup.DOFade(1f, _fadeDuration));

            seq.Join(_panel.DOScale(1f, _scaleDuration)
                .SetEase(Ease.OutBack));

            await seq.AsyncWaitForCompletion().AsUniTask();
        }

        /// <summary>
        /// Just for game test
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDestroy()
        {
            _bus.Unsubscribe<GameOverSignal>(ShowScoreView);
        }
    }
}