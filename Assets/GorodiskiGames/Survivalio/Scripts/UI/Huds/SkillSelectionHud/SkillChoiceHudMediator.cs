using Game.Core.UI;
using Game.Managers;
using Game.Weapon;
using Injection;
using System.Linq;

namespace Game.UI.Hud
{
    public sealed class SkillChoiceHudMediator : Mediator<SkillChoiceHudView>
    {
        [Inject] private LevelManager _levelManager;
        [Inject] private HudManager _hudManager;
        [Inject] private GameManager _gameManager;

        protected override void Show()
        {
            _levelManager.Pause();

            var configs = _levelManager.Model.WeaponConfigs;
            foreach (var config in configs)
            {
                var serial = GameConstants.GetSerial();
                var index = config.Index;
                var existedWeapon = _levelManager.WeaponsMap.Keys.ToList().Find(w => w.Model.Index == index);
                var model = new WeaponModel(config, index, serial, 0);

                var existed = existedWeapon != null;
                if (existed)
                {
                    var bulletsCount = existedWeapon.Model.BulletsCount;
                    bulletsCount++;
                    model.BulletsCount = bulletsCount;
                }

                var slotIndex = configs.ToList().IndexOf(config);
                var view = _view.Slots[slotIndex];
                view.Model = model;

                view.ON_CLICK += OnSlotClick;
            }

            foreach (var icon in _view.WeaponIcons)
            {
                var index = _view.WeaponIcons.ToList().IndexOf(icon);
                var weapon = _levelManager.WeaponsMap.Keys.ToList().ElementAtOrDefault(index);
                var visibility = weapon != null;
                icon.gameObject.SetActive(visibility);

                if (!visibility)
                    continue;

                icon.sprite = weapon.Model.Icon;
            }

            foreach (var icon in _view.AbilityIcons)
            {
                icon.gameObject.SetActive(false);
            }
        }

        protected override void Hide()
        {
            foreach (var slot in _view.Slots)
            {
                slot.ON_CLICK -= OnSlotClick;
                slot.Model = null;
            }

            foreach (var icon in _view.WeaponIcons)
            {
                icon.sprite = null;
            }

            foreach (var icon in _view.AbilityIcons)
            {
                icon.sprite = null;
            }

            _levelManager.Unpause();
        }

        private void OnSlotClick(WeaponModel model)
        {
            _hudManager.HideSingle();
            _gameManager.FireWeaponSelected(model);
        }
    }
}

