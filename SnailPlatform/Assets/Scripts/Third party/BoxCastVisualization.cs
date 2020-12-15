using UnityEngine;

/// <summary>
/// FM: BoxCast
/// </summary>
public class BoxCastVisualization : MonoBehaviour
{
    public Vector3 centerOffset;
    public Vector3 halfExtends = new Vector3(1f, 1f, 0.1f);
    public Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask Mask;
    public bool BoxCast;
    public RaycastHit Hit;
    public bool PlayerBoxCast;

    public bool drawFinish = true;

    public Quaternion DesiredRotation;

    public void Cast()
    {
        BoxCast = Physics.BoxCast
        (
        transform.position + transform.TransformVector(centerOffset),
        Vector3.Scale(halfExtends, transform.lossyScale), 
        direction, 
        out Hit, 
        transform.rotation, 
        transform.TransformVector(direction.normalized).magnitude * maxDistance,
        Mask, QueryTriggerInteraction.Ignore
        );


    }


    private void OnDrawGizmosSelected()
    {
        Cast();

        if (Hit.transform)
        {
            Gizmos.DrawRay(Hit.point, Hit.normal);
            Gizmos.color = new Color(1f, 0.2f, 0.5f, 0.45f);
        }
        else
        {
            Gizmos.color = new Color(.1f, 1f, 0.5f, 0.5f);
        }

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(centerOffset, halfExtends * 2f);
        Gizmos.DrawRay(centerOffset, Gizmos.matrix.inverse.MultiplyVector(direction) * maxDistance);

        if (drawFinish)
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
            Gizmos.DrawWireCube(centerOffset + Gizmos.matrix.inverse.MultiplyVector(direction) * (maxDistance), halfExtends * 2f);
            Gizmos.color = new Color(1f, 1f, 1f, 0.05f);
            Gizmos.DrawLine(centerOffset + Vector3.up * halfExtends.y, centerOffset + Gizmos.matrix.inverse.MultiplyVector(direction) * maxDistance + Vector3.up * halfExtends.y);
            Gizmos.DrawLine(centerOffset + Vector3.down * halfExtends.y, centerOffset + Gizmos.matrix.inverse.MultiplyVector(direction) * maxDistance + Vector3.down * halfExtends.y);
        }
    }
}
