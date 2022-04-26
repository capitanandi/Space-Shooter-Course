using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _speedBoostPrefab;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] audioClips; //0=LaserShot, 1=Explosion, 2=PowerUp

    private Vector3 _offset;
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int score;
    [SerializeField]
    private int bestScore;
    private Spawn_Manager _spawnManager;
    private UIManager _uiManager;
    private Game_Manager _gameManager;

    [SerializeField]
    private bool _tripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldActive = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _offset = new Vector3(0, 0.94f, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<Spawn_Manager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<Game_Manager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL.");
        }

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _gameManager._isPaused == false)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);        

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 5.7f), 0);

        if(transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if(_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }

        _audioSource.PlayOneShot(audioClips[0]);
    }

    public void DamagePlayer()
    {
        if(_shieldActive == true)
        {
            _shieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);
        _audioSource.PlayOneShot(audioClips[1]);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }

        else if(_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        if(_lives < 1)
        {
            _gameManager._isGameOver = true;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.OnPlayerDeath();
            AudioSource.PlayClipAtPoint(audioClips[1], transform.position);
            Destroy(this.gameObject, 0.2f);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        _audioSource.PlayOneShot(audioClips[2]);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while(_tripleShotActive == true)
        {
            yield return new WaitForSeconds(5f);
            _tripleShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedMultiplier;
        _audioSource.PlayOneShot(audioClips[2]);
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while(_speedBoostActive == true)
        {
            yield return new WaitForSeconds(5f);
            _speedBoostActive = false;
            _speed /= _speedMultiplier;
        }
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldVisual.SetActive(true);
        _audioSource.PlayOneShot(audioClips[2]);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        while(_shieldActive == true)
        {
            yield return new WaitForSeconds(5f);
            _shieldActive = false;
            _shieldVisual.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        _uiManager.UpdateScore(score);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy_Laser")
        {
            DamagePlayer();
        }
    }
}
