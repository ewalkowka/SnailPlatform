using UnityEngine;

/// <summary>
/// EW: Klasa obsługuje trigger collider dodany do bossa, który wykrywa gdy player jest w jego zasięgu
/// </summary>
public class BossCollider : MonoBehaviour
{
    public BossController Boss;

    void Start()
    {
        if(Boss== null)
            transform.parent.GetComponent<BossController>();
    }

    /// <summary>
    /// Sprawdza czy gracz jest w triggerze, zapobiega atakowaniu gracza gdy boss jest tyłem do niego
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        PlayerController player = PlayerController.Instance;

        if (other.transform == player.transform)
        {
            if (Boss.MoveRight == true && Boss.transform.position.x >= player.transform.position.x - 1f)
                return;

            else if (Boss.MoveRight == false && Boss.transform.position.x <= player.transform.position.x + 1f)
                return;

                Boss.IsPlayerOnTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform==PlayerController.Instance.transform)
            Boss.IsPlayerOnTrigger = false;
    }
}
