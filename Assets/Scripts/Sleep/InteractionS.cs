using UnityEngine;

public class InteractionS : MonoBehaviour
{
    [SerializeField] private GameObject SUI;

    private void Awake()
    {
        if (SUI != null)
            SUI.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        if (SUI != null)
            SUI.SetActive(show);
    }
}
