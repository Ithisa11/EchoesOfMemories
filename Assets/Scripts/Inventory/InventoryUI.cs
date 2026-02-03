using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject inventoryPanel;  

    [Header("Icons")]
    [SerializeField] private Image bathroomKeyIcon; 

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.I;

    private void Awake()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        if (bathroomKeyIcon != null)
            bathroomKeyIcon.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (inventoryPanel != null)
            {
                bool newState = !inventoryPanel.activeSelf;
                inventoryPanel.SetActive(newState);

                if (newState)
                    RefreshIcons();
            }
        }
    }

    private void RefreshIcons()
    {
        if (InventoryManager.I == null) return;

        if (bathroomKeyIcon != null)
            bathroomKeyIcon.enabled = InventoryManager.I.hasBathroomKey;
    }
}