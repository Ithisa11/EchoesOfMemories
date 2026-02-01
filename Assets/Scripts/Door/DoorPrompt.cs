using UnityEngine;

public class DoorPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptUI;

    private void Awake()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        if (promptUI != null)
            promptUI.SetActive(show);
    }
}
