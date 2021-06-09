using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float rotationThrust = 1f;
    [SerializeField] private AudioClip mainThrustEngine;

    [SerializeField] private ParticleSystem mainShipBoosterParticles;
    [SerializeField] private ParticleSystem leftShipBoosterParticles;
    [SerializeField] private ParticleSystem rightShipBoosterParticles;

    // rb = Rigidbody
    private Rigidbody rb;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        // depending upon the key being pressed a method will be called
        if (Input.GetKey(KeyCode.A))
        {
            ProcessRotationLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ProcessRotationRight();
        }
        else
        {
            StopRotation();
        }
    }


    private void StartThrusting()
    {
        // adds force to the rigidbody of the ship
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        // if no audio source is playing it will play the main thrust engine audio
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainThrustEngine);
        }
        // if no particles are playing it will play the main ship booster particles
        if (!mainShipBoosterParticles.isPlaying)
        {
            mainShipBoosterParticles.Play();
        }
    }


    private void StopThrusting()
    {
        audioSource.Stop(); // stops the audio from playing
        mainShipBoosterParticles.Stop(); // stops the particles from playing
    }

    private void ProcessRotationRight()
    {
        ApplyRotation(-rotationThrust); // apply a minus rotation
        // if no left ship booster particles are playing, play the pariticles.
        if (!leftShipBoosterParticles.isPlaying)
        {
            leftShipBoosterParticles.Play();
        }
    }

    private void ProcessRotationLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightShipBoosterParticles.isPlaying)
        {
            rightShipBoosterParticles.Play();
        }
    }

    private void StopRotation()
    {
        // stops the particles from playing
        leftShipBoosterParticles.Stop();
        rightShipBoosterParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freeze rotation so the player will be able to manually rotate.
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so that the physics system can take over.
    }

}
