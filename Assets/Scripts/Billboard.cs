using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (camera != null)
        {
            transform.LookAt(camera.transform.position);
            transform.Rotate(0,180,0);
        }
    }
}
