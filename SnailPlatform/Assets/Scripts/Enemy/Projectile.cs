using UnityEngine;

/// <summary>
/// EW: Klasa pocisku, którym Boss atakuje gracza, pocisk naprowadza na gracza
/// </summary>
public class Projectile : MonoBehaviour, IMinusHP
{
    public float Speed = 20f;
    public GameObject Particle; // Instantiate przy uderzeniu
    public GameObject ParticleAttack; //Instantiate przy powstaniu pocisku

    /// <summary>
    /// Zmienna determinująca czas, w którym ma śledzić gracza
    /// </summary>
    private float followDuration = 0f;
    private Vector3 startPosition;


    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        startPosition = transform.position;
        transform.LookAt(PlayerController.Instance.transform);
        followDuration = 1.5f;
    }

    void Update()
    {
        Attack();

        transform.position += transform.forward * Time.deltaTime * Speed;

        if (Vector3.Distance(startPosition, transform.position) > 100)
        {
            Destroy(gameObject);
        }

        followDuration -= Time.deltaTime;
    }

    /// <summary>
    /// Atakowanie gracza, śledzenie go
    /// </summary>
    public void Attack()
    {
        if (followDuration < 0) return;

        Vector3 playerPos = PlayerController.Instance.transform.position;

        GameObject particleAttack = Instantiate(ParticleAttack, transform.position, Quaternion.identity);
        Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
        Destroy(particleAttack, 1f);
    }

    /// <summary>
    /// Zaimplementowany interfejs IMinusHP
    /// </summary>
    /// <param name="hp"> Damage zadany graczowi </param>
    public void PlayerHpDamage(int hp)
    {
        GameLogics.Instance.HP -= hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == PlayerController.Instance.transform)
        {
            PlayerHpDamage(Random.Range(10, 20));
            GameObject particle = Instantiate(Particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(particle, 0.5f);
        }
    }
}
