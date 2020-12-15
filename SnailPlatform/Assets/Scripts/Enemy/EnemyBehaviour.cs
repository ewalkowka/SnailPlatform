using UnityEngine;

public partial class EnemyBehaviour : MonoBehaviour, IMinusHP
{
    public Rigidbody Rigbody;
    public bool UseMovePosition = false;
    public bool Dead = false;
    public float TargetSpeed = 1f;
    public GameObject Particle;
    public float EnemySpeed = 3f;

    private Vector3 targetPos;
    private Vector3 startPos;
    private float rotationSpeed = 3f;

    void Start()
    {
        Rigbody = GetComponent<Rigidbody>();
        startPos = Rigbody.transform.position;
        InitAI();
        InitDetection();
    }

    void FixedUpdate()
    {
        if (Dead == true)
        {
            Instantiate(Particle, Rigbody.transform.position + Vector3.up * 0.4f, Particle.transform.rotation);
            Destroy(gameObject);
            return;
        }
        else
        {
            DetectionUpdate();
            AIUpdate();
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform==PlayerController.Instance.transform)
        {
            PlayerHpDamage(50);
            DamageBehaviour();
        }
    }

    /// <summary>
    /// Zaimplementowany interfejs IMinusHP
    /// </summary>
    /// <param name="hp"> Damage zadany graczowi </param>
    public void PlayerHpDamage(int hp)
    {
        GameLogics.Instance.HP -= hp;
    }

    /// <summary>
    /// Zachowanie gracza po otrzymaniu obrażeń
    /// </summary>
    public void DamageBehaviour()
    {
        if(GameLogics.Instance.HP<=0)
        {
            return;
        }

        if (PlayerController.Instance.Rigbody.transform.position.x > transform.position.x)
        {
            PlayerController.Instance.Rigbody.velocity = new Vector3(2f, 5f, PlayerController.Instance.Rigbody.velocity.z);
        }
        else
        {
            PlayerController.Instance.Rigbody.velocity = new Vector3(-2f, 5f, PlayerController.Instance.Rigbody.velocity.z);
        }
    }
}
