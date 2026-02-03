using System.Collections;
using UnityEngine;

public class BedWakePopupTrigger : MonoBehaviour
{
    [SerializeField] private BedWakeMessage popup;
    [SerializeField] private float delay = 1f;
    [SerializeField] private bool triggerOnce = true;

    private bool used;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used && triggerOnce) return;
        if (!other.CompareTag("Player")) return;

        used = true;
        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (popup != null)
            popup.Show();
    }
}