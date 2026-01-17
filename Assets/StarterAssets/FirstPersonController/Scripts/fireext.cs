using UnityEngine;
using UnityEngine.InputSystem; // Required for New Input System

public class FireExtinguisher : MonoBehaviour
{
    [Header("Settings")]
    public ParticleSystem foamParticles;
    public float sprayForce = 15f; // Optional: Push objects back with foam

    // We track if the particles are currently playing to avoid spamming Play()
    private bool isSpraying = false;

    void Update()
    {
        // 1. CHECK PARENT: If parent is null, we aren't holding it.
        // The PickupSystem sets the parent to the Camera when held.
        if (transform.parent == null) 
        {
            StopSpray();
            return;
        }

        // 2. CHECK INPUT: Left Mouse Button (New Input System)
        // We ensure Mouse.current isn't null to prevent errors
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            StartSpray();
        }
        else
        {
            StopSpray();
        }
    }

    void StartSpray()
    {
        if (!isSpraying)
        {
            foamParticles.Play();
            isSpraying = true;
        }
        
        // Optional: Logic to extinguish fire or push objects goes here
    }

    void StopSpray()
    {
        if (isSpraying)
        {
            foamParticles.Stop();
            isSpraying = false;
        }
    }
}