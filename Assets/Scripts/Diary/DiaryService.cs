using System;
using System.Collections.Generic;
using UnityEngine;

public class DiaryService : MonoBehaviour
{
    public static DiaryService Instance { get; private set; }

    private DiaryData data;

    public int EntryCount => data?.entries?.Count ?? 0;

    public event Action OnDiaryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Load();
    }

    public void Load()
    {
        data = DiaryStorage.Load();
        OnDiaryChanged?.Invoke();
    }

    public void Save()
    {
        DiaryStorage.Save(data);
    }

    public IReadOnlyList<DiaryEntry> GetEntries()
    {
        if (data == null) data = new DiaryData();
        return data.entries;
    }

    public void AddPlayerEntry(string title, string body)
    {
        AddEntry(title, body, DiaryEntrySource.Player);
    }

    public void AddAutoEntry(string title, string body)
    {
        AddEntry(title, body, DiaryEntrySource.Auto);
    }

    private void AddEntry(string title, string body, DiaryEntrySource source)
    {
        if (data == null) data = new DiaryData();

        string safeTitle = string.IsNullOrWhiteSpace(title) ? "(Untitled)" : title.Trim();
        string safeBody = body?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(safeBody))
            return;

        var entry = new DiaryEntry
        {
            id = Guid.NewGuid().ToString("N"),
            title = safeTitle,
            body = safeBody,
            source = source
        };

        data.entries.Add(entry);
        Save();
        OnDiaryChanged?.Invoke();
    }

}
