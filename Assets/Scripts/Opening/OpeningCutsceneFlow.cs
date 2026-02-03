using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpeningCutsceneFlow : MonoBehaviour
{
    [Header("Next Scene")]
    [SerializeField] private string bedroomSceneName = "Bedroom";

    [Header("Cutscene")]
    [SerializeField] private VideoPlayer videoPlayer;        

    private bool running;

    // Trigger after sleep
    public void StartSleepThenCutscene()
    {
        if (running) return;
        StartCoroutine(RunFlow());
    }

    private IEnumerator RunFlow()
    {
        running = true;

        // Play cutscene
        yield return PlayCutscene();

        // load nexr scene

        SceneManager.LoadScene(bedroomSceneName);
    }

    private IEnumerator PlayCutscene()
    {
        // Video
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            while (videoPlayer.isPlaying)
                yield return null;

            yield break;
        }
        yield return null;
    }
}