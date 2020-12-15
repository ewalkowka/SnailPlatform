using UnityEngine;

/// <summary>
/// EW: Klasa poruszania góra-dół
/// </summary>
public class MovableUpDown : MonoBehaviour
{
    public Vector3 Up = Vector3.up;
    public Vector3 Down = Vector3.down;
    public float Speed = 1;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool toDown = true;
    private float progress = 0f;
    private Rigidbody rigBody;

    /// <summary>
    /// Rysowanie docelowych punktów ruchu obiektu
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;
        Gizmos.DrawSphere(transform.position + Up, 0.1f);
        Gizmos.DrawSphere(transform.position + Down, 0.1f);
    }

    void Start()
    {
        startPosition = transform.position + Up;
        targetPosition = transform.position + Down;

        if(GetComponent<Rigidbody>())
        {
            rigBody = GetComponent<Rigidbody>();
            FixedUpdate();
        }
        else
        {
            Update();
        }
    }

    /// <summary>
    /// Jeśli obiekt nie ma komponentu Rigidbody, wykonuje się Update. Jeśli ma - FixedUpdate
    /// </summary>
    void Update()
    {
        
        if (progress >= 1) toDown = false;
        if (progress <= 0) toDown = true;

        if (toDown == true)
        {
            progress += Time.deltaTime * Speed;
        }
        else
        {
            progress -= Time.deltaTime * Speed;
        }

        transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
    }

    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody>() == false)
            return;
        if (progress >= 1) toDown = false;
        if (progress <= 0) toDown = true;

        if (toDown == true)
        {
            progress += Time.deltaTime * Speed;
        }
        else
        {
            progress -= Time.deltaTime * Speed;
        }

        Vector3 pos = Vector3.Lerp(startPosition, targetPosition, progress);

        rigBody.MovePosition(pos); 
    }
}
