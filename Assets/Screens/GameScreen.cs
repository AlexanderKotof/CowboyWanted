using ScreenSystem.Screens;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : BaseScreen
{
    public Button shootButton;
    public Button upgradeWeaponButton;
    public Button bombButton;

    public TMP_Text upgradeCostText;
    public TMP_Text pistolLevelText;
    public TMP_Text bombCooldown;

    public TMP_Text coinsText;

    private View _view;

    private GameController _controller;

    public void SetInfo(GameController controller, View view)
    {
        _controller = controller;
        _view = view;

        _view.ViewUpdated += ViewUpdated;
    }

    protected override void OnShow()
    {
        base.OnShow();

        shootButton.onClick.AddListener(() => _controller.shoot?.Invoke());
        upgradeWeaponButton.onClick.AddListener(() => _controller.upgradeWeapon?.Invoke());
        bombButton.onClick.AddListener(() => _controller.throwBomb?.Invoke());
    }

    private void ViewUpdated()
    {
        coinsText.SetText(_view.coins.ToString());

        if (_view.pistolLevel == 0)
            pistolLevelText.SetText("Buy");
        else
        {
            pistolLevelText.SetText($"Lvl {_view.pistolLevel}");
        }

        if (_view.upgradePrice == 0)
        {
            upgradeCostText.SetText("");
        }
        else
            upgradeCostText.SetText($"Cost {_view.upgradePrice}");

        if (_view.bombCooldown > 0)
        {
            bombButton.interactable = false;
            bombCooldown.gameObject.SetActive(true);
            bombCooldown.SetText(_view.bombCooldown.ToString());
        }
        else
        {
            bombButton.interactable = true;
            bombCooldown.gameObject.SetActive(false);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        _view.ViewUpdated -= ViewUpdated;

        shootButton.onClick.RemoveAllListeners();
        upgradeWeaponButton.onClick.RemoveAllListeners();
        bombButton.onClick.RemoveAllListeners();

        _controller = null;
        _view = null;
    }

    protected override void OnDestroy()
    {
       
    }
}
