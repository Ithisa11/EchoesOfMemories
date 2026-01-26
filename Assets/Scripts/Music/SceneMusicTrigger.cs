using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    public AudioClip sceneClip;
    public float fadeSeconds = 0.5f;

    void Start()
    {
        if (MusicManager.Instance != null && sceneClip != null)
            MusicManager.Instance.EnsurePlaying(sceneClip, fadeSeconds);
    }
}
