using System.Linq;
using Game.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public sealed class SkillPauseSlotView : BaseHudWithModel<WeaponModel>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image[] _stars;

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }

        protected override void OnModelChanged(WeaponModel model)
        {
            _icon.sprite = model.Icon;

            var bulletsCount = model.BulletsCount - 1;
            foreach (var star in _stars)
            {
                var index = _stars.ToList().IndexOf(star);

                var fade = 1f;
                if (index > bulletsCount)
                    fade = 0f;

                var color = star.color;
                star.color = new Color(color.r, color.g, color.b, fade);
            }
        }

        public void Initialize(WeaponController weapon)
        {
            WeaponModel model = null;

            var weaponExists = weapon != null;
            if (weaponExists)
                model = weapon.Model;

            _icon.gameObject.SetActive(weaponExists);
            foreach (var star in _stars)
            {
                star.gameObject.SetActive(weaponExists);
            }

            Model = model;
        }
    }
}

