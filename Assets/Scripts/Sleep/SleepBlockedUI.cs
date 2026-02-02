using System.Collections;
using UnityEngine;
using TMPro;

public class SleepBlockedUI : MonoBehaviour
{
    [Header("UI object to enable/disable (SleepBlockedText)")]
    public GameObject uiRoot;

    [Header("Text (optional if you just show/hide)")]
    public TextMeshProUGUI text;

    public float showDuration = 2.5f;

    private Coroutine routine;

    private void Awake()
    {
        // Auto-find text if not assigned
        if (text == null && uiRoot != null)
            text = uiRoot.GetComponent<TextMeshProUGUI>();

        // Hide UI at start (but DO NOT disable this script object)
        if (uiRoot != null)
            uiRoot.SetActive(false);
    }

    public void Show(string message = null)
    {
        if (uiRoot == null)
        {
            Debug.LogWarning("SleepBlockedUI: uiRoot is not assigned.");
            return;
        }

        if (!string.IsNullOrEmpty(message) && text != null)
            text.text = message;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        uiRoot.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        uiRoot.SetActive(false);
        routine = null;
    }
}