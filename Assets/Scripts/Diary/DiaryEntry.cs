using System;
using UnityEngine;

public enum DiaryEntrySource
{
    Player = 0,
    Auto = 1
}

[Serializable]
public class DiaryEntry
{
    public string id;
    public string title;

    [TextArea(3, 20)]
    public string body;

    public DiaryEntrySource source;
}
