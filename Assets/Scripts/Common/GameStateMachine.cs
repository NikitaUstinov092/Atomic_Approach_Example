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

        private void Update()
        {
            UpdateGameListener(Time.deltaTime);
        }

        private void OnDisable()
        {
            Disable();
        }

        private void InitGame()
        {
            foreach (var listener in _container.Resolve<IEnumerable<IInitListener>>())
            {
                listener.OnInit();
            }
        }

        private void StartGame()
        {
            foreach (var listener in _container.Resolve<IEnumerable<IStartListener>>())
            {
                listener.StartGame();
            }
            _state = StateGame.PLAYING;
        }

        private void UpdateGameListener(float deltaTime)
        {
            if (_state != StateGame.PLAYING)
                return;

            foreach (var listener in _container.Resolve<IEnumerable<IUpdateListener>>())
            {
                listener.Update(deltaTime);
            }
        }

        private void Disable()
        {
            foreach (var listener in _container.Resolve<IEnumerable<IDisableListener>>())
            {
                listener.Disable();
            }
            _state = StateGame.OFF;
        }

    }
}