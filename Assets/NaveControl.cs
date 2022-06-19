using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveControl : MonoBehaviour
{
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");

        rb.AddRelativeTorque(new Vector3(x, y, 0));
        

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(new Vector3(0, 0, 1));
        }
    }
}
