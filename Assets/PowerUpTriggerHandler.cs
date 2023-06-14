using UnityEngine;

public class PowerUpTriggerHandler : MonoBehaviour
{
    public float fuelIncreaseAmount = 20f; // Amount to increase the fuel
    public AudioClip boostSound;
    private AudioSource audioSource;
    private Renderer objectRenderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Get the PlaneController component from the plane GameObject
            Plane plane = other.gameObject.GetComponent<Plane>();

            if (plane != null)
            {
                audioSource.clip = boostSound;
                audioSource.Play();
                // Increase the fuel of the plane
                plane.IncreaseFuel(fuelIncreaseAmount);
                if(gameObject.name.Contains("extra"))
                    plane.fuelConsumptionRate = 17f;
                // Handle any other effects or actions related to the power-up
                Deactivate();
            }
        }
    }

    public void Activate()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
        Color color = objectRenderer.material.color;
        color.a = 2f;
        objectRenderer.material.color = color;
        //gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
        Color color = objectRenderer.material.color;
        color.a = 0.5f;
        objectRenderer.material.color = color;
        //gameObject.SetActive(false);
    }
}

