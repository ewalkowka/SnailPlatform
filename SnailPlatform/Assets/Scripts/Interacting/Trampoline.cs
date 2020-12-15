using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EW: Klasa aktywuje obiekt wyłaniający się po aktywacji obiektu z klasą Pressable
/// </summary>
public class Trampoline : MonoBehaviour
{
    public Pressable Pressable;
    public Vector3 targetPosition = new Vector3(1.45f, 1.3f, 0f); //Pozycja docelowa obiektu, można idywidualnie ją dopasować w inspektorze

    private Vector3 initialPosition;
    private Vector3 currentPosition;

    void Start()
    {
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if(Pressable.Pressed==true)
        {
            currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 11f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 11f);
        }
    }

    /// <summary>
    /// Gdy gracz staje w miejscu "trampoliny"
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            Vector3 v = other.attachedRigidbody.velocity;
            other.attachedRigidbody.velocity = new Vector3(150f, 30f, v.z);
        }
    }
}
