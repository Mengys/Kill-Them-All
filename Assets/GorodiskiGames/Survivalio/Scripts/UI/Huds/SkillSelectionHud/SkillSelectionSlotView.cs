using System;
using System.Linq;
using DG.Tweening;
using Game.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public sealed class SkillSelectionSlotView : BaseHudWithModel<WeaponModel>
    {
        private const float _fade = 0.5f;

        public event Action<WeaponModel> ON_CLICK;

        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Image[] _stars;

        protected override void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        protected override void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            KillTween();
        }

        private void OnButtonClick()
        {
            ON_CLICK?.Invoke(Model);
        }

        protected override void OnApplyModel(WeaponModel model)
        {
            base.OnApplyModel(model);
        }

        protected override void OnModelChanged(WeaponModel model)
        {
            _icon.sprite = model.Icon;
            _labelText.text = model.Label;

            var bulletsCount = model.BulletsCount - 1;

            foreach (var star in _stars)
            {
                var index = _stars.ToList().IndexOf(star);

                var fade = 1f;
                if(index > bulletsCount)
                    fade = 0f;

                var color = star.color;
                star.color = new Color(color.r, color.g, color.b, fade);

                if(index == bulletsCount)
                {
                    var image = star.GetComponent<Image>();
                    image.DOFade(0f, _fade).SetLoops(-1, LoopType.Yoyo).SetId(this);
                }
            }
        }

        private void KillTween()
        {
            DOTween.Kill(this);
        }
    }
}

