using ScreenSystem.Screens;
using System;
using UnityEngine.UI;

public class StartGameScreen : BaseScreen
{
    public Button startButton;

    public void SetStartCallback(Action callback)
    {
        startButton.onClick.AddListener(callback.Invoke);
    }

    protected override void OnHide()
    {
        startButton.onClick.RemoveAllListeners();
        base.OnHide();
    }
}
