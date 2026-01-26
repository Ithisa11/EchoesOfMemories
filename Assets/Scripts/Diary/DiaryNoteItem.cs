using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryNoteItem : MonoBehaviour
{
    [SerializeField] private TMP_Text indexText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button button;

    private DiaryEntry entry;
    private DiaryUIController ui;

    public void Bind(int displayIndex, DiaryEntry e, DiaryUIController controller)
    {
        entry = e;
        ui = controller;

        if (indexText) indexText.text = $"Note {displayIndex}";
        if (titleText) titleText.text = e.title;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ui.ShowRead(entry));
    }
}
