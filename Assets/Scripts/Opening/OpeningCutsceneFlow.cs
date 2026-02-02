using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Video;

public class OpeningCutsceneFlow : MonoBehaviour
{
    [Header("Next Scene")]
    [SerializeField] private string bedroomSceneName = "Bedroom";

    [Header("Cutscene (choose one)")]
    [SerializeField] private VideoPlayer videoPlayer;        // if using Video
    [SerializeField] private PlayableDirector timeline;      // if using Timeline

    [Header("Optional UI")]
    [SerializeField] private CanvasGroup fadeCanvas;         // optional fade (alpha 0 -> 1)
    [SerializeField] private float fadeDuration = 0.5f;

    private bool running;

    // Call this when the player sleeps
    public void StartSleepThenCutscene()
    {
        if (running) return;
        StartCoroutine(RunFlow());
    }

    private IEnumerator RunFlow()
    {
        running = true;

        // Optional: fade to black
        if (fadeCanvas != null)
            yield return Fade(0f, 1f);

        // Play cutscene
        yield return PlayCutscene();

        // Optional: fade out from black (or keep black and load)
        // If you want the bedroom to appear from black, keep fadeCanvas at 1 and fade in after load in Bedroom.

        SceneManager.LoadScene(bedroomSceneName);
    }

    private IEnumerator PlayCutscene()
    {
        // VIDEO
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();

            // Wait until video finishes
            while (videoPlayer.isPlaying)
                yield return null;

            yield break;
        }

        // TIMELINE
        if (timeline != null)
        {
            timeline.Stop();
            timeline.Play();

            while (timeline.state == PlayState.Playing)
                yield return null;

            yield break;
        }

        Debug.LogWarning("No VideoPlayer or Timeline assigned for cutscene.");
        yield return null;
    }

    private IEnumerator Fade(float from, float to)
    {
        if (fadeCanvas == null) yield break;

        fadeCanvas.blocksRaycasts = true;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = to;
    }
}