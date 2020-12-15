using UnityEngine;

/// <summary>
/// EW: Klasa do wyświetlania tekstu tutoriali w trakcie gry
/// </summary>
public class TutorialUI : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public float Distance = 1f;

    private float targetAlpha = 1f;

    void Start()
    {
        CanvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (Vector3.Distance(CanvasGroup.transform.position, PlayerController.Instance.transform.position) < Distance)
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, targetAlpha, Time.deltaTime); //Gdy player jest w poblliżu, tekst zaczyna się odsłaniać
    }
}
