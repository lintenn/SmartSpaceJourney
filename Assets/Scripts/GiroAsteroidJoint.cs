using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroAsteroidJoint : MonoBehaviour
{
    Rigidbody rb;
    public float rapidezAngular = 0.0003f;
    public float velocidad = 0.0005f;
    public FixedJoint[] joints;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*float diferencia = velocidad - rb.angularVelocity.x;
        if (rb.angularVelocity.x <= velocidad)
        {
            float factor = diferencia * rapidezAngular;
            rb.AddTorque(transform.right * factor);
        }
        else 
        {
            float factor = diferencia * rapidezAngular;
            rb.AddTorque(transform.right * factor);
        }*/
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Misil"))
        {
            print("ENTRAMOS EN EL CENTRO");
            RomperJoints();
            Destroy(collider.gameObject);
        }
    }

    public void RomperJoints() 
    {
        foreach (FixedJoint joint in joints)
        {
            joint.connectedBody.useGravity = true;
            joint.breakForce = 0;
        }
    }
}
