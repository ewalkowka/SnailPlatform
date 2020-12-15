using UnityEngine;

/// <summary>
/// EW: Klasa obiektu "miażdżącego" gracza
/// </summary>
public class Smashable : MonoBehaviour, IMinusHP
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
        if(other.transform==PlayerController.Instance.transform)
        {
            PlayerHpDamage(100);

            if(GameLogics.Instance.PlayerDead == false)
            {
                PlayerController.Instance.SetDead();
            }
        }
    }
}
