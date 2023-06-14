using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Circles : MonoBehaviour
{
    public float movementDistance = 2f; // Distance to move up and down
    public float movementSpeed = 2f; // Speed of movement

    private Vector2 startPosition;
    private bool movingUp;
    private float currentDistance;
    private Rigidbody2D rb;

    public AudioClip boostSound;
    private AudioSource audioSource;
    public bool music;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        movingUp = true;
        currentDistance = 0f;
        music = true;
    }

    private void Update()
    {
        if (movingUp)
        {
            currentDistance += movementSpeed * Time.deltaTime;
            if (currentDistance >= movementDistance)
            {
                currentDistance = movementDistance;
                movingUp = false;
            }
        }
        else
        {
            currentDistance -= movementSpeed * Time.deltaTime;
            if (currentDistance <= -movementDistance)
            {
                currentDistance = -movementDistance;
                movingUp = true;
            }
        }

        Vector2 newPosition = startPosition + Vector2.up * currentDistance;
        transform.position = newPosition;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(music)
            {
                audioSource.clip = boostSound;
                audioSource.Play();
            }
            Plane plane = other.gameObject.GetComponent<Plane>();
            plane.IncreaseFuel(-100f);
        }
    }
    
}
