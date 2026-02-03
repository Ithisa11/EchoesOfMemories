using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneEndLoadScene : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Cutscene")]
    [SerializeField] private GameObject cutsceneUIRoot; 

    [Header("Canvas")]
    [SerializeField] private Canvas gameplayCanvas;     

    [Header("Spawn Point")]
    [SerializeField] private string nextSpawnObjectName = "Spawn_BedWake";

    private bool started;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (cutsceneUIRoot != null)
            cutsceneUIRoot.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.loopPointReached += OnFinished;
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnFinished;
    }

    public void Play()
    {
        if (started) return;
        started = true;

        if (gameplayCanvas != null)
            gameplayCanvas.enabled = false;

        if (cutsceneUIRoot != null)
            cutsceneUIRoot.SetActive(true);

        videoPlayer.Stop();
        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    private void OnFinished(VideoPlayer vp)
    {
        SpawnRouter.nextSpawnObjectName = nextSpawnObjectName;  
        SpawnRouter.playWakeOnNextScene = true;                 
        UnityEngine.SceneManagement.SceneManager.LoadScene("Bedroom");
    }
}