using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    float Yrot;

    RaycastHit hit;
    GameObject grabbedOBJ;
    public Transform grabPos;
    public Transform playerBody;
    public float sensitivity = 100f;

    // Update is called once per frame

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Yrot -= Input.GetAxis("Mouse Y") * sensitivity;
        Yrot = Mathf.Clamp(Yrot, -60, 80);
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(Yrot, 0, 0); //                        distance that the raycast will travel | How to detect a rigid body

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out hit, 5) && hit.transform.GetComponent<Rigidbody>())
        {
            grabbedOBJ = hit.transform.gameObject;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            grabbedOBJ = null;
        }
        if (grabbedOBJ)
        {
            grabbedOBJ.GetComponent<Rigidbody>().velocity = 10 * (grabPos.position - grabbedOBJ.transform.position);
        }
    }
}
