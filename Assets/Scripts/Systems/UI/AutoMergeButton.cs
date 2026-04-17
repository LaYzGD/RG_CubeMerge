using Cysharp.Threading.Tasks;
using Game.Features.Boosters;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AutoMergeButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private IBooster _booster;
    private bool _isBusy = false;

    [Inject]
    public void Construct(IBooster booster)
    {
        _booster = booster;
    }

    public void OnClick()
    {
        OnClickAsync().Forget();
    }

    private async UniTask OnClickAsync()
    {
        if (_isBusy) return;

        _isBusy = true;
        _button.interactable = false;
        try
        {
            await _booster.Execute();
        }
        finally
        {
            _isBusy = false;
            _button.interactable = true;
        }
    }
}