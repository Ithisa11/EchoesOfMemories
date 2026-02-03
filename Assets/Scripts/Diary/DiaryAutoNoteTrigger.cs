using UnityEngine;

public class DiaryAutoNoteTrigger : MonoBehaviour
{
    [SerializeField] private string title = "title";
    [TextArea(2, 8)]
    [SerializeField] private string body = "body";
    [SerializeField] private bool triggerOnce = true;

    private bool used;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used && triggerOnce) return;
        if (!other.CompareTag("Player")) return;

        DiaryService.Instance.AddAutoEntry(title, body);
        used = true;
    }
}
