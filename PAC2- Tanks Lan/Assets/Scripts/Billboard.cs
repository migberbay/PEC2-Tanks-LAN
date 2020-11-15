using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    Camera cam;

    private void Start()
    {
        cam = Camera.allCameras[0];
    }

    // Update is called once per frame
    void Update () {
        /*if (cam == null) {
            
        }
        Debug.Log(cam);*/

        transform.LookAt(cam.transform);
    }
}
