using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    //[SerializeField] Collider ship;

    //[SerializeField] Transform shipTransform;

    public float acceleration = 0.005f;//500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;
    public string estado;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
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
        if (rb.velocity.magnitude > maxSpeed)
        {
            //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.X))
        {
            rb.AddTorque(transform.up * 1, ForceMode.Impulse);
        }

        currentAcceleration = acceleration * (script.enabled ? speed : Input.GetAxis("Vertical"));
        //currentAcceleration = acceleration * 1;

        if (Input.GetKeyUp(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0f;

        

        if (rb.velocity.magnitude < maxSpeed) 
        {
            rb.AddForce(transform.forward * currentAcceleration);
        }
        else
        {
            rb.AddForce(-transform.forward * currentAcceleration);
        }

        

//            frontRight.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * (script.enabled ? turn : Input.GetAxis("Horizontal"));
//            frontLeft.steerAngle = currentTurnAngle;

        //rb.transform.Rotate(0, currentTurnAngle, 0);
        
        if (currentTurnAngle == 0)
        {
            //rb.angularVelocity = new Vector3(0,0,0);
            rb.AddTorque(transform.up * -rb.angularVelocity.y);
        }
        else 
        {
            rb.AddTorque(transform.up * currentTurnAngle);
            
            /*if (rb.angularVelocity.magnitude < maxTurnAngle)
            {
                rb.AddTorque(transform.up * currentTurnAngle);
            }
            else
            {
                rb.AddTorque(-transform.up * rb.angularVelocity.y);
            }*/
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
