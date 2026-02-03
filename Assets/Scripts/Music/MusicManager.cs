using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Defaults")]
    public AudioClip defaultClip;
    public float defaultFade = 0.5f;

    private AudioSource _source;
    private bool _muted;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
        _source = GetComponent<AudioSource>();
        if (_source == null) _source = gameObject.AddComponent<AudioSource>();

        _source.playOnAwake = false;
        _source.loop = true;
        _source.spatialBlend = 0f;
        if (defaultClip != null && _source.clip == null)
        {
            _source.clip = defaultClip;
            _source.volume = 1f;
            _muted = false;
            _source.Play();
        }
    }

    public AudioClip CurrentClip => _source != null ? _source.clip : null;
    public bool IsPlaying => _source != null && _source.isPlaying;

    public void EnsurePlaying(AudioClip clip, float fadeSeconds = -1f)
    {
        if (clip == null || _source == null) return;
        if (_source.clip == clip && _source.isPlaying) return;

        Play(clip, fadeSeconds);
    }

    public void Play(AudioClip clip, float fadeSeconds = -1f)
    {
        if (clip == null || _source == null) return;
        if (fadeSeconds < 0f) fadeSeconds = defaultFade;

        StopAllCoroutines();
        StartCoroutine(SwapClip(clip, fadeSeconds));
    }

    public void SetMuted(bool muted, float fadeSeconds = -1f)
    {
        if (_source == null) return;
        if (fadeSeconds < 0f) fadeSeconds = defaultFade;

        _muted = muted;

        StopAllCoroutines();
        StartCoroutine(FadeTo(muted ? 0f : 1f, fadeSeconds));
    }
    public void StopMusic(float fadeSeconds = -1f)
    {
        if (_source == null) return;
        if (fadeSeconds < 0f) fadeSeconds = defaultFade;

        _muted = true;

        StopAllCoroutines();
        StartCoroutine(StopAfterFade(fadeSeconds));
    }

    public void ResumeDefault(float fadeSeconds = -1f)
    {
        if (fadeSeconds < 0f) fadeSeconds = defaultFade;
        SetMuted(false, fadeSeconds);
        if ((_source == null) || defaultClip == null) return;

        if (!_source.isPlaying || _source.clip != defaultClip)
            EnsurePlaying(defaultClip, fadeSeconds);
    }

    private IEnumerator StopAfterFade(float fade)
    {
        if (_source.isPlaying && fade > 0f)
            yield return StartCoroutine(FadeTo(0f, fade));
        else
            _source.volume = 0f;

        _source.Stop();
    }

    private IEnumerator SwapClip(AudioClip newClip, float fade)
    {
        if (_source.isPlaying && fade > 0f)
            yield return StartCoroutine(FadeTo(0f, fade));

        _source.clip = newClip;
        _source.Play();
        float target = _muted ? 0f : 1f;

        if (fade > 0f)
            yield return StartCoroutine(FadeTo(target, fade));
        else
            _source.volume = target;
    }

    private IEnumerator FadeTo(float target, float duration)
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