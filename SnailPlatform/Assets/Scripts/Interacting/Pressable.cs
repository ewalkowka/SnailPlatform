using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// EW: Klasa określa czy przycisk został wciśnięty i tym samym trampolina aktywowana
/// </summary>
public class Pressable : MonoBehaviour
{
    public bool Pressed = false;
    public UnityEvent OnPressed;
    public UnityEvent OnPressedUpdate;
    public UnityEvent OnPressedExit;

    private int numberEnter = 0;

    void Update()
    {
        if(numberEnter>=1)
        {
            Pressed = true;
        }
        else
        {
            Pressed = false;
        }
    }
    
    /// <summary>
    /// Aktywowanie akcji gdy obiekt jest w triggerze
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        ++numberEnter;
        if (numberEnter == 1) OnPressed.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        --numberEnter;
        if(numberEnter<=0)
        {
            OnPressedExit.Invoke();
        }
    }
}
