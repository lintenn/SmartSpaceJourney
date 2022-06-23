using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveControl : MonoBehaviour
{
    Rigidbody rb;
    public float module = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //float x = Input.GetAxis("Vertical");
        //float y = Input.GetAxis("Horizontal");

        //rb.AddRelativeTorque(new Vector3(x, y, 0));

        if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, 10, 1 << 3)) 
        {
            Debug.DrawRay(transform.position, transform.right * 10 , Color.red);
            rb.AddForce(-transform.right * 2);
        }

        if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit2, 10, 1 << 3)) 
        {
            Debug.DrawRay(transform.position, -transform.right * 10 , Color.red);
            rb.AddForce(transform.right * 2);
        }
        

        if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            rb.AddForce(transform.right * module, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rb.AddForce(-transform.right * module, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(new Vector3(0, 0, 1));
        }
    }
}
