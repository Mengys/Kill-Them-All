using System;
using System.Collections.Generic;
using Core;
using Game.Cloth;
using Game.Config;
using Game.Core;
using Game.Effect;
using Game.Managers;
using Game.Modules;
using Game.Player.States;
using Game.Unit;
using Game.Weapon;
using Injection;
using UnityEngine;

namespace Game.Player
{
    public enum UnitAttributeType
    {
        Attack,
        Health
    }

    public abstract class UnitModel : Observable
    {
        public float HealthNominal;

        protected Dictionary<UnitAttributeType, float> _attributes;

        public float Health
        {
            get { return _attributes[UnitAttributeType.Health]; }
            set { _attributes[UnitAttributeType.Health] = value; }
        }
    }

    public sealed class PlayerModel : UnitModel
    {
        public Sprite Icon;
        public string Label;
        public float WalkSpeed;
        public float RotateSpeed;
        public bool HasAllClothMeshes;
        public Mesh FullSkinnedMesh;
        public Mesh HelmetMesh;

        private readonly PlayerData _data;
        public Dictionary<ClothElementType, Mesh> ClothMeshMap;

        public int EquippedWeapon
        {
            get { return _data.EquippedWeapon; }
            set { _data.EquippedWeapon = value; }
        }
        public Dictionary<int, int> StoredWeapons => _data.StoredWeapons;
        public Dictionary<int, int> WeaponLevels => _data.WeaponLevels;

        public List<int> EquippedCloth => _data.EquippedCloth;
        public Dictionary<int, int> StoredCloth => _data.StoredCloth;
        public Dictionary<int, int> ClothLevels => _data.ClothLevels;

        public float Attack
        {
            get { return _attributes[UnitAttributeType.Attack]; }
            set { _attributes[UnitAttributeType.Attack] = value; }
        }

        public PlayerModel(GameConfig gameConfig)
        {
            _data = PlayerData.Load(gameConfig);

            var config = gameConfig.PlayerConfig;

            Label = config.Label;
            HasAllClothMeshes = config.HasAllClothMeshes;
            Icon = config.Icon;
            WalkSpeed = config.WalkSpeed;
            RotateSpeed = config.RotateSpeed;
            FullSkinnedMesh = config.FullSkinnedMesh;

            _attributes = new Dictionary<UnitAttributeType, float>();
            foreach (UnitAttributeType value in Enum.GetValues(typeof(UnitAttributeType)))
            {
                _attributes[value] = 0;
            }

            //attack
            if(StoredWeapons.ContainsKey(EquippedWeapon))
            {
                var index = StoredWeapons[EquippedWeapon];
                var weaponConfig = gameConfig.WeaponMap[index];
                var level = WeaponLevels[EquippedWeapon];
                var weaponModel = new WeaponModel(weaponConfig, index, EquippedWeapon, level);
                Attack = weaponModel.Attack;
            }

            //health
            ClothMeshMap = new Dictionary<ClothElementType, Mesh>();
            var health = config.Health;
            foreach (var serial in EquippedCloth)
            {
                var configIndex = StoredCloth[serial];
                var clothConfig = gameConfig.ClothMap[configIndex];
                var level = ClothLevels[serial];
                var clothModel = new ClothModel(clothConfig, configIndex, serial, level);
                ClothMeshMap[clothModel.ClothType] = clothModel.Mesh;
                health += (int)clothModel.Armor;
            }

            Health = health;
            UpdateNominalHealth();
        }

        public void UpdateNominalHealth()
        {
            HealthNominal = Health;
        }

        public void Save()
        {
            _data.Save();
        }
    }

    public sealed class PlayerController : UnitController, IDisposable
    {
        private const string _damageFormat = "-{0}";

        private const float _distance = 1.5f;
        private const float _speed = 15f;

        private readonly PlayerView _view;
        private readonly PlayerModel _model;

        public PlayerModel Model => _model;
        public override TeamIDType TeamID => TeamIDType.Player;

        private readonly StateManager<PlayerState> _stateManager;

        private float _spinDirection = 1f;
        public float SpinDirection => _spinDirection *= -1f;

        public PlayerController(PlayerView view, PlayerModel model, Context context) : base(view)
        {
            _view = view;
            _model = model;

            var subContext = new Context(context);
            var injector = new Injector(subContext);

            subContext.Install(this);
            subContext.Install(injector);

            _stateManager = new StateManager<PlayerState>();
            _stateManager.IsLogEnabled = false;

            injector.Inject(_stateManager);

            _view.Model = model;
            Visibility(true);
            _view.SetCollider(true);
        }

        public void Dispose()
        {
            _stateManager.Dispose();
            Visibility(false);
        }

        private void Visibility(bool value)
        {
            _view.gameObject.SetActive(value);
        }

        public void Idle()
        {
            _stateManager.SwitchToState(new PlayerIdleState());
        }

        public void IdleMenu()
        {
            _stateManager.SwitchToState(new PlayerIdleMenuState());
        }

        public void Walk()
        {
            _stateManager.SwitchToState(new PlayerWalkState());
        }

        private void Die()
        {
            _stateManager.SwitchToState(new PlayerDieState());
        }

        public void ChangeCloth()
        {
            _stateManager.SwitchToState(new PlayerChangeClothState());
        }

        public void Win()
        {
            _stateManager.SwitchToState(new PlayerWinState());
        }

        public void Damage(float damage, Vector3 direction, GameManager gameManager)
        {
            var damageInt = (int)damage;
            var damageResult = Mathf.Min(damageInt, _model.Health);

            _model.Health -= damageResult;
            _model.SetChanged();

            var blinkDuration = _distance / _speed * 0.5f;
            _view.Damage(blinkDuration);

            var colorType = UINotificationColorType.White;
            if (_model.Health <= 0)
            {
                colorType = UINotificationColorType.Red;
                Die();
            }

            var position = _view.AimPosition;
            gameManager.FireSpawnEffect(EffectType.Blood, position, direction);

            var info = string.Format(_damageFormat, damageResult);
            gameManager.FireSpawnNotificationPopUp(info, position, colorType);
        }
    }
}