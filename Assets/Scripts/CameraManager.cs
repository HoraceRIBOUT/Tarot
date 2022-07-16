using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform mainTarget = null;

    public float delay = 5f;

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, mainTarget.position, Time.deltaTime * delay);
    }
}
