using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RockTrigger : MonoBehaviour
{
    public AudioClip boostSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.clip = boostSound;
            audioSource.Play();
            Plane plane = other.gameObject.GetComponent<Plane>();
            plane.IncreaseFuel(-100f);
        }
    }
}
