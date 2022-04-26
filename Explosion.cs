using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source for Explosion is NULL.");
        }
        else
        {
            _audioSource.clip = _explosion;
        }

        _audioSource.Play();
        Destroy(this.gameObject, 2.8f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
