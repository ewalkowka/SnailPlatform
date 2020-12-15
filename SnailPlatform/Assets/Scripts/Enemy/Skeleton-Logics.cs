using UnityEngine;

/// <summary>
/// EW: Logika postaci
/// </summary>
public partial class Skeleton
{
    public int HP = 100;
    public GameObject HPbar;

    private Vector3 hpBarInitialScale;
    private Vector3 hpBarInitialPosition;
    private Quaternion hpBarInitialRotation;

    /// <summary>
    /// Metoda zwraca true jeśli postać jest martwa
    /// </summary>
    /// <returns>Stan postaci</returns>
    public bool IsSkeletonDead()
    {
        if (HP <= 0f)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Ustawianie skali i pozycji paska HP postaci w zależności od jego życia
    /// </summary>
    private void HPBarSetScaleAndPosition()
    {
        Vector3 currentScale = HPbar.transform.localScale;
        currentScale.x = hpBarInitialScale.x * (HP / 100f);
        HPbar.transform.localScale = currentScale;

        Vector3 currentPosition = HPbar.transform.position;
        currentPosition = transform.position + transform.up * 10f;
        currentPosition.z = hpBarInitialPosition.z;
        HPbar.transform.position = currentPosition;

        HPbar.transform.rotation = hpBarInitialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == PlayerController.Instance.transform)//gdy gracz dotknie przeciwnika
        {
            if (IsSkeletonDead())
                return;

            PlayerHpDamage(20);
            PlayerController.Instance.Rigbody.velocity = new Vector3(0, 10, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon playerWeapon = other.transform.GetComponent<PlayerWeapon>();

        if (playerWeapon != null) //pocisk gracza trafił szkieletora
        {
            HP -= Random.Range(3, 5);
            if (IsSkeletonDead())//uśmiercony
                SkeletonDead();
            else //został zadany damage, bez śmierci
            {
                PlayAnimation("Damage", 0.05f);
            }

            Destroy(playerWeapon.gameObject);
        }
    }

    /// <summary>
    /// Metoda po śmierci postaci
    /// </summary>
    private void SkeletonDead()
    {
        HoldAnimation = false;
        PlayAnimation("Die");
        HoldAnimation = true;
        Destroy(gameObject, 3f); //Zniknięcie obiektu szkieletora ze sceny z opóźnieniem 3s
    }
}