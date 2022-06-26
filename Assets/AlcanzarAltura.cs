using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcanzarAltura : MonoBehaviour
{
    private float fuerzaLevitacion;
    private Rigidbody rb;
    public GameObject ObjetoPerseguido = null;
    public float RapidezVertical = 0.4f;
    public float AlturaDeseada = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.mass = 1000;
        fuerzaLevitacion = -(rb.mass * Physics.gravity.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ObjetoPerseguido != null)
        {
            AlturaDeseada = ObjetoPerseguido.transform.position.y + 30;
        }
        
        AlcanzarAlturaM(AlturaDeseada, RapidezVertical);
    }

    private void AlcanzarAlturaM(float alturaObjetivo, float rapidezVertical)
    {
        float distancia = (alturaObjetivo - transform.position.y);
        if (rb.velocity.y >= 0f)
        {
            float factor = distancia * rapidezVertical;
            rb.AddForce(Vector3.up * fuerzaLevitacion * factor);
        }
        else
        {
            float factor = Mathf.Max(0, distancia * rapidezVertical * 5);
            rb.AddForce(Vector3.up * fuerzaLevitacion * factor);
        }
    }
}
