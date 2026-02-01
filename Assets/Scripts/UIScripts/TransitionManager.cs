using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager I;

    private bool transitioning;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (transitioning) return;
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        transitioning = true;
        if (ScreenFader.I != null)
        yield return ScreenFader.I.FadeOut();
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;
        yield return null;
        if (ScreenFader.I != null)
            yield return ScreenFader.I.FadeIn();

        transitioning = false;
    }
}
