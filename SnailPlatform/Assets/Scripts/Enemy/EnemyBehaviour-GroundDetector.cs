using UnityEditor;
using UnityEngine;

/// <summary>
/// EW
/// </summary>
public partial class EnemyBehaviour
{
    public float ProbeLength = 4f;
    /// <summary>
    /// Sprawdza czy uda mu się przeskoczyć ścianę
    /// </summary>
    public float HeightCheckDistance = 2f;
    /// <summary>
    /// Sprawdza czy przed nim jest ściana
    /// </summary>
    public float WallCheckDistance = 1f;

    private int groundMask;
    private bool moveRight = true;

    /// <summary>
    /// Przypisanie Rigidbody i maski kolizji
    /// </summary>
    void InitDetection()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    /// <summary>
    /// Wykrywanie czy postać może dalej iść
    /// </summary>
    void DetectionUpdate()
    {
        Vector3 moveDir = Vector3.right;
        if (moveRight == false) moveDir = Vector3.left;

        Ray groundRay = new Ray(transform.position + moveDir + (Vector3.up * 0.1f), Vector3.down);
        RaycastHit ground;
        if (Physics.Raycast(groundRay, out ground, ProbeLength, groundMask) == false)
        {
            moveRight = !moveRight;
        }

        if (moveRight == true)
        {
            Quaternion rightRotation = Quaternion.Euler(0f, 90f, 0f);
            Rigbody.rotation = Quaternion.Slerp(transform.rotation, rightRotation, Time.deltaTime * rotationSpeed);
            Rigbody.MoveRotation(Rigbody.rotation);
        }
        else
        {
            Quaternion leftRotation = Quaternion.Euler(0f, 270f, 0f);
            Rigbody.rotation = Quaternion.Slerp(transform.rotation, leftRotation, Time.deltaTime * rotationSpeed);
            Rigbody.MoveRotation(Rigbody.rotation);
        }

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, moveDir, WallCheckDistance, groundMask)) //wykrywa ścianę
        {
            if (Physics.Raycast(transform.position + Vector3.up * 1.2f, moveDir, HeightCheckDistance, groundMask)) //uderza rayem w ścianę - za wysoka
            {
                moveRight = !moveRight;
            }
            else
            {
                JumpOverWalls();
            }
        }

        if (UseMovePosition)
            Rigbody.MovePosition(transform.position + (transform.forward * Time.deltaTime * EnemySpeed));
        else
        {
            Vector3 desiredVelo = transform.forward * EnemySpeed;
            Rigbody.velocity = new Vector3(desiredVelo.x, Rigbody.velocity.y, desiredVelo.z);
        }
    }

    private void JumpOverWalls()
    {
        float veloX;
        if (moveRight)
            veloX = 1f;
        else
            veloX = -1f;

        Vector3 targetVelocity = new Vector3(veloX, 8f, 0f);

        Rigbody.velocity = targetVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Ray r = new Ray(transform.position + (Vector3.up * 0.1f), Vector3.down);
        Gizmos.DrawRay(r.origin, r.direction * ProbeLength);
        Gizmos.DrawRay(transform.position + Vector3.up * 0.2f, transform.forward * Distance);

        Ray rr = new Ray(transform.position + Vector3.up, transform.forward);
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawRay(rr.origin, rr.direction * HeightCheckDistance); //ray do wykrywania czy ściana jest za wysoka

        Gizmos.color = new Color(0, 1, 0); //ray do wykrywania czy ściana przed graczem
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward * WallCheckDistance);
    }
}
