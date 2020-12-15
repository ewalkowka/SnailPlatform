using UnityEngine;

/// <summary>
/// EW: Klasa pocisku z broni, w którą jest wyposażony gracz po zebraniu WeaponCollectable
/// </summary>
public class PlayerWeapon : MonoBehaviour
{
    public GameObject ParticleOnCollision;

    void Update()
    {
        transform.position = transform.position + transform.forward;
    }

    /// <summary>
    /// Podczas kolizji tworzy się particle
    /// </summary>
    public void OnCollision()
    {
        GameObject particleOnCollision = Instantiate(ParticleOnCollision, transform.position, transform.rotation);
        Destroy(particleOnCollision, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        int groundMask = LayerMask.GetMask("Ground");

        if(other.gameObject.layer == groundMask)
        {
            OnCollision();
        }
    }
}
