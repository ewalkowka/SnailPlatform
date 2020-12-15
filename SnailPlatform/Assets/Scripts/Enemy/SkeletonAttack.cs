using System.Collections;
using UnityEngine;

/// <summary>
/// EW: Klasa ataku postaci, logika ruchu gdy gracz znajdzie się w triggerze postaci
/// </summary>
public class SkeletonAttack : MonoBehaviour
{
    public Skeleton Skeleton;
    public bool StopMovement = false;
    /// <summary>
    /// Zmienna ustawiana w inspektorze na podstawie wizualizacji kuli w OnDrawGizmosSelected()
    /// </summary>
    public float DistanceDetection = 15f;
    public bool IsAttacking = false;
    public bool IsChasingPlayer = false;

    private float initialSpeed;
    private float translationSpeed;

    void Start()
    {
        Skeleton = GetComponentInParent<Skeleton>();
        initialSpeed = Skeleton.Speed;
        translationSpeed = Skeleton.Speed * 1.5f;
    }

    /// <summary>
    /// Gdy gracz jest w triggerze, postać przyspiesza i atakuje
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (Skeleton.IsSkeletonDead())
            return;


        if (other.transform == PlayerController.Instance.transform)
        {
            if (!StopMovement)
            {
                Skeleton.Speed = translationSpeed;

                Skeleton.PlayAnimation("Run", 0.1f);
                IsChasingPlayer = true;

                if (Vector3.Distance(PlayerController.Instance.transform.position, Skeleton.transform.position) <= DistanceDetection)
                {
                    if (IsAttacking == false)
                    {
                        StartCoroutine(IAttack());
                    }
                }

                Vector3 velo = (transform.forward * Skeleton.Speed);
                velo.y = Skeleton.rigbody.velocity.y;
                Skeleton.rigbody.velocity = velo;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == PlayerController.Instance.transform)
        {
            Skeleton.HoldAnimation = false;
            Skeleton.Speed = initialSpeed;
            StopMovement = false;
            IsChasingPlayer = false;
            IsAttacking = false;
        }
    }

    /// <summary>
    /// Atakowanie gracza
    /// </summary>
    private IEnumerator IAttack()
    {
        if (Skeleton.IsSkeletonDead())
            yield break;

        StopMovement = true;
        Skeleton.HoldAnimation = false;
        Skeleton.PlayAnimation("Attack", 0.2f);
        Skeleton.HoldAnimation = true;
        Skeleton.rigbody.velocity = new Vector3(0, 0, 0);
        IsAttacking = true;
        IsChasingPlayer = false;
        yield return new WaitForSeconds(0.5f);
        Skeleton.PlayerHpDamage(Random.Range(10, 15));
        yield return new WaitForSeconds(0.5f);
        StopMovement = false;
    }

    /// <summary>
    /// Kula określająca dystans
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Skeleton.transform.position, DistanceDetection);
    }
}