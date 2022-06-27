using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroAsteroidJoint : MonoBehaviour
{
    Rigidbody rb;
    public float rapidezAngular = 0.0003f;
    public float velocidad = 0.0005f;
    public FixedJoint[] joints;
    public GameObject firePrefab;
    private GameObject fireInstance;
    
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
            RomperJoints();
            Destroy(collider.gameObject);
        }
    }

    public void RomperJoints() 
    {
        foreach (FixedJoint joint in joints)
        {
            joint.connectedBody.useGravity = true;
            joint.connectedBody.AddExplosionForce(1000, transform.position, 5);
            joint.breakForce = 0;
            
        }
        rb.AddExplosionForce(1000, transform.position, 5);
        fireInstance = Instantiate(firePrefab, transform.position + new Vector3(0, -30, 0), Quaternion.Euler(-90, 180, 0));
        Destroy(fireInstance, 5);
    }


}
