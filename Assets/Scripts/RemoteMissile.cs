using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteMissile : MonoBehaviour
{
    
    private Rigidbody rb;
    public GameObject ObjetoPerseguido;
    public float RapidezHorizontal = 30f;
    public float SeparacionConObjetivo = 0;
    void Start() 
    { 
        rb = GetComponent<Rigidbody>(); 
        rb.mass = 500; 
    }
    void FixedUpdate() 
    {
        AlcanzarPosicion( ObjetoPerseguido.transform.position - (SeparacionConObjetivo * ObjetoPerseguido.transform.forward), RapidezHorizontal, 1);
    }
    private void AlcanzarPosicion(Vector3 posObjetivo, float rapidezHorizontal, float propulsionFrontal)
    {
        Vector3 vectorHaciaObjetivo = posObjetivo - transform.position;
        float velocidadRelativa= ObjetoPerseguido.GetComponent<Rigidbody>().velocity.magnitude - rb.velocity.magnitude;
        float angulo = Vector3.Angle(vectorHaciaObjetivo, GetComponent<Rigidbody>().velocity);
        float factor = vectorHaciaObjetivo.magnitude * rapidezHorizontal;
        if ((velocidadRelativa>0) || (angulo < 70))
        {
            
            rb.AddForce(vectorHaciaObjetivo * propulsionFrontal * factor);
            rb.transform.LookAt(new Vector3(posObjetivo.x, rb.transform.position.y, posObjetivo.z));
        } 
        else 
        { //Ir frenando... 
            //rb.velocity = rb.velocity * 0.95f;
            rb.AddForce(vectorHaciaObjetivo * propulsionFrontal * factor);
            rb.AddForce(-rb.velocity * 2f);
            rb.transform.LookAt(new Vector3(posObjetivo.x, rb.transform.position.y, posObjetivo.z));
        }
    }
}
