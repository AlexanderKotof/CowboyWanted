using System;
using UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader
{
    private const string _gameSceneName = "Game";

    private static Action _callback;

    public static void LoadGameScene(Action callback)
    {
        _callback = callback;
        ScreenSystem.ScreensManager.ShowScreen<LoadingScreen>();

        var operation = SceneManager.LoadSceneAsync(_gameSceneName);
        operation.completed += OnLoaded;
    }

    private static void OnLoaded(AsyncOperation operation)
    {
        operation.completed -= OnLoaded;

        ScreenSystem.ScreensManager.HideScreen<LoadingScreen>();

        _callback?.Invoke();
    }
}
