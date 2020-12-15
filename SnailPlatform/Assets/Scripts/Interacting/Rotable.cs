using UnityEngine;

/// <summary>
/// Klasa do przeszkód obracających się
/// </summary>
public class Rotable : MonoBehaviour
{
    public float RotationSpeed = -40f;

    private Rigidbody rigbody;
    private Vector3 currentRotation;

    void Start()
    {
        rigbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()  
    {
        currentRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        rigbody.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z + Time.time * RotationSpeed);

        rigbody.MoveRotation(rigbody.rotation);
    }
}
