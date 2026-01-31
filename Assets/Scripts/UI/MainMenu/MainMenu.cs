    using UnityEngine.UI;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button creditButton;
        [SerializeField] private Button exitButton;    

        private void Start()
        {
            MusicManager.Instance.Play("mainmenu");        
            startButton.onClick.AddListener(OnStartButtonClick);        
            creditButton.onClick.AddListener(OnCreditButtonClick);
            exitButton.onClick.AddListener(OnExitButtonClick);            
        }

        private void OnStartButtonClick()
        {
            SceneManager.LoadScene(1);
        }

        private void OnCreditButtonClick()
        {
            // Will implement it later
        }

        private void OnExitButtonClick()
        {        
            Application.Quit();
        }
    }

