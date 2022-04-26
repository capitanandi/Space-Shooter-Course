using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseWindow;

    public bool _isGameOver;
    public bool _isPaused;

    private Animator _pauseAnimator;

    private void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Window").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //    Debug.Log("Application Closed");
        //}

        RestartGame();
        PauseGame();
    }

    //Note: The buttons work but would be cleaner if their functions were in
    //UIManager and then communicating with Game_Manager

    public void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }
    }

    public void RestartButton()
    {
        if(_pauseWindow.activeInHierarchy == true)
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            _pauseWindow.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            _isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if(_pauseWindow.activeInHierarchy == true)
        {
            Time.timeScale = 1;
            _pauseWindow.SetActive(false);
            _isPaused = false;
        }
    }

    public void MainMenuButton()
    {
        if(_pauseWindow.activeInHierarchy == true)
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void GameOver()
    {
        _isGameOver = true;
        //Debug.Log("GameManager::GameOver() Called");
    }
}
