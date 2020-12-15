using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// EW: Kontroler gracza, ruch gracza
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Rigidbody Rigbody;
    public float Speed = 5f;
    public float JumpPower = 100f;
    public bool OnGround = false;
    /// <summary> Wykorzystane do napisu końcowego po przejściu poziomu </summary>
    public CanvasGroup CanvasGroup;

    [Header("Referencje do prefabów")]
    public GameObject Bullet;
    public GameObject ShootingParticle;


    internal Vector3 InitialPosition;
    private int jumpsAvailable = 2;


    void Start()
    {
        Rigbody = GetComponent<Rigidbody>();
        InitialPosition = Rigbody.transform.position;
        desiredRotation = transform.rotation;
    }


    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (GameLogics.Instance.PlayerDead)
            return;

        if(transform.position.y<=-120f)
        {
            SetDead();
            return;
        }

        Movement();

        if(OnGround==true)
        {
            jumpsAvailable = 2;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform!=transform)
        {
            if (other.tag == "Respawn") transform.SetParent(other.transform, true);
            OnGround = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform != transform)
        {
            if (other.tag == "platforma") transform.SetParent(null, true);
        }
    }


    /// <summary>
    /// Poruszanie się gracza
    /// </summary>
    private void Movement()
    {
        if(Input.GetKey(KeyCode.D))
        {
            desiredRotation = Quaternion.Slerp(desiredRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime*8f);
            desiredVelocity = transform.forward;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            desiredRotation = Quaternion.Slerp(desiredRotation, Quaternion.Euler(0, -90, 0), Time.deltaTime*8f);
            desiredVelocity = transform.forward;
        }
        else
            desiredVelocity = Vector3.zero;


        if (Input.GetKeyDown(KeyCode.Space) && jumpsAvailable>0)
        {
            Rigbody.velocity = new Vector3(Rigbody.velocity.x, JumpPower, Rigbody.velocity.z);
            OnGround = false;
            --jumpsAvailable;
        }

        if(Input.GetKeyDown(KeyCode.C) && GameLogics.Instance.HasWeapon == true)
        {
            Shoot();
        }
    }


    private Vector3 desiredVelocity;
    private Quaternion desiredRotation;

    private void FixedUpdate()
    {
        Rigbody.velocity = new Vector3(desiredVelocity.x * Speed, Rigbody.velocity.y, desiredVelocity.z * Speed);
        Rigbody.MoveRotation(desiredRotation);
    }


    /// <summary>
    /// Wykonuje się gdy gracz ma mniej lub 0 HP
    /// </summary>
    public void SetDead()
    {
        GameLogics.Instance.PlayerDead = true;
        Rigbody.velocity = new Vector3(-4, 5f, 0f);
        --GameLogics.Instance.Lives;
    }


    /// <summary>
    /// Gracz strzela (jeśli zebrał broń)
    /// </summary>
    public void Shoot()
    {
        Vector3 bulletPosition = transform.position + transform.forward + Vector3.up * 0.5f;

        GameObject bullet = Instantiate(Bullet, bulletPosition, transform.rotation);

        GameObject particle = Instantiate(ShootingParticle, bulletPosition, transform.rotation);

        Destroy(particle, 1f);

        StartCoroutine(IDestroy(bullet));
    }

    
    /// <summary>
    /// Usuwanie obiektów, aby zapobiec nagromadzeniu ich na scenie
    /// </summary>
    private IEnumerator IDestroy(GameObject gameObject)
    {
        if (gameObject == null)
            yield break;

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Wykonywane gdy gracz ukończy poziom
    /// </summary>
    public IEnumerator IFinishLevel(GameObject particle)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject _particle;
        _particle = Instantiate(particle, Instance.Rigbody.transform.position, Quaternion.identity);
        Rigbody.velocity = new Vector3(0, 15, 0);
        yield return new WaitForSeconds(1f);
        CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 1, Time.deltaTime);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Menu");
    }

}
