using ScreenSystem.Screens;
using System;
using UnityEngine.UI;

public class LevelEndedScreen : BaseScreen
{
    public Button restartButton;

    public void SetRestartCallback(Action callback)
    {
        restartButton.onClick.AddListener(callback.Invoke);
    }

    protected override void OnHide()
    {
        restartButton.onClick.RemoveAllListeners();
        base.OnHide();
    }
}
