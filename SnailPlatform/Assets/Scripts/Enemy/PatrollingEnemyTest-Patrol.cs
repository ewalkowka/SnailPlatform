using UnityEngine;

/// <summary>
/// EW
/// </summary>
public partial class PatrollingEnemyTest : MonoBehaviour
{
    public float Distance = 1f;

    private int playerMask;
    private float followDuration = 0f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, GetPatrolRayPoint());
    }

    /// <summary>
    /// Metoda zwraca punkt patrolowy obiektu
    /// </summary>
    private Vector3 GetPatrolRayPoint()
    {
        float sin = Mathf.Sin(Time.time * Speed);
        float patrolpointY = transform.position.y - Mathf.Abs(sin) * Distance;
        float cos = Mathf.Cos(Time.time * Speed);
        float patrolpointX = transform.position.x + cos * Distance;

        return new Vector3(patrolpointX, patrolpointY, transform.position.z);
    }

    /// <summary>
    /// Obiekt śledzi gracza aż osiągnie jego pozycję
    /// </summary>
    private void ChasePlayer()
    {
        transform.rotation = Quaternion.LookRotation(PlayerController.Instance.transform.position - transform.position);
        transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position, Time.deltaTime);
    }
}
