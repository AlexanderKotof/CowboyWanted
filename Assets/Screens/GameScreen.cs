using ScreenSystem.Screens;
using TMPro;
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

        _view.coins.Subscribe(SetCoins);
        _view.upgradePrice.Subscribe(SetUpgradeCost);
        _view.pistolLevel.Subscribe(SetPistolLevel);
        _view.bombCooldown.Subscribe(SetBombCooldown);
    }

    private void SetCoins(int value)
    {
        coinsText.SetText(value.ToString());
    }

    private void SetUpgradeCost(int value)
    {
        if (value == 0)
        {
            upgradeCostText.SetText("");
        }
        else
            upgradeCostText.SetText($"Cost {value}");
    }

    private void SetPistolLevel(int value)
    {
        if (value == 0)
            pistolLevelText.SetText("Buy");
        else
        {
            pistolLevelText.SetText($"Lvl {value}");
        }
    }

    private void SetBombCooldown(float value)
    {
        if (value > 0)
        {
            bombButton.interactable = false;
            bombCooldown.gameObject.SetActive(true);
            bombCooldown.SetText(value.ToString());
        }
        else
        {
            bombButton.interactable = true;
            bombCooldown.gameObject.SetActive(false);
        }
    }

    protected override void OnShow()
    {
        base.OnShow();

        shootButton.onClick.AddListener(() => _controller.shoot?.Invoke());
        upgradeWeaponButton.onClick.AddListener(() => _controller.upgradeWeapon?.Invoke());
        bombButton.onClick.AddListener(() => _controller.throwBomb?.Invoke());
    }

    protected override void OnHide()
    {
        base.OnHide();

        shootButton.onClick.RemoveAllListeners();
        upgradeWeaponButton.onClick.RemoveAllListeners();
        bombButton.onClick.RemoveAllListeners();

        _controller = null;
        _view = null;
    }
}
