using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// EW: Klasa odpowiedzialna za logikę gry, ustawianie HP, żyć gracza
/// </summary>
public class GameLogics : MonoBehaviour, IRespawnable
{
    public static GameLogics Instance;
    public int Score = 0;
    public int Lives = 3;
    public int HP = 100;
    public bool HasWeapon = false;
    public bool PlayerDead = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (HP <= 0)
        {
            PlayerController.Instance.SetDead();

            if (Instance.Lives >= 1)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                HasWeapon = false;
                StartCoroutine(GameOver());
            }
        }

        if (Lives <= 0)
            Lives = 0;
    }

    /// <summary>
    /// Metoda do wywoływania IEnumeratora NewGame po naciśnięciu przycisku UI
    /// </summary>
    public void NewGameButton()
    {
        StartCoroutine(NewGame());
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");
    }

    public IEnumerator NewGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("PrototypeLevel");
    }


    public IEnumerator Respawn()
    {
        PlayerController.Instance.transform.position = PlayerController.Instance.InitialPosition;
        Instance.PlayerDead = false;
        HP = 100;
        yield return new WaitForSeconds(0.2f);
    }
}