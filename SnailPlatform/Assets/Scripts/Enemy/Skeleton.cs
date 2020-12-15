using UnityEngine;
/// <summary>
/// EW: Klasa przeciwnika (kościotrupa). Poruszanie się, omijanie przeszkód, atakowanie gracza, logika przeciwnika
/// </summary>
public partial class Skeleton : MonoBehaviour, IMinusHP
{
    public float Speed = 1f;
    public float Distance = 1f; //raycast distance, sprawdza czy przed obiektem jest ściana
    public float WallCheckDistance = 1f; //raycast do sprawdzania czy na wysokosci glowy jest pustka czy dalej wysoka sciana
    public float RotSpeed = 1f;
    public bool IsJumping = false;
    public BoxCastVisualization BoxCastVis; //komponent pomocniczy do raycastowania
    public float GroundDetectorDistance = 1f; //do sprawdzania czy obiekt jest na ziemi
    public SkeletonAttack SkeletonAttack;
    public Vector3 MoveDir;
    public Rigidbody rigbody;

    private bool moveRight = true;
    private int groundMask;
    new private Animator animation;
    private string playedAnimation;
    public bool HoldAnimation = false; //wstrzymuje wynonanie animacji Idle i Walk
    private bool onGround = true;
    private Quaternion toRightRotation = Quaternion.Euler(0, 90, 0);
    private Quaternion toLeftRotation = Quaternion.Euler(0, 270, 0);

    void Start()
    {
        rigbody = GetComponent<Rigidbody>();
        BoxCastVis = GetComponentInChildren<BoxCastVisualization>();
        groundMask = LayerMask.GetMask("Ground");
        hpBarInitialScale = HPbar.transform.localScale;
        hpBarInitialPosition = HPbar.transform.position;
        hpBarInitialRotation = HPbar.transform.rotation;
        animation = GetComponent<Animator>();
        SkeletonAttack = GetComponentInChildren<SkeletonAttack>();
    }

    void FixedUpdate()
    {
        if (IsSkeletonDead()) //jeśli postać jest martwa, przerwij update
        {
            Vector3 veloDead = new Vector3(0, rigbody.velocity.y, 0);

            if (IsSkeletonDead())
                rigbody.velocity = veloDead;

            return;
        }

        if (HoldAnimation)
        {
            AnimatorStateInfo animatorStateInfo = animation.GetCurrentAnimatorStateInfo(0);

            if (animatorStateInfo.loop == false) //sprawdza czy animacja sie skonczyla
                if (animatorStateInfo.normalizedTime > 0.99f)
                    if (animation.IsInTransition(0) == false)
                    {
                        HoldAnimation = false;
                    }
        }

        if (SkeletonAttack.StopMovement == false)
        {
            Movement();
        }

        HPBarSetScaleAndPosition();
    }

    /// <summary>
    /// Poruszanie się postaci
    /// </summary>
    void Movement()
    {
        if (!Physics.Raycast(transform.position + MoveDir * 1.2f + transform.up, Vector3.down, Distance, groundMask))//sprawdza czy mie ma sciany w kierunku, w ktorym idzie, od popziomu korpusu
        {
            if (!Physics.Raycast(transform.position + MoveDir * 1.2f + transform.up, Vector3.down, Distance * 2.5f, groundMask)) // sprawdza czy nie ma ziemi nizej, czy może zejść z platformy
            {
                moveRight = !moveRight;
            }
        }

        //Sprawdza czy jest na ziemi
        if (Physics.Raycast(transform.position + transform.up * 0.1f, Vector3.down, GroundDetectorDistance, groundMask))
        {
            if (onGround == false)
            {
                onGround = true;
                HoldAnimation = false;
            }
        }
        else
            onGround = false;


        if (moveRight == true)
            rigbody.MoveRotation(Quaternion.Lerp(transform.rotation, toRightRotation, Time.deltaTime * RotSpeed));
        else
            rigbody.MoveRotation(Quaternion.Lerp(transform.rotation, toLeftRotation, Time.deltaTime * RotSpeed));


        if (onGround)
        {
            if (BoxCastVis.BoxCast) // Wykrycie ze przed postacią stoi sciana
            {
                // Sprawdzanie czy na wysokosci glowy jest pustka czy dalej wysoka sciana
                if (Physics.Raycast(transform.position + MoveDir * 0.1f + transform.up * 6f, MoveDir, WallCheckDistance, groundMask))// Uderzył w ścianę czyli musi zawrocic
                {
                    if (onGround == true)
                    {
                        moveRight = !moveRight;
                    }
                }
                else // Przeskocz nad niską ścianą
                {
                    JumpOverWalls();
                }
            }
        }

        if (moveRight == true)
        {
            MoveDir = Vector3.right;
        }
        else
        {
            MoveDir = Vector3.left;
        }

        //dostosowanie pomocniczego komponentu i puszczenie raycastu
        BoxCastVis.direction = MoveDir;
        BoxCastVis.Cast();


        if (!HoldAnimation && onGround == true)
        {
            if (SkeletonAttack.IsChasingPlayer) return; //logika scigania w pliku SkeletonAttack
            PlayAnimation("Walk");
        }


        Vector3 veloAlive = transform.forward * Speed; //proste patrolowanie
        veloAlive.y = rigbody.velocity.y;

        rigbody.velocity = veloAlive;
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
    /// Metoda umożliwiająca wskakiwanie na przeszkodę
    /// </summary>
    public void JumpOverWalls()
    {
        RaycastHit hit;
        Vector3 castOrigin = BoxCastVis.Hit.point; //punkt uderzenia w ścianę przed szkieletem
        castOrigin.y = transform.position.y + 6f; //wysokość postaci
        Physics.Raycast(castOrigin, Vector3.down, out hit, 5.9f, groundMask, QueryTriggerInteraction.Ignore); //sprawdza jak  wysoka jest przeszkoda

        float velocityValue = 10f;
        if (hit.transform)
        {
            // factor = moc wyskoku
            // Jesli dystans lotu raycasta = 5.9 lub wiecej to factor = 0
            // Jesli dystans = 0 to factor = 1
            // Jesli ray leci dluzej to znaczy ze przeszkoda jest niska
            float jumpFactor = Mathf.InverseLerp(5.9f, 0f, hit.distance);
            velocityValue = Mathf.Lerp(7f, 17f, jumpFactor);
        }

        PlayAnimation("Jump", 0.1f);
        HoldAnimation = true;
        rigbody.velocity = new Vector3(MoveDir.x, velocityValue, 0f);
    }

    /// <summary>
    /// Metoda rysująca raycasty do sprawdzania czy postać może iść dalej, czy jest przeszkoda na drodze. Jeśli jest, to czy ma grunt pod sobą
    ///</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawRay(transform.position + MoveDir + transform.up, Vector3.down * Distance);

        Gizmos.color = new Color(0, 0, 1);
        Gizmos.DrawRay(transform.position + MoveDir * 0.1f + transform.up * 6f, MoveDir * WallCheckDistance);

        Gizmos.color = new Color(2, 1, 0);
        Gizmos.DrawRay(transform.position + MoveDir * 1.2f + transform.up, Vector3.down * Distance * 2.5f);

        Gizmos.color = new Color(2, 5, 1);
        Gizmos.DrawRay(transform.position - transform.up * 0.1f, Vector3.down * GroundDetectorDistance);
    }


    /// <summary>
    /// Zmiana animacji
    /// </summary>
    /// <param name="name">Nazwa animacji</param>
    /// <param name="duration">Czas zmiany jednej animacji na drugą</param>
    public void PlayAnimation(string name, float duration = 0.2f)
    {
        if (playedAnimation == name) return;
        if (HoldAnimation == true) return;

        animation.CrossFade(name, duration);
        playedAnimation = name;
    }
}
 