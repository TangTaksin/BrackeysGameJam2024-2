using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericSceneLoader : MonoBehaviour
{
    public static string _refScene;
    static bool transitioning;

    public static void TriggerLoadScene(string _scneName)
    {
        if (!transitioning)
        {
            _refScene = _scneName;
            Transition.CalledFadeIn?.Invoke();
            Transition.FadeInOver += LoadScene;
            transitioning = true;
        }

    }

    static void LoadScene()
    {
        transitioning = false;
        Transition.FadeInOver -= LoadScene;
        SceneManager.LoadScene(_refScene);
    }

    public void LoadSceneNoFade(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
