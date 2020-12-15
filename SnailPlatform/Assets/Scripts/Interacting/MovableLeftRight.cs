using UnityEngine;

/// <summary>
/// EW: Klasa dla platform poruszających się w prawo i lewo
/// </summary>
public class MovableLeftRight : MonoBehaviour
{
    public Vector3 Left = Vector3.left;
    public Vector3 Right = Vector3.right;
    public float PlatformSpeed = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool toTheRight = true;
    private float progress = 0f;

    /// <summary>
    /// Określa dwa punkty krańcowe w ruchu platformy
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return; 
        Gizmos.DrawSphere(transform.position + Left, 0.1f);
        Gizmos.DrawSphere(transform.position + Right, 0.1f);
    }

    void Start()
    {
        startPosition = transform.position + Left;
        targetPosition = transform.position + Right;
    }

    void Update()
    {
        if (progress >= 1) toTheRight = false;
        if (progress <= 0) toTheRight = true;

        if (toTheRight == true)
        {
            progress += Time.deltaTime * PlatformSpeed;
        }
        else
        {
            progress -= Time.deltaTime * PlatformSpeed;
        }

        transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

    }

}
