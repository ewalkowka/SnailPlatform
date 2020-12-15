using UnityEngine;

/// <summary>
/// EW: Klasa przeciwnika, który sprawdza czy gracz jest w pobliżu za pomocą raycasta
/// </summary>
public partial class PatrollingEnemyTest : MonoBehaviour, IMinusHP
{
    public float Speed = 1f;
    public float MovementFrequency = 3f;
    public float Stopwatch = 0f;
    public float Amplitude = 1f;

    private bool moveRight = true;
    private Vector3 initialPosition;
    private Rigidbody rigbody;

    void Start()
    {
        initialPosition = transform.position;
        playerMask = LayerMask.GetMask("Slimag");
        rigbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Physics.Linecast(transform.position, GetPatrolRayPoint(), playerMask))
        {
            followDuration = 3f;
        }

        if(followDuration > 0)
        {
            ChasePlayer();
        }
        else
        {
            Movement();
        }

        followDuration -= Time.deltaTime;
        Stopwatch += 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform==PlayerController.Instance.transform)
        {
            PlayerHpDamage(30);

            if(PlayerController.Instance.transform.position.x>rigbody.transform.position.x)
            {
                PlayerController.Instance.Rigbody.velocity = new Vector3(3, 10, 0);

            }
            else
            {
                PlayerController.Instance.Rigbody.velocity = new Vector3(-3, 10, 0);
            }
        }
    }

    /// <summary>
    /// Ruch przeciwnika
    /// </summary>
    private void Movement()
    {
        if(moveRight==true)
        {
            Vector3 newPos = rigbody.transform.position + (Vector3.right * Time.deltaTime * Speed);
            newPos.y += (Mathf.Sin(Time.time * Speed) * Amplitude) * Time.deltaTime;
            rigbody.MovePosition(newPos);

            if (Stopwatch>=MovementFrequency)
            {
                moveRight = false;
                Stopwatch = 0f;
            }
        }
        else
        {
            Vector3 newPos2 = rigbody.transform.position + (Vector3.left * Time.deltaTime * Speed);
            newPos2.y += (Mathf.Sin(Time.time * Speed) * Amplitude) * Time.deltaTime;
            rigbody.MovePosition(newPos2);

            if (Stopwatch >= MovementFrequency)
            {
                moveRight = true;
                Stopwatch = 0f;
            }
        }

        rigbody.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Zaimplementowany interfejs IMinusHP
    /// </summary>
    /// <param name="hp"> Damage zadany graczowi </param>
    public void PlayerHpDamage(int hp)
    {
        GameLogics.Instance.HP -= hp;
    }
}
