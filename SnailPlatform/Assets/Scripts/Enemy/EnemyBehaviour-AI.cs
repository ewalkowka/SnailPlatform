using UnityEngine;

/// <summary>
/// EW
/// </summary>
public partial class EnemyBehaviour
{
    /// <summary> Dystans, na jakim obiekt kontroluje obecność gracza </summary>
    public float Distance = 1f;

    private int playerMask;

    private void InitAI()
    {
        playerMask = LayerMask.GetMask("Slimag");
    }

    /// <summary>
    /// Przyspiesza gdy gracz jest w zasięgu raya
    /// </summary>
    private void AIUpdate()
    {
        Ray r = new Ray(transform.position + Vector3.up * 0.2f, transform.forward);

        if (Physics.Raycast(r, Distance, playerMask))
        {
            EnemySpeed = 6f;
        }
        else
        {
            EnemySpeed = 3f;
        }
    }

}
