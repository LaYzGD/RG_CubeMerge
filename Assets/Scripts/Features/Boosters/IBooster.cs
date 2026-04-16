using Cysharp.Threading.Tasks;

namespace Game.Features.Boosters
{
    public interface IBooster
    {
       public UniTask Execute();
    }
}