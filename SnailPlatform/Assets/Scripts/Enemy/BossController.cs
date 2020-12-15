using System.Collections;
using UnityEngine;

/// <summary>
/// EW: Klasa bossa; ruch i logika bossa
/// </summary>
public class BossController : MonoBehaviour, IMinusHP
{
    public float Distance = 1f;
    public float MoveSpeed = 1f;
    public float RotSpeed = 1f;
    public GameObject Projectile;
    public bool IsPlayerOnTrigger = false;
    public GameObject Boss;
    public GameObject BossHPBar;
    public GameObject Particle;
    public bool MoveRight = true;
    public bool StopMoving = false;

    private int groundMask;
    private Rigidbody rigbody;
    private int bossHP = 200;
    /// <summary> Zapobiega zapętlaniu się animacji </summary>
    private float stopwatch = 2f;
    /// <summary> Mnożnik prędkości przemieszczania </summary>
    private float acceleration = 1f;
    private bool isCoroutineWorking = false;
    new private Animator animation = new Animator();
    private string playedAnimation = "";
    private bool holdAnimation = false;
    private bool isOnGround = true;
    private Vector3 initialPosition;
    private Vector3 hpBarInitialScale;
    private Vector3 hpBarInitialPosition;
    private Quaternion hpBarInitialRotation;

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        rigbody = GetComponent<Rigidbody>();
        animation = GetComponent<Animator>();
        StartCoroutine(IJumping());
        initialPosition = transform.position;
        hpBarInitialScale = BossHPBar.transform.localScale;
        hpBarInitialPosition = BossHPBar.transform.position;
        hpBarInitialRotation = BossHPBar.transform.rotation;
    }

    void FixedUpdate()
    {
        if (IsBossDead() == true)
            return;


        if (IsPlayerOnTrigger == true && stopwatch < 0f)
        {
            stopwatch = 2f;
            if (isCoroutineWorking == false)
                StartCoroutine(IWaiting(0.7f));
        }
        else
        {
            Movement();
        }

        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, Vector3.down, 2f, groundMask))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }

        HPBarSetScaleAndPosition();
    }

    private void Update()
    {
        stopwatch -= Time.deltaTime;
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
    /// Jeśli gracz dotknie bossa, zostaje "odepchnięty" oraz traci 50 HP
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == PlayerController.Instance.transform && IsBossDead() == false)
        {
            PlayerHpDamage(50);

            if (GameLogics.Instance.HP > 0)
                PlayerController.Instance.Rigbody.velocity = new Vector3(0f, 5f, 0f);
        }
    }

    /// <summary>
    /// Metoda umożliwia przemieszczanie się obiektu.
    /// </summary>
    private void Movement()
    {
        Quaternion toRightRotation = Quaternion.Euler(0, 90, 0);
        Quaternion toLeftRotation = Quaternion.Euler(0, 270, 0);

        Vector3 moveDir = Vector3.right;
        if (MoveRight == false) moveDir = Vector3.left;

        //Wykrywanie czy boss stoi na platformie za pomocą raycasta, zmiana kierunku ruchu
        if (!Physics.Raycast((transform.position + moveDir * 2.5f + transform.up), Vector3.down, Distance, groundMask))
        {
            MoveRight = !MoveRight;
        }

        //Rotacja bossa
        if (MoveRight == true)
            rigbody.MoveRotation(Quaternion.Lerp(transform.rotation, toRightRotation, Time.deltaTime * RotSpeed));
        else
            rigbody.MoveRotation(Quaternion.Lerp(transform.rotation, toLeftRotation, Time.deltaTime * RotSpeed));

        //Animacje podczas poruszania się
        if (isOnGround == true)
        {
            if (StopMoving)
            {
                acceleration = Mathf.Lerp(acceleration, 0f, Time.deltaTime * 8f);
                if (!holdAnimation)
                    PlayAnimation("Idle");
            }
            else
            {
                acceleration = Mathf.Lerp(acceleration, 1f, Time.deltaTime * 8f);
                if (!holdAnimation)
                    PlayAnimation("Walk");
            }
        }
        else
        {
            PlayAnimation("Fly", 0.25f);

        }

        rigbody.MovePosition(transform.position + transform.forward * MoveSpeed * acceleration * Time.deltaTime);

    }

    /// <summary>
    /// Przełączanie animacji na atak
    /// </summary>
    /// <param name="seconds"> okresla na ile sekund ma być wstrzymana metoda</param>
    /// <returns></returns>
    IEnumerator IWaiting(float seconds)
    {
        isCoroutineWorking = true;
        yield return new WaitForSeconds(seconds);

        StopMoving = true;

        PlayAnimation("Attack", 0.25f);
        holdAnimation = true;

        yield return new WaitForSeconds(0.5f);
        holdAnimation = false;
        yield return new WaitForSeconds(0.27f);

        StopMoving = false;
        isCoroutineWorking = false;
    }

    /// <summary>
    /// Tworzy pocisk, aby zaatakować gracza
    /// </summary>
    public void CreateShot()
    {
        GameObject newProjectile = Instantiate(Projectile);
        newProjectile.transform.position = transform.position + transform.TransformVector(0, 3, 3);
    }

    
    /// <summary>
    /// Animacja skoku wykonywana co 3 sekundy
    /// </summary>
    IEnumerator IJumping()
    {
        while (IsBossDead() == false)
        {
            yield return new WaitForSeconds(3f);
            if (IsBossDead() == false)
                rigbody.velocity = new Vector3(0, 13f, 0) + transform.forward * 0.5f;
            else
                yield break;
        }
    }

    /// <summary>
    /// Przełączanie animacji
    /// </summary>
    /// <param name="name"> Nazwa animacji </param>
    /// <param name="duration"> Czas przejścia z jednej animacji do drugiej</param>
    void PlayAnimation(string name, float duration = 0.3f)
    {
        if (playedAnimation == name)
            return;

        animation.CrossFade(name, duration);
        playedAnimation = name;
    }

    /// <summary>
    /// Sprawdza czy został uderzony bronią ślimaka (gracza)
    /// </summary>
    /// <param name="other">Player</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerWeapon>())
        {
            bossHP -= Random.Range(1, 5);

            if (IsBossDead())
            {
                BossDead();
            }
        }
    }

    /// <summary>
    /// Sprawdza czy boss został pokonany
    /// </summary>
    private bool IsBossDead()
    {
        if (bossHP <= 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Metoda ustala pozycję paska życia oraz skalę w zależności od HP bossa
    /// </summary>
    private void HPBarSetScaleAndPosition()
    {
        Vector3 currentScale = BossHPBar.transform.localScale;
        currentScale.x = hpBarInitialScale.x * (bossHP / 100f);
        BossHPBar.transform.localScale = currentScale;

        Vector3 currentPosition = BossHPBar.transform.position;
        currentPosition = transform.position + transform.up * 10f;
        currentPosition.z = hpBarInitialPosition.z;
        BossHPBar.transform.position = currentPosition;

        BossHPBar.transform.rotation = hpBarInitialRotation;
    }

    /// <summary>
    /// Śmierć bossa
    /// </summary>
    private void BossDead()
    {
        StartCoroutine(PlayerController.Instance.IFinishLevel(Particle));
        GameObject boss = Boss;
        PlayAnimation("Die", 1f);
        holdAnimation = true;
    }
}
