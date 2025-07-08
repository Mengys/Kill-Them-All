using System;
using Game.Config;
using Game.Core;
using Game.Equipment;
using Game.Weapon.States;
using Injection;

namespace Game.Weapon
{
    public sealed class WeaponModel : EquipmentModel
    {
        public float Range;
        public float BulletMoveSpeed;
        public float BulletLifeTime;
        public BulletControllerType BulletType;
        public int BulletIndex;
        public int BulletsCount;
        public float FireRate;
        public float MultipleBulletsDelay;
        public float OrbitRadius;

        public float Attack => 1f / FireRateNominal * Damage;
        public float FireRateNominal => Attributes[AttributeType.FireRate];
        public float Damage => Attributes[AttributeType.Damage];

        public WeaponModel(WeaponConfig config, int index, int serial, int level) : base(config, index, serial, level)
        {
            Range = config.Range;
            BulletType = config.BulletType;
            BulletIndex = config.BulletIndex;
            BulletMoveSpeed = config.BulletSpeed;
            BulletLifeTime = config.BulletLifeTime;
            MultipleBulletsDelay = config.MultipleBulletsDelay;
            BulletsCount = config.BulletsCount;
            OrbitRadius = config.OrbitRadius;

            UpdateStats();
        }

        public override void SetLocalParameters()
        {
            FireRate = Attributes[AttributeType.FireRate];
        }
    }

    public sealed class WeaponController : IDisposable
    {
        public event Action<WeaponController> ON_READY;

        public WeaponModel Model => _model;

        private readonly StateManager<WeaponState> _stateManager;
        private readonly WeaponModel _model;

        public WeaponController(WeaponModel model, Context context)
        {
            _model = model;

            var subContext = new Context(context);
            var injector = new Injector(subContext);

            subContext.Install(this);
            subContext.Install(injector);

            _stateManager = new StateManager<WeaponState>
            {
                IsLogEnabled = false
            };

            injector.Inject(_stateManager);
        }

        public void Dispose()
        {
            _stateManager.Dispose();
        }

        public void Ready()
        {
            _stateManager.SwitchToState(new WeaponReadyState());
        }

        public void Reload()
        {
            _stateManager.SwitchToState(new WeaponReloadState());
        }

        public void FireReady()
        {
            ON_READY?.Invoke(this);
        }
    }
}

