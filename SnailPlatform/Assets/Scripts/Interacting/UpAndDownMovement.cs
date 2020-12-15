using UnityEngine;

/// <summary>
/// EW: Klasa podpinana do childa. Sinusoidalny ruch góra-dół w lokalnej pozycji;
/// Podpięte pod child modelu Patrolling enemy.
/// </summary>
public class UpAndDownMovement : MonoBehaviour
{
    public float Speed = 1f;
    public float Amplitude = 1f;

    void Update()
    {
        MovementUpDown();
    }

    void MovementUpDown()
    {
        transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * Speed) * Amplitude, 0);
    }
}
