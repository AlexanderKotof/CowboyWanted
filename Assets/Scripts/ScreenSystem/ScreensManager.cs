using ScreenSystem.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScreenSystem
{
    public class ScreensManager : MonoBehaviour
    {
        public static ScreensManager Instance;

        public BaseScreen[] screens;

        private readonly Dictionary<Type, BaseScreen> _screenInstances = new Dictionary<Type, BaseScreen>();

        private readonly Dictionary<Type, BaseScreen> _screensTable = new Dictionary<Type, BaseScreen>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            foreach (var screen in screens)
            {
                _screensTable.Add(screen.GetType(), screen);
            }

        }

        public static void HideScreen<T>() where T : BaseScreen
        {
            var screen = GetScreen<T>();
            screen.Hide();
        }

        public static T ShowScreen<T>() where T : BaseScreen
        {
            var screen = GetScreen<T>();
            if (screen)
            {
                if (!screen.isActiveAndEnabled)
                    screen.Show();

                //screen.transform.SetSiblingIndex(Instance.transform.childCount);
                return screen;
            }

            screen = InstatiateScreen<T>();
            screen.Show();
            return screen;
        }

        public static T ShowScreen<T>(T reference) where T : BaseScreen
        {
            var referenceType = reference.GetType();
            if (Instance._screenInstances.TryGetValue(referenceType, out var screen))
            {
                if (!screen.isActiveAndEnabled)
                    screen.Show();

                //screen.transform.SetSiblingIndex(Instance.transform.childCount);
                return (T)screen;
            }

            if (Instance._screensTable.TryGetValue(referenceType, out var screenPrefab))
            {
                var instance = Instantiate(screenPrefab, Instance.transform);
                Instance._screenInstances.Add(referenceType, instance);
                return (T)instance;
            }

            return null;
        }

        public static T GetScreen<T>() where T : BaseScreen
        {
            if (Instance._screenInstances.TryGetValue(typeof(T), out var screen))
            {
                return (T)screen;
            }

            return null;
        }

        public static void HideAll()
        {
            foreach (var screen in Instance._screenInstances.Values)
            {
                screen.Hide();
            }
        }

        public static void DestroyScreen<T>() where T : BaseScreen
        {
            var screen = GetScreen<T>();
            Instance._screenInstances.Remove(typeof(T));
            Destroy(screen.gameObject);
        }

        private static T InstatiateScreen<T>() where T : BaseScreen
        {
            if (Instance._screensTable.TryGetValue(typeof(T), out var screenPrefab))
            {
                var instance = Instantiate(screenPrefab, Instance.transform);
                Instance._screenInstances.Add(typeof(T), instance);
                return (T)instance;
            }
            else
                Debug.LogError("No screen prefab founded");

            return null;
        }
    }
}
