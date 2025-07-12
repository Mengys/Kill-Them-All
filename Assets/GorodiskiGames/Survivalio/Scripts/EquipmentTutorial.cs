using Game.UI.Hud;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentTutorial : MonoBehaviour {
    [SerializeField] private ButtonMenuView _button;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private GameObject _weaponCell;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private GameObject _hand;

    private void Start() {
        if (PlayerPrefs.GetInt("EquipmentTutorial") == 1 || _textMeshPro.text[0] != '2') {
            Destroy(gameObject);
            return;
        }
        _button.OnButtonClick();
        _weaponCell.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnWeaponCellClick);
        _hand.SetActive(true);
        _hand.transform.position = _weaponCell.transform.position;
    }

    private void OnWeaponCellClick() {
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        _hand.transform.position = _upgradeButton.transform.position;
        _weaponCell.transform.GetChild(0).GetComponent<Button>().onClick.RemoveListener(OnWeaponCellClick);
    }

    private void OnUpgradeButtonClick() {
        Destroy(gameObject);
        PlayerPrefs.SetInt("EquipmentTutorial", 1);
        _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
    }
}
