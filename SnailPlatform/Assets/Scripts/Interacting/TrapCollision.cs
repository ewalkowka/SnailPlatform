using UnityEngine;

/// <summary>
/// EW: Po wejściu w trigger obiektu gracz traci życie
/// </summary>
public class TrapCollision : MonoBehaviour, IMinusHP
{
    /// <summary>
    /// Zaimplementowany interfejs IMinusHP
    /// </summary>
    /// <param name="hp"> Damage zadany graczowi </param>
    public void PlayerHpDamage(int hp)
    {
        GameLogics.Instance.HP -= hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform==PlayerController.Instance.Rigbody.transform)
        {
            PlayerHpDamage(100);
        }
    }
}
