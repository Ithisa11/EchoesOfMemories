using UnityEngine;

public class HideIfAlreadyPicked : MonoBehaviour
{
    private void Awake()
    {
        if (InventoryManager.I != null && InventoryManager.I.hasBathroomKey)
        {
            Destroy(gameObject);
        }
    }
}