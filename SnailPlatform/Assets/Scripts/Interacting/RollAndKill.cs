using UnityEngine;

/// <summary>
/// EW: Klasa dla obiektu zabijającego przeciwnika
/// </summary>
public class RollAndKill : MonoBehaviour
{
    private Rigidbody Rigbody;
    private Vector3 smoothVelocity;

    void Start()
    {
        Rigbody = GetComponent<Rigidbody>();
        smoothVelocity = Vector3.zero;
    }

    void Update()
    {
        smoothVelocity = Vector3.Lerp(smoothVelocity, Rigbody.velocity, Time.deltaTime * 7f);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehaviour enemy = other.transform.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            if (smoothVelocity.y < -1f) //jeśli spada
            {
                enemy.Dead = true;
            }
        }
    }
}
