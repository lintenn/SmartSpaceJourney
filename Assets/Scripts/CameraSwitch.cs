using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera[] cameras;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            cameras[0].enabled = !cameras[0].enabled;
            cameras[1].enabled = !cameras[1].enabled;
        }
    }
}
