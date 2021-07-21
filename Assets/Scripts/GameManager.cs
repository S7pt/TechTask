using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;
    private string _currentScene;
    private static GameManager _manager;

    
    private void Awake()
    {
        if (_manager != null && _manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _manager = this;
        }
    }

    void Start()
    {
        EntranceCheck.SubscribeToPassedDoors(GameWonScreen);
        PlayerScript.SubscribeToPlayerDeath(GameOverScreen);
        _currentScene = SceneManager.GetActiveScene().name;
    }

    private void GameOverScreen()
    {
        Time.timeScale = 0;
        if (_deathScreen != null)
        {
            _deathScreen.SetActive(true);
        }
    }

    private void GameWonScreen()
    {
        Time.timeScale = 0;
        if (_winScreen != null)
        {
            _winScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(_currentScene);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
