using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float boostForce = 5f;
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 10f;

    public AudioClip boostSound;
    public TextMeshProUGUI fuelText;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool boosting = false;
    private float currentFuel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        currentFuel = maxFuel;
    }

    private void Update()
    {
        // Boost the player upwards when Space is held
        if (Input.GetKey(KeyCode.Space) && currentFuel > 0f)
        {
            rb.AddForce(Vector2.up * boostForce, ForceMode2D.Force);
            boosting = true;
            PlayBoostSound();
        }
        else
        {
            boosting = false;
        }

        // Consume fuel while boosting
        if (boosting)
        {
            ConsumeFuel(fuelConsumptionRate * Time.deltaTime);
        }

        // Update fuel UI
        UpdateFuelUI();
    }

    private void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        if (currentFuel <= 0f)
        {
            currentFuel = 0f;
        }
    }

    private void UpdateFuelUI()
    {
        float fuelPercentage = currentFuel / maxFuel;

        // Update the fuel text based on remaining fuel percentage
        if (fuelPercentage > 0f)
        {
            int fuelTextIndex = Mathf.CeilToInt(fuelPercentage * 10f);
            fuelText.text = (fuelTextIndex * 10).ToString();
        }
        else
        {
            fuelText.text = "Empty";
        }
    }


    private void PlayBoostSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = boostSound;
            audioSource.Play();
        }
    }
}
