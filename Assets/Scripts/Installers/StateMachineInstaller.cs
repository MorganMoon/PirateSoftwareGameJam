using Cerberus;
using Cerberus.Builder;
using Cerberus.IoC;
using PirateSoftwareGameJam.Client.States;
using PirateSoftwareGameJam.Client.States.Startup;
using PirateSoftwareGameJam.Client.States.Startup.MainMenu;
using System;
using Zenject;

namespace PirateSoftwareGameJam.Client.Installers
{
    public class StateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var stateMachine = CreateStateMachine();
            Container.Bind<IStateMachine<GameState>>().FromInstance(stateMachine);
            foreach (var binding in stateMachine.StateControllerProvider.StateControllers)
            {
                Container.Bind(binding.ContractTypes).FromInstance(binding.Instance);
            }

            Container.BindInitializableExecutionOrder<StateMachineInitializer>(int.MinValue);
            Container.Bind<IInitializable>().To<StateMachineInitializer>().AsSingle();
        }

        private IStateMachine<GameState> CreateStateMachine()
        {
            return new StateMachineBuilder<GameState>(new ZenjectStateMachineContainer(Container))
                .State<StartupState, StartupStateEvents, StartupStateSubStates>(GameState.Startup)
                    .State<MainMenuState, MainMenuStateEvents>(StartupStateSubStates.MainMenu)
                    .End()
                .End()
                .Build();
        }

        private class ZenjectStateMachineContainer : IStateMachineContainer
        {
            private readonly DiContainer _diContainer;

            public ZenjectStateMachineContainer(DiContainer diContainer)
            {
                _diContainer = diContainer;
            }

            public T Resolve<T>()
            {
                if (TryResolve<T>(out var result))
                {
                    return result;
                }
                else
                {
                    return _diContainer.Instantiate<T>();
                }
            }

            public object Resolve(Type type)
            {
                if (TryResolve(type, out var result))
                {
                    return result;
                }
                else
                {
                    return _diContainer.Instantiate(type);
                }
            }

            private bool TryResolve<T>(out T result)
            {
                try
                {
                    result = _diContainer.Resolve<T>();
                    return true;
                }
                catch
                {
                    result = default;
                    return false;
                }
            }

            private bool TryResolve(Type contract, out object result)
            {
                try
                {
                    result = _diContainer.Resolve(contract);
                    return true;
                }
                catch
                {
                    result = default;
                    return false;
                }
            }
        }
    }
}
