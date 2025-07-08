using System;
using Game.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public sealed class ShopProductView : BaseHud
    {
        public Action<string> ON_CLICK;

        [SerializeField] private ShopProductConfig _config;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private TMP_Text _priceText;

        public TMP_Text PriceText => _priceText;
        public ShopProductConfig Config => _config;

        public void Initialize(string price, string reward)
        {
            _priceText.text = price;
            _rewardText.text = reward;
        }

        protected override void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        protected override void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ON_CLICK?.Invoke(_config.ID);
        }
    }
}

