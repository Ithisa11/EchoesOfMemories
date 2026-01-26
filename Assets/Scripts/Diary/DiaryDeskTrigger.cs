using UnityEngine;

public class DiaryDeskTrigger : MonoBehaviour
{
    [SerializeField] private DiaryUIController diaryUI;
    [SerializeField] private GameObject pressEPrompt;

    private bool playerInside;

    private void Awake()
    {
        if (pressEPrompt != null)
            pressEPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (pressEPrompt != null)
            pressEPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (pressEPrompt != null)
            pressEPrompt.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside) return;
        if (diaryUI == null || diaryUI.IsOpen) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pressEPrompt != null)
                pressEPrompt.SetActive(false);

            diaryUI.OpenDiary();
        }
    }
}
