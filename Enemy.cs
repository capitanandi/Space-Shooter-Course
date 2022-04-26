using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float spawnPoint;
    private Player _player;
    [SerializeField]
    private Animator _enemyDeathAnim;
    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaser;

    //Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyDeathAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if(_enemyDeathAnim == null)
        {
            Debug.LogError("Animator is NULL.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source for Enemy is NULL.");
        }
        else
        {
            _audioSource.clip = _explosion;
        }

        StartCoroutine(EnemyFire());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6.5f)
        {
            float spawnPoint = Random.Range(-9.3f, 9.3f);
            transform.position = new Vector3(spawnPoint, 9, 0);
        }
    }

    IEnumerator EnemyFire()
    {
        while (this.gameObject != null && GetComponent<BoxCollider2D>() != null && _speed != 0.0f)
        {
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayer();
            }
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
        }

         if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _audioSource.Play();

            if(_player != null)
            {
                _player.AddScore(10);
            }
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }
}
