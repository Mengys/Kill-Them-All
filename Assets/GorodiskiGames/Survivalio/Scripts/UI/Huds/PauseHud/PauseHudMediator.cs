using System.Linq;
using Game.Core.UI;
using Game.Managers;
using Game.Weapon;
using Injection;

namespace Game.UI.Hud
{
    public sealed class PauseHudMediator : Mediator<PauseHudView>
    {
        [Inject] private LevelManager _levelManager;
        [Inject] private HudManager _hudManager;

        protected override void Show()
        {
            _levelManager.Pause();

            _view.Model = _levelManager.Model;

            foreach (var slot in _view.WeaponSlots)
            {
                var index = _view.WeaponSlots.ToList().IndexOf(slot);
                var weapon = _levelManager.WeaponsMap.Keys.ToList().ElementAtOrDefault(index);
                slot.Initialize(weapon);
            }

            foreach (var slot in _view.AbilitySlots)
            {
                slot.Initialize(null);
            }

            _view.HomeButton.onClick.AddListener(OnHomeButtonClick);
            _view.ContinueButton.onClick.AddListener(OnContinueButtonClick);
        }

        protected override void Hide()
        {
            _view.HomeButton.onClick.RemoveListener(OnHomeButtonClick);
            _view.ContinueButton.onClick.RemoveListener(OnContinueButtonClick);

            foreach (var slot in _view.WeaponSlots)
            {
                slot.Model = null;
            }

            foreach (var slot in _view.AbilitySlots)
            {
                slot.Model = null;
            }

            _levelManager.Unpause();
        }

        private void OnContinueButtonClick()
        {
            _hudManager.HideAdditional<PauseHudMediator>();
        }

        private void OnHomeButtonClick()
        {
            _hudManager.ShowAdditional<LeaveGameplayHudMediator>();
        }
    }
}

