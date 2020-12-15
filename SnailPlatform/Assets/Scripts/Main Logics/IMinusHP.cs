/// <summary>
/// EW: Interfejs, dziedziczą z niego przeciwnicy i pułapki. Odejmowanie hp graczowi
/// </summary>
public interface IMinusHP
{
    /// <summary>
    /// Odejmowanie HP gracza
    /// </summary>
    /// <param name="hp"> Damage zadany graczowi </param>
    void PlayerHpDamage(int hp);
}
