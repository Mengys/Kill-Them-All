using System;
using Game.Managers;
using Game.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Game.UI.Hud
{
    public sealed class DefeatHudView : BaseHudWithModel<LevelModel>
    {
        private const string _levelFormat = "LEVEL {0}";
        private const string _durationMaxFormat = "BEST TIME: {0}";
        private const string _enemiesKilledFormat = "{0} {1}";

        [SerializeField] private Button _confirmButton;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _newRecordTextText;
        [SerializeField] private TMP_Text _elapsedText;
        [SerializeField] private TMP_Text _durationMaxText;
        [SerializeField] private TMP_Text _enemiesKilledText;
        [SerializeField] private RectTransform _content;
        [SerializeField] private GridLayoutGroup _layoutGroup;

        [SerializeField] private GameObject _tutorial;

        public RectTransform Content => _content;
        public GridLayoutGroup LayoutGroup => _layoutGroup;
        public Button ConfirmButton => _confirmButton;

        protected override void OnEnable()
        {
            _tutorial.SetActive(false);
        }

        protected override void OnDisable()
        {

        }

        protected override void OnModelChanged(LevelModel model)
        {
            var levelNice = model.Level + 1;
            _levelText.text = string.Format(_levelFormat, levelNice);
            _newRecordTextText.gameObject.SetActive(model.IsNewRecord);

            var elapsed = TimeSpan.FromSeconds(model.Elapsed);
            _elapsedText.text = elapsed.DateToMMSS();

            var durationMax = ColorUtil.ColorString(TimeSpan.FromSeconds(model.DurationMax).DateToMMSS(), ColorUtil.HEXToColor(GameConstants.GreenColorHex));
            _durationMaxText.text = string.Format(_durationMaxFormat, durationMax);
            _enemiesKilledText.text = string.Format(_enemiesKilledFormat, GameConstants.EnemyIcon, model.EnemiesKilled);
        }
    }
}

