using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraParent;
    public Vector3 currentAngle;
    public Vector3 targetAngle;
    float mouseX;
    float mouseSensitivty = 4f;
    float speed = 1f;

    void Start()
    {
        transform.LookAt(cameraParent);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) { targetAngle.y += (Input.GetAxis("Mouse X") * mouseSensitivty); }

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * speed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * speed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * speed));

        cameraParent.transform.eulerAngles = currentAngle;
    }

    public void RotateToGame1() { targetAngle = new Vector3(0, -60, 0); }
    public void RotateToGame2() { targetAngle = new Vector3(0, -120, 0); }
    public void RotateToGame3() { targetAngle = new Vector3(0, -175, 0); }
    public void RotateToGame4() { targetAngle = new Vector3(0, -230, 0); }
    public void RotateToStats() { targetAngle = new Vector3(0, -282, 0); }
}
