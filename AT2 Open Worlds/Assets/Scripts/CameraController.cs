using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    Vector3 offset;
    Vector3 Yoffset;

    Vector3 angle;

    public float speed;
    public float camDist;


    // Use this for initialization
    void Start()
    {
        offset = new Vector3(0, 0, -camDist);
    }

    void LateUpdate()
    {
        angle = transform.rotation.eulerAngles;


        float haxis = Input.GetAxis("Mouse X");
        float vaxis = -Input.GetAxis("Mouse Y");

        offset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X"), Vector3.up) * offset;
        Yoffset = Quaternion.AngleAxis (Input.GetAxis ("Mouse Y"), Vector3.right) * offset;

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
