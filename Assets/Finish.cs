using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Finish : MonoBehaviour
{
    public AudioClip boostSound;
    private AudioSource audioSource;
    public TextMeshProUGUI won;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.clip = boostSound;
            audioSource.Play();
            Plane plane = other.gameObject.GetComponent<Plane>();
            plane.IncreaseFuel(100f);
            StartCoroutine(End());
        }
    }

    private IEnumerator End()
    {
        won.gameObject.SetActive(true); // Enable the TextMeshProUGUI gameObject
        yield return new WaitForSeconds(5f);
        won.gameObject.SetActive(false); // Enable the TextMeshProUGUI gameObject
        SceneManager.LoadScene(0);
    }
}
