using Cerberus;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PirateSoftwareGameJam.Client.States
{
    public abstract class AsyncState : State
    {
        private CancellationTokenSource _enterCancellationTokenSource;

        public sealed override void OnEnter()
        {
            _enterCancellationTokenSource = new CancellationTokenSource();
            OnEnterAsync(_enterCancellationTokenSource.Token).Forget();
        }

        public sealed override void OnExit()
        {
            _enterCancellationTokenSource?.Cancel();
            OnExitAsync().Forget();
        }


        protected virtual UniTask OnEnterAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}
