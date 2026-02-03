using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseRoot;
    [SerializeField] private GameObject firstSelected;

    [Header("Keys")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string optionsSceneName = "Options";

    private bool isPaused;

    void Awake()
    {
        if (pauseRoot != null)
            pauseRoot.SetActive(false);

        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (isPaused &&
            EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject == null &&
            firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseRoot != null)
            pauseRoot.SetActive(true);

        LockCursor();

        if (EventSystem.current != null && firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseRoot != null)
            pauseRoot.SetActive(false);

        LockCursor();
    }

    public void GoToMainMenu()
    {
        LoadSceneSafe(mainMenuSceneName);
    }

    public void GoToOptions()
    {
        LoadSceneSafe(optionsSceneName);
    }

    private void LoadSceneSafe(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }

        Time.timeScale = 1f;
        isPaused = false;

        if (pauseRoot != null)
            pauseRoot.SetActive(false);

        LockCursor();

        SceneManager.LoadScene(sceneName);
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
