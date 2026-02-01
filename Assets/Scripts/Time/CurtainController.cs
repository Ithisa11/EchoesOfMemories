using UnityEngine;

public class CurtainController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private SpriteRenderer targetRenderer; 

    [Header("Sprites")]
    [SerializeField] private Sprite curtainsClosed;
    [SerializeField] private Sprite curtainsOpen;

    [Header("Daylight Rules")]
    [SerializeField] private int dayStartHour = 6;  
    [SerializeField] private int nightStartHour = 19; 

    private bool? lastIsDay = null;

    void Reset()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        if (targetRenderer == null) targetRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (timeManager == null || targetRenderer == null) return;

        int h = timeManager.GetHour();
        bool isDay = (h >= dayStartHour && h < nightStartHour);
        if (lastIsDay.HasValue && lastIsDay.Value == isDay) return; 

        targetRenderer.sprite = isDay ? curtainsClosed : curtainsOpen;
        lastIsDay = isDay;
    }
}
