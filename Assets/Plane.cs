using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Plane : MonoBehaviour
{
    float timer;
    private float boostForce;
    private float maxFuel;
    public float fuelConsumptionRate;

    public AudioClip boostSound;
    public TextMeshProUGUI fuelText;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool boosting = false;
    private float currentFuel;

    public TextMeshProUGUI again;
    private bool isPlaying = false;
    public Image fuelBarImage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        boostForce = 66f;
        maxFuel = 100f;
        fuelConsumptionRate = 20f;

        currentFuel = maxFuel;
        timer = -1f;
    }

    void FixedUpdate()
    {
        if(currentFuel > 0f)
        {
            rb.velocity = new Vector2(2f, 0f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        timer -= Time.deltaTime;

        if (currentFuel == 0f)
        {
            GameObject rocksObject = GameObject.Find("Rocks");
            GameObject powerUps = GameObject.Find("Powerup");
            GameObject circles = GameObject.Find("Circles");
            

            if (rocksObject != null)
            {
                // Get all EdgeCollider2D components in children
                EdgeCollider2D[] colliders = rocksObject.GetComponentsInChildren<EdgeCollider2D>();
                BoxCollider2D[] powerup = powerUps.GetComponentsInChildren<BoxCollider2D>();
                Circles[] xatars = circles.GetComponentsInChildren<Circles>();
                // Disable all EdgeCollider2D components
                foreach (EdgeCollider2D collider in colliders)
                {
                    collider.enabled = false;
                }
                foreach (BoxCollider2D pu in powerup)
                {
                    pu.enabled = false;
                }
                foreach (Circles xatar in xatars)
                {
                    xatar.music = false;
                }

            }

            StartCoroutine(Reset(rocksObject,powerUps, circles));
        }

        // Boost the player upwards when Space is held
        if (Input.GetKey(KeyCode.Space) && currentFuel > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 45f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f);
            timer = 0.5f;
            rb.AddForce(Vector2.up * boostForce); //, ForceMode2D.Force);
            boosting = true;
            if (!isPlaying)
            {
                isPlaying = true;
                audioSource.loop = true;
                audioSource.Play();
            }
            
        }
        else
        {
            boosting = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isPlaying = false;
            audioSource.loop = false;
            audioSource.Stop();
        }

        if (rb.velocity.magnitude > 0.1f & timer < 0)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Update fuel UI
        UpdateFuelUI();
    }

    private void UpdateFuelUI()
    {
        // Consume fuel while boosting
        if (boosting)
        {
            IncreaseFuel(-fuelConsumptionRate * Time.deltaTime);
        }

        float fuelPercentage = currentFuel / maxFuel;

        // Update the fuel text based on remaining fuel percentage
        if (fuelPercentage > 0f)
        {
            int fuelTextIndex = Mathf.CeilToInt(fuelPercentage * 10f);
            fuelText.text = Mathf.Round(fuelPercentage * 100f).ToString() + "%";
            float fillAmount = currentFuel / 100f;
            Vector3 newScale = new Vector3(fillAmount, 1f, 1f);

        // Set the new scale of the fuel bar Image
            fuelBarImage.rectTransform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = new Vector3(0f, 1f, 1f);
            fuelBarImage.rectTransform.localScale = newScale;
            fuelText.text = "Empty";
        }
    }

    public void IncreaseFuel(float amount)
    {
        currentFuel += amount;
        if (currentFuel > 100)
        {
            currentFuel = 100;
        }
        if(currentFuel < 0)
        {
            currentFuel = 0;
        }
    }


    private void PlayBoostSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = boostSound;

            // Start a coroutine to repeatedly play the shortened audio clip
            StartCoroutine(PlayShortenedAudioClip());
        }
    }

    private System.Collections.IEnumerator PlayShortenedAudioClip()
    {
        float playTime = 0.1f; // Length of the shortened audio clip to play
        audioSource.time = Mathf.Min(audioSource.clip.length, playTime);
        audioSource.Play();
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator Reset(GameObject rocksObject, GameObject powerUps, GameObject circles)
    {
        // Start the Game again
        yield return new WaitForSeconds(2f);
        if(currentFuel == 0)
        {
            again.gameObject.SetActive(true); // Enable the TextMeshProUGUI gameObject

            yield return new WaitForSeconds(0.5f);

            if(currentFuel > 0)
            {
                again.gameObject.SetActive(false);
            }

            if (Input.GetKey(KeyCode.Space))
            {              
                again.gameObject.SetActive(false); // Disable the TextMeshProUGUI gameObject
                transform.position = new Vector2(-7f,1.54f);
                transform.rotation = Quaternion.identity;
                boostForce = 66f;
                currentFuel = 100f;
                fuelConsumptionRate = 20f;
                EdgeCollider2D[] colliders = rocksObject.GetComponentsInChildren<EdgeCollider2D>();
                // Disable all EdgeCollider2D components
                foreach (EdgeCollider2D collider in colliders)
                {
                    collider.enabled = true;
                }
                BoxCollider2D[] powerup = powerUps.GetComponentsInChildren<BoxCollider2D>();
                // Disable all EdgeCollider2D components
                foreach (BoxCollider2D pu in powerup)
                {
                    pu.enabled = true;
                }
                PowerUpTriggerHandler[] powerup2 = powerUps.GetComponentsInChildren<PowerUpTriggerHandler>();
                // Disable all EdgeCollider2D components
                foreach (PowerUpTriggerHandler pu in powerup2)
                {
                    pu.Activate();
                }
                Circles[] xatars = circles.GetComponentsInChildren<Circles>();
                foreach (Circles xatar in xatars)
                {
                    xatar.music = true;
                }
                
            }
        }
        else
        {
            again.gameObject.SetActive(false);
        }
    }

    
}
