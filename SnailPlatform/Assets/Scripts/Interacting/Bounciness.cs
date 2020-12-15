using UnityEngine;

/// <summary>
/// EW: Klasa umożliwiająca odbicie się gracza po kontakcie z fizycznym materiałem
/// </summary>
public class Bounciness : MonoBehaviour
{
    public PhysicMaterial BouncyMat;
    public PhysicMaterial SlideMat;
    /// <summary>
    /// Dodawanie velocity, do edytowania w inspektorze
    /// </summary>
    public Vector3 PlayerVelocity = new Vector3(5f, 15f, 0f);


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform==PlayerController.Instance.Rigbody.transform)
        {
            CapsuleCollider coll = PlayerController.Instance.GetComponent<CapsuleCollider>();
            coll.material = BouncyMat;
            PlayerController.Instance.Rigbody.velocity = PlayerVelocity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CapsuleCollider coll = PlayerController.Instance.GetComponent<CapsuleCollider>();
        coll.material = SlideMat; //powrót do domyślnego materiału
    }
}
