using System.Collections;

/// <summary>
/// EW: Zawiera metody zawierające poszczególne etapy gry
/// </summary>
public interface IRespawnable
{
    IEnumerator Respawn();

    IEnumerator GameOver();

    IEnumerator NewGame();
}
