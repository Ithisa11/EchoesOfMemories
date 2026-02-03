using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    public AudioClip sceneClip;

    public float fadeSeconds = 0.5f;
    public bool muteMusicInThisScene = false;
    public bool stopInsteadOfMute = false;

    void Start()
    {
        var mm = MusicManager.Instance;
        if (mm == null)
        {
            return;
        }

        if (muteMusicInThisScene)
        {
            if (stopInsteadOfMute) mm.StopMusic(fadeSeconds);
            else mm.SetMuted(true, fadeSeconds);
            return;
        }

        AudioClip clipToPlay = sceneClip != null ? sceneClip : mm.defaultClip;

        mm.SetMuted(false, fadeSeconds);

        if (clipToPlay != null)
            mm.EnsurePlaying(clipToPlay, fadeSeconds);
    }
}