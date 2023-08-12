using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Common
{
    public enum StateGame
    {
        OFF = 0,
        PLAYING = 1,
    }

    public class GameStateMachine : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        public StateGame State
        {
            get { return _state; }
        }

        private StateGame _state;

        [Inject]
        private readonly DiContainer _container;

        public void Awake()
        {
            InitGame();
        }
        private void Start()
        {
            StartGame();
        }

        private void OnDisable()
        {
            Disable();
        }

        public void InitGame()
        {
            foreach (var listener in _container.Resolve<IEnumerable<IInitListener>>())
            {
                listener.OnInit();
            }
        }

        public void StartGame()
        {
            foreach (var listener in _container.Resolve<IEnumerable<IStartListener>>())
            {
                listener.StartGame();
            }
            _state = StateGame.PLAYING;
        }

        public void Disable()
        {
            foreach (var listener in _container.Resolve<IEnumerable<IDisableListener>>())
            {
                listener.Disable();
            }
            _state = StateGame.OFF;
        }

    }
}