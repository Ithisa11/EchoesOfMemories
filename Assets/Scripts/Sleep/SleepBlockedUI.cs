using System.Collections;
using UnityEngine;
using TMPro;

public class SleepBlockedUI : MonoBehaviour
{
    public GameObject uiRoot;
    public TextMeshProUGUI text;

    public float showDuration = 2.5f;

    private Coroutine routine;

    private void Awake()
    {
        // Auto-find text
        if (text == null && uiRoot != null)
            text = uiRoot.GetComponent<TextMeshProUGUI>();

        // Hide UI 
        if (uiRoot != null)
            uiRoot.SetActive(false);
    }

    public void Show(string message = null)
    {
        if (uiRoot == null)
        {
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