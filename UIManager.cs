using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _resetText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImg;

    public Text _scoreText, _bestText;
    public int score, bestScore;

    [SerializeField]
    private Game_Manager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + score;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestText.text = "Best: " + bestScore;

        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<Game_Manager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
            _bestText.text = "Best: " + bestScore.ToString();
            //Debug.Log("UIManager::CheckforBestScore() Called");
            Debug.Log(PlayerPrefs.GetInt("BestScore", bestScore));
        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }

    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(_gameOverText.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

    public void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _resetText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }
}
