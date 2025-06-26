using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraEspejo : MonoBehaviour
{
    public Transform playerTarget;
    public Transform mirror;

    public Vector3 offset;
    public Vector3 offsetMultiply;
    public float fieldOffsetMult;
    public float fieldOffset;

    public float distance;

    public Camera cam;

    void LateUpdate()
    {
        
    }

    public float offsetAltura;

    private void Update()
    {

        distance = Vector3.Distance(mirror.position, playerTarget.position);

        cam.fieldOfView = (distance * fieldOffsetMult) + fieldOffset;

        Vector3 localPlayer = mirror.InverseTransformPoint(playerTarget.position);
        transform.position = mirror.TransformPoint(new Vector3(localPlayer.x * offsetMultiply.x + offset.x, localPlayer.y * offsetMultiply.y + offset.y, -localPlayer.z * offsetMultiply.z + offset.z));

        Vector3 lookatmirror = mirror.TransformPoint(new Vector3(-localPlayer.x, -localPlayer.y + offsetAltura, localPlayer.z));
        transform.LookAt(lookatmirror);
    }
}
