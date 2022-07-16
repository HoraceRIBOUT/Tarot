using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cam;
    public Transform mainTarget = null;

    public Color targetColor = Color.white;

    public float delay = 5f;

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, mainTarget.position, Time.deltaTime * delay);
        cam.backgroundColor = Color.Lerp(cam.backgroundColor, targetColor, Time.deltaTime / 2f);
    }

    public void ChangeBG(Color col)
    {
        targetColor = col;
    }
}
