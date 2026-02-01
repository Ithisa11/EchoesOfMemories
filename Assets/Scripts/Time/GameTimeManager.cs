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
        gameHours = 0;
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
        timeText.text = $"{gameHours:D2}{gameMinutes:D2}";
    }
    public int GetHour() => gameHours;
    public int GetMinute() => gameMinutes;
}
