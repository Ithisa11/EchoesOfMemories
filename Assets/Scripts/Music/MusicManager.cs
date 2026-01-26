using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Defaults")]
    public AudioClip defaultClip;
    public float defaultFade = 0.5f;

    AudioSource _source;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); 

        _source = gameObject.AddComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = true;
        _source.spatialBlend = 0f;

        if (defaultClip != null) Play(defaultClip, 0f);
    }

    public AudioClip CurrentClip => _source.clip;
    public bool IsPlaying => _source.isPlaying;


    public void EnsurePlaying(AudioClip clip, float fadeSeconds = -1f)
    {
        if (clip == null) return;
        if (_source.clip == clip && _source.isPlaying) return; 
        Play(clip, fadeSeconds);
    }

    public void Play(AudioClip clip, float fadeSeconds = -1f)
    {
        if (clip == null) return;
        if (fadeSeconds < 0f) fadeSeconds = defaultFade;
        StartCoroutine(SwapClip(clip, fadeSeconds));
    }

    IEnumerator SwapClip(AudioClip newClip, float fade)
    {
        if (_source.isPlaying && fade > 0f)
            yield return StartCoroutine(FadeTo(0f, fade));

        _source.clip = newClip;
        _source.Play();

        if (fade > 0f)
            yield return StartCoroutine(FadeTo(1f, fade));
    }

    IEnumerator FadeTo(float target, float duration)
    {
        float start = _source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _source.volume = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        _source.volume = target;
    }
}
