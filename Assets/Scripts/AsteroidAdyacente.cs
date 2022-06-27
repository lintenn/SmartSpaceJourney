using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidAdyacente : MonoBehaviour
{
    public GameObject center;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Misil"))
        {
            center.GetComponent<GiroAsteroidJoint>().RomperJoints();
            Destroy(collider.gameObject);
        }
    }
}
