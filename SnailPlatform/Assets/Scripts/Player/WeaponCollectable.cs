using UnityEngine;

/// <summary>
/// EW: Klasa elementu zbieranego przez gracza, umożliwiającego graczowi użycie broni
/// </summary>
public class WeaponCollectable : MonoBehaviour
{
    public GameObject Particle;
    public float Amplitude = 1f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * 5) * Amplitude, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == PlayerController.Instance.transform)
        {
            GameObject particle = Instantiate(Particle, transform.position, transform.rotation);
            Destroy(particle, 0.5f);
            GameLogics.Instance.HasWeapon = true;
            Destroy(gameObject);
        }
    }
}
