using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    private PlayerInput playerInput;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.InputAction.Enable();
        playerInput.InputAction.Pause.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        playerInput.InputAction.Pause.performed -= OnPausePerformed;
        playerInput.InputAction.Disable();
    }

    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(MainMenu);
        pausePanel.SetActive(false);
    }

    private void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        if (IsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
       
        IsPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void MainMenu()
    {
        Destroy(EmotionManager.Instance.gameObject);
        Destroy(NPCQueueManager.Instance.gameObject);
        Destroy(NotesManager.Instance.gameObject);
        Destroy(DialogueManager.Instance.gameObject);
        Destroy(EndOfDayUI.Instance.gameObject);
        Destroy(CraftingManager.Instance.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}