using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public float acceleration = 0.005f;//500f;
    public float maxTurnAngle = 15f;
    public string estado;

    private float currentAcceleration = 0f;
    public float currentTurnAngle = 0f;

    public float maxSpeed = 4f;

    public float speed = 0;
    public float turn = 0;
    public float rapidezAngular = 0.3f;

    private Rigidbody rb;
    AprendizNavegar script;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        script = GetComponent<AprendizNavegar>();
        estado = "Normal";
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.X))
        {
            rb.AddTorque(transform.up * 1, ForceMode.Impulse);
        }

        // Aplicar Aceleración
        currentAcceleration = acceleration * (script.enabled ? speed : Input.GetAxis("Vertical"));

        if (rb.velocity.magnitude < maxSpeed) 
        {
            rb.AddForce(transform.forward * currentAcceleration);
        }
        else
        {
            rb.AddForce(-transform.forward * currentAcceleration);
        }

        // Aplicar Giro
        currentTurnAngle = maxTurnAngle * (script.enabled ? turn : Input.GetAxis("Horizontal"));
        
        if (currentTurnAngle == 0)
        {
            rb.AddTorque(transform.up * -rb.angularVelocity.y);
        }
        else 
        {
            rb.AddTorque(transform.up * currentTurnAngle);
        }


        //AlcanzarAnguloY(currentTurnAngle, rapidezAngular);
    }

    private void AlcanzarAnguloY(float anguloObjetivo, float rapidezAngular)
    {
        float diferenciaAgulo = Mathf.Abs(anguloObjetivo - transform.rotation.eulerAngles.y);

        if (rb.angularVelocity.y >= 0)
        {
            float factor = diferenciaAgulo * rapidezAngular;
            rb.AddTorque(new Vector3(0, 0, 1) * factor);
        }
        else
        {
            float factor = diferenciaAgulo * rapidezAngular;
            rb.AddTorque(new Vector3(0, 0, -1) * factor);
        }
    }
}
