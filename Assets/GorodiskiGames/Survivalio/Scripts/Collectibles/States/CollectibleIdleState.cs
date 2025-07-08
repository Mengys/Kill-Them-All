using Game.Core;
using Game.Managers;
using Injection;
using UnityEngine;

namespace Game.Collectible.States
{
    public class CollectibleIdleState : CollectibleState
    {
        private const float _distance = 5f;

        [Inject] private Timer _timer;
        [Inject] private GameManager _gameManager;

        public override void Initialize()
        {
            _timer.TICK += OnTick;
        }

        public override void Dispose()
        {
            _timer.TICK -= OnTick;
        }

        private void OnTick()
        {
            var distanceToPlayer = Vector3.Distance(_gameManager.Player.View.Position, _collectible.View.Position);
            if(distanceToPlayer > _distance)
                return;

            _collectible.FlyToPlayer();
        }
    }
}

