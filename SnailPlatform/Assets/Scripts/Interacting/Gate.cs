using UnityEngine;

/// <summary>
/// Klasa bramy otwierającej się gdy gracz zabije przeciwnika 
/// </summary>
public class Gate : MonoBehaviour
{
    /// <summary> Do ustawienia w inspektorze </summary>
    public Vector3 TargetPosition = Vector3.zero;
    /// <summary>Przeciwnik, którego gracz musi zabić </summary>
    public Skeleton Skeleton;
    public float Speed = 0.1f;

    private Vector3 initialPosition;
    private Rigidbody rigbody;
    private float elapsed = 0f;

    void Start()
    {
        initialPosition = transform.position;
        rigbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(Skeleton.IsSkeletonDead() == true)
        {
            elapsed += Time.deltaTime;

            rigbody.MovePosition(Vector3.Lerp(initialPosition, TargetPosition, elapsed * Speed));
        }
    }
}
