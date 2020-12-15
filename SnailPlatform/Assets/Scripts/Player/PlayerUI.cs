using TMPro;
using UnityEngine;

/// <summary>
/// EW: Klasa wypisująca liczbę żyć, punktów oraz HP bar gracza
/// </summary>
public class PlayerUI : MonoBehaviour
{
    public GameObject HPbar;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LivesText;

    private Vector3 initialScaleHPBar;
    void Start()
    {
        initialScaleHPBar = HPbar.transform.localScale;
    }

    void Update()
    {
        SetHPBarScale();
        SetScoresAndLives();
    }

    /// <summary>
    /// Ustala rozmiar HP Bara
    /// </summary>
    private void SetHPBarScale()
    {
        Vector3 currentScale = HPbar.transform.localScale;
        currentScale.x = initialScaleHPBar.x * (GameLogics.Instance.HP / 100f);
        HPbar.transform.localScale = currentScale;
    }

    /// <summary>
    /// Wypisuje liczbę punktów i żyć gracza
    /// </summary>
    private void SetScoresAndLives()
    {
        ScoreText.text = ("SCORE: " + GameLogics.Instance.Score);
        LivesText.text = ("LIVES: " + GameLogics.Instance.Lives);
    }
}
