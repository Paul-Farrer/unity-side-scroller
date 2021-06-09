using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osillator : MonoBehaviour
{

    private Vector3 startingPosition;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] [Range(0, 1)] private float movementFactor;
    [SerializeField] private float period = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // Mathf.Epsilon = smallest float (Basically same as putting == 0f (Do not do this as comparing 
        // two floats can vary by a tiny amount) but prevents errors that could possibly occur using 0f)
        if (period <= Mathf.Epsilon) // prevent the error "Input position is { NaN, NaN, NaN }." 
        {
            return; // if the value period is equal to or less than the smallest float return and don't run the code below
        }
        float cycles = Time.time / period; // continually growing over time

        const float tau = Mathf.PI * 2; // constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); //going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // recalulated to go fron 0 to 1 so its cleaner

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
