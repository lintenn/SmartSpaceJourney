using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroAsteroidSmall : MonoBehaviour
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
        float diferencia = velocidad - rb.angularVelocity.x;
        if (rb.angularVelocity.x <= velocidad)
        {
            float factor = diferencia * rapidezAngular;
            rb.AddTorque(transform.right * factor);
        }
        else 
        {
            float factor = diferencia * rapidezAngular;
            rb.AddTorque(transform.right * factor);
        }
    }
}
