using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroAsteroidJump : MonoBehaviour
{
    Rigidbody rb;
    public float rapidezAngular = 0.3f;
    public float velocidad = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float diferencia = velocidad - rb.angularVelocity.z;
        if (rb.angularVelocity.z <= velocidad)
        {
            float factor = diferencia * rapidezAngular;
            rb.AddTorque(transform.forward * factor);
        }
        else 
        {
            float factor = diferencia * rapidezAngular;
            rb.AddTorque(transform.forward * factor);
        }

    }
}
