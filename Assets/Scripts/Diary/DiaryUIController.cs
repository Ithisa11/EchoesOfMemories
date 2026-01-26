using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiaryUIController : MonoBehaviour
{
    [Header("Root + Panels")]
    [SerializeField] private GameObject diaryRoot;
    [SerializeField] private GameObject listPanel;
    [SerializeField] private GameObject writePanel;
    [SerializeField] private GameObject readPanel;

    [Header("List")]
    [SerializeField] private Transform listContent;      
    [SerializeField] private DiaryNoteItem itemPrefab;
    [SerializeField] private Button firstSelectedOnOpen;

    [Header("Write")]
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField bodyInput;

    [Header("Read")]
    [SerializeField] private TMP_Text readTitleText;
    [SerializeField] private TMP_Text readBodyText;

    [Header("Player Control Lock")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    [Header("Cursor")]
    [SerializeField] private bool lockCursorInGameplay = false; 

    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;
    


    private void Awake()
    {
        if (diaryRoot != null)
            diaryRoot.SetActive(false);
        if (DiaryService.Instance != null)
            DiaryService.Instance.OnDiaryChanged += OnDiaryChanged;
        SetCursorForDiary(false);
    }

    private void OnDestroy()
    {
        if (DiaryService.Instance != null)
            DiaryService.Instance.OnDiaryChanged -= OnDiaryChanged;
    }

    private void OnDiaryChanged()
    {
        if (diaryRoot != null && diaryRoot.activeSelf)
            RebuildList();
    }

    public bool IsOpen => diaryRoot != null && diaryRoot.activeSelf;

    public void OpenDiary()
    {
        if (diaryRoot == null) return;

        diaryRoot.SetActive(true);
        SetPlayerControl(false);
        SetCursorForDiary(true);

        ShowList();
        if (firstSelectedOnOpen != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(firstSelectedOnOpen.gameObject);

        if (debugLogs) Debug.Log("[DiaryUI] OpenDiary");
    }

    public void CloseDiary()
    {
        if (diaryRoot == null) return;

        diaryRoot.SetActive(false);
        SetPlayerControl(true);
        SetCursorForDiary(false);
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        if (debugLogs) Debug.Log("[DiaryUI] CloseDiary");
    }

    public void ShowList()
    {
        if (listPanel) listPanel.SetActive(true);
        if (writePanel) writePanel.SetActive(false);
        if (readPanel) readPanel.SetActive(false);

        RebuildList();

        if (debugLogs) Debug.Log("[DiaryUI] ShowList");
    }

    public void ShowWrite()
    {
        if (listPanel) listPanel.SetActive(false);
        if (writePanel) writePanel.SetActive(true);
        if (readPanel) readPanel.SetActive(false);

        if (titleInput) titleInput.text = "";
        if (bodyInput) bodyInput.text = "";

        if (titleInput) titleInput.ActivateInputField();

        if (debugLogs) Debug.Log("[DiaryUI] ShowWrite");
    }

    public void SaveNewNote()
    {
        if (DiaryService.Instance == null)
        {
            Debug.LogWarning("[DiaryUI] DiaryService.Instance is null. Did you add DiaryService to the scene?");
            return;
        }

        string title = titleInput ? titleInput.text : "";
        string body = bodyInput ? bodyInput.text : "";
        if (string.IsNullOrWhiteSpace(body))
        {
            if (debugLogs) Debug.Log("[DiaryUI] Not saving: body is empty.");
            return;
        }

        DiaryService.Instance.AddPlayerEntry(title, body);
        ShowList();
        RebuildList();

        if (debugLogs) Debug.Log($"[DiaryUI] SaveNewNote -> service count: {DiaryService.Instance.GetEntries().Count}");
    }

    public void ShowRead(DiaryEntry entry)
    {
        if (entry == null) return;

        if (listPanel) listPanel.SetActive(false);
        if (writePanel) writePanel.SetActive(false);
        if (readPanel) readPanel.SetActive(true);

        if (readTitleText) readTitleText.text = entry.title;
        if (readBodyText) readBodyText.text = entry.body;

        if (debugLogs) Debug.Log("[DiaryUI] ShowRead");
    }

    private void RebuildList()
    {
        if (DiaryService.Instance == null)
        {
            Debug.LogWarning("[DiaryUI] DiaryService.Instance is null. Cannot rebuild list.");
            return;
        }

        if (listContent == null || itemPrefab == null)
        {
            Debug.LogWarning("[DiaryUI] listContent or itemPrefab is not assigned.");
            return;
        }

        foreach (Transform child in listContent)
            Destroy(child.gameObject);

        var entries = DiaryService.Instance.GetEntries();

        if (debugLogs) Debug.Log($"[DiaryUI] RebuildList entries count = {entries.Count}");

        for (int i = 0; i < entries.Count; i++)
        {
            var item = Instantiate(itemPrefab, listContent);
            item.Bind(i + 1, entries[i], this);
        }
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = enabled;
    }

    private void SetCursorForDiary(bool diaryOpen)
    {
        Cursor.visible = diaryOpen;

        if (lockCursorInGameplay)
            Cursor.lockState = diaryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
