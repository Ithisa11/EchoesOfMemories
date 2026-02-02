using UnityEngine;
using TMPro;

public class GameTimeManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshPro timeText;

    [Header("Time Settings")]
    public float realSecondsPerGameMinute = 10f;

    private float timer;
    private int gameMinutes;
    private int gameHours;

    void Start()
    {
        gameHours = 9;
        gameMinutes = 0;
        UpdateTimeUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realSecondsPerGameMinute)
        {
            timer -= realSecondsPerGameMinute;
            AdvanceMinute();
        }
    }

    void AdvanceMinute()
    {
        gameMinutes++;

        if (gameMinutes >= 60)
        {
            gameMinutes = 0;
            gameHours++;

            if (gameHours >= 24)
                gameHours = 0;
        }

        UpdateTimeUI();
    }

    void UpdateTimeUI()
    {
        // Keeping your original format (HHMM with no colon)
        timeText.text = $"{gameHours:D2}{gameMinutes:D2}";
    }

    public int GetHour() => gameHours;
    public int GetMinute() => gameMinutes;

    // ✅ New: force time (used when waking up)
    public void SetTime(int hour, int minute)
    {
        gameHours = Mathf.Clamp(hour, 0, 23);
        gameMinutes = Mathf.Clamp(minute, 0, 59);
        timer = 0f; // optional: reset minute timer so it doesn't instantly tick
        UpdateTimeUI();
    }

    // ✅ New: check if it's night (22:00 -> 05:00)
public bool IsNightForSleeping()
{
    // Allowed window: 22:00 -> 05:00 (inclusive only at 05:00)
    // Meaning:
    // - 22:00 to 23:59 ✅
    // - 00:00 to 04:59 ✅
    // - 05:00 ✅
    // - 05:01+ ❌

    if (gameHours >= 22) return true;
    if (gameHours < 5) return true;
    if (gameHours == 5 && gameMinutes == 0) return true;

    return false;
}

}
