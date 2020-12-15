using UnityEngine;

/// <summary>
/// EW: Klasa zbieranych elementów, dodających punkty
/// </summary>
public class Collectable : MonoBehaviour
{
    public float RotationSpeed = 20f;
    public GameObject CollectableObject; 
    public GameObject Tourus; // okrąg wokół obiektu
    
    private float rotation = 10f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        rotation += Time.deltaTime * RotationSpeed;
        Tourus.transform.rotation = Quaternion.Euler(rotation, 0, 0);
        CollectableObject.transform.position = new Vector3(initialPosition.x, initialPosition.y + Mathf.Sin(Time.time * 3) * 0.3f, initialPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == PlayerController.Instance.transform)
        {
            GameLogics.Instance.Score += 20;
            Destroy(CollectableObject);
        }
    }

}
