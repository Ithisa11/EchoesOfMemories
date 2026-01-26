using System;
using System.IO;
using UnityEngine;

public static class DiaryStorage
{
    private const string FileName = "diary.json";

    private static string FilePath =>
        Path.Combine(Application.persistentDataPath, FileName);

    public static DiaryData Load()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new DiaryData();

            var json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(json))
                return new DiaryData();

            var data = JsonUtility.FromJson<DiaryData>(json);
            return data ?? new DiaryData();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[DiaryStorage] Load failed: {e.Message}");
            return new DiaryData();
        }
    }

    public static void Save(DiaryData data)
    {
        try
        {
            if (data == null) data = new DiaryData();
            var json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[DiaryStorage] Save failed: {e.Message}");
        }
    }
}
