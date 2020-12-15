using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EW: Ruch kamery
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float CameraMovementSpeed = 1f;
    public Vector3 CameraVector;

    void LateUpdate()
    {
        Vector3 PlayerPosition = PlayerController.Instance.transform.position;

        Vector3 CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y, PlayerPosition.z - 15f);

        transform.position = Vector3.Lerp(transform.position, CameraPosition + CameraVector, Time.deltaTime * CameraMovementSpeed);
    }
}
