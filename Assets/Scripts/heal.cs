using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healing : MonoBehaviour
{
    AudioSource audioData;
    public AudioClip spawning;
    public AudioClip looting;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = spawning;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        audioSource.clip = looting;
        audioSource.Play();
        Destroy(gameObject);
    }
}
