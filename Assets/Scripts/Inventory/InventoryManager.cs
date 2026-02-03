using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager I { get; private set; }
    public bool hasBathroomKey;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddBathroomKey()
    {
        hasBathroomKey = true;
    }
}