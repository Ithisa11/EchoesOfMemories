using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader I;

    [SerializeField] private float fadeDuration = 0.35f;

    private CanvasGroup group;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        BuildIfMissing();
        ForceOnTop();
        SetInstant(0f);
    }

    private void BuildIfMissing()
    {
        if (group != null) return;
        var canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(transform, false);

        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10000;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var imgGO = new GameObject("FadeImage");
        imgGO.transform.SetParent(canvasGO.transform, false);

        var img = imgGO.AddComponent<Image>();
        img.color = Color.black;

        var rt = img.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        group = imgGO.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    private void ForceOnTop()
    {
        var canvas = GetComponentInChildren<Canvas>(true);
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10000;
        }
    }

    public void SetInstant(float a)
    {
        if (group == null) BuildIfMissing();
        ForceOnTop();

        group.alpha = Mathf.Clamp01(a);
        bool block = group.alpha > 0.01f;
        group.blocksRaycasts = block;
        group.interactable = block;
    }

    public IEnumerator FadeOut()
    {
        yield return FadeTo(1f);
    }

    public IEnumerator FadeIn()
    {
        yield return FadeTo(0f);
    }

    private IEnumerator FadeTo(float target)
    {
        if (group == null) BuildIfMissing();
        ForceOnTop();
        if (target > 0.01f)
        {
            group.blocksRaycasts = true;
            group.interactable = true;
        }

        float start = group.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);
            group.alpha = Mathf.Lerp(start, target, k);
            yield return null;
        }

        group.alpha = target;
        bool block = group.alpha > 0.01f;
        group.blocksRaycasts = block;
        group.interactable = block;
    }
}
