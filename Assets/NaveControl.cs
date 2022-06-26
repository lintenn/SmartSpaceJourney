using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveControl : MonoBehaviour
{
    Rigidbody rb;
    public float module = 1;

    AlcanzarAltura scriptAltura;
    string estado = "Abajo";
    public GameObject[] ojos;
    public GameObject misilPrefab;
    GameObject misilInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scriptAltura = GetComponent<AlcanzarAltura>();
    }

    // Update is called once per frame
    void Update()
    {
        //float x = Input.GetAxis("Vertical");
        //float y = Input.GetAxis("Horizontal");

        //rb.AddRelativeTorque(new Vector3(x, y, 0));
        GameObject paredDcha = null, paredIzda = null;

        // Evitamos chocar por la derecha
        if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, 10, 1 << 3)) 
        {
            Debug.DrawRay(transform.position, transform.right * 10 , Color.red);
            rb.AddForce(-transform.right * 2);
            paredDcha = hit.transform.gameObject;
        }

        // Evitamos chocar por la izquierda
        if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit2, 10, 1 << 3)) 
        {
            Debug.DrawRay(transform.position, -transform.right * 10 , Color.red);
            rb.AddForce(transform.right * 2);
            paredIzda = hit2.transform.gameObject;
        }
        

        // Observamos
        foreach (GameObject ojo in ojos)
        {
            if (Physics.Raycast(ojo.transform.position, ojo.transform.forward, out RaycastHit hit3, 40, 1 << 6) && estado == "Abajo")
            {
                if (hit3.transform.gameObject.CompareTag("AsteroidJoint"))
                {
                    
                    misilInstance = Instantiate(misilPrefab, transform.position + new Vector3(0, 5, 0), transform.rotation);
                    Rigidbody rbMisil = misilInstance.GetComponent<Rigidbody>();
                    //Vector3 vectorDiferencia = transform.position - hit3.transform.position;
                    
                    //vectorDiferencia.Normalize(); // Lo normalizamos

                    misilInstance.transform.LookAt(hit3.transform);

                    //rbMisil.AddForce(misilInstance.transform.forward * 10, ForceMode.Impulse);

                    Invoke("CoolDownDisparar", 45);
                    estado = "Disparando";
                }
                else if (hit3.transform.localScale.z >= 28)
                {
                    scriptAltura.AlturaDeseada = 30;
                    Invoke("BajarAltura", 85);
                    estado = "Arriba";
                }
                else
                {
                    if (paredDcha == null) {
                        rb.AddForce(transform.right * module, ForceMode.Impulse);
                    }
                    else 
                    {
                        rb.AddForce(-transform.right * module, ForceMode.Impulse);
                    }
                }
            }
        }




        if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            rb.AddForce(transform.right * module, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rb.AddForce(-transform.right * module, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(new Vector3(0, 0, 1));
        }
    }

    private void BajarAltura() 
    {
        scriptAltura.AlturaDeseada = 0;
        estado = "Abajo";
    }

    private void CoolDownDisparar()
    {
        estado = "Abajo";
    }
}
