using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvniPropulsion : MonoBehaviour
{
    public float maxSpeed = 5;
    public float module = 1;
    Rigidbody rb;
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
        
        if (rb.velocity.magnitude < maxSpeed) 
        {
            rb.AddForce(transform.forward * module);
        }
        else
        {
            rb.AddForce(-transform.forward * module);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Misil"))
        {
            fireInstance = Instantiate(firePrefab, transform.position + new Vector3(0, -30, 0), Quaternion.Euler(-90, 180, 0));
            Destroy(fireInstance, 5);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
