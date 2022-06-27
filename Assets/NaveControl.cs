using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveControl : MonoBehaviour
{
    Rigidbody rb;
    public float module = 1;

    AlcanzarAltura scriptAltura;
    string estado = "Abajo";
    string estadoOvni = "Libre";
    public GameObject[] ojos;
    public GameObject misilPrefab;
    public GameObject remoteMisilPrefab;
    public GameObject ovniPrefab;
    GameObject misilInstance;
    GameObject remoteMisilInstance;
    GameObject ovniInstance;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scriptAltura = GetComponent<AlcanzarAltura>();
        //animator.Play("Piloting");
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
            rb.AddForce(-transform.right * 1);
            paredDcha = hit.transform.gameObject;
        }

        // Evitamos chocar por la izquierda
        if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit2, 10, 1 << 3)) 
        {
            Debug.DrawRay(transform.position, -transform.right * 10 , Color.red);
            rb.AddForce(transform.right * 1);
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

                    animator.Play("Switches");

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
                    float distanciaDcha = float.MaxValue, distanciaIzda = float.MaxValue;

                    if (Physics.Raycast(hit3.transform.position, hit3.transform.right, out RaycastHit hitDcha))
                    {
                        distanciaDcha = Vector3.Distance(hit3.transform.position, hitDcha.transform.position);
                    }

                    if (Physics.Raycast(hit3.transform.position, -hit3.transform.right, out RaycastHit hitIzda))
                    {
                        distanciaIzda = Vector3.Distance(hit3.transform.position, hitIzda.transform.position);
                    }

                    if (distanciaDcha >= distanciaIzda)
                    {
                        rb.AddForce(transform.right * module, ForceMode.Impulse);
                    }
                    else
                    {
                        rb.AddForce(-transform.right * module, ForceMode.Impulse);
                    }

                }
            }
        }

        if (estadoOvni == "Libre") 
        {
            Invoke("ProbarSuerteOvni", 150);
            estadoOvni = "Probando suerte";
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

    private void ProbarSuerteOvni() 
    {
        if (Random.Range(0, 2) == 1)
        {
            ovniInstance = Instantiate(ovniPrefab, transform.position + new Vector3(0, -20, 200), transform.rotation);

            ovniInstance.transform.LookAt(transform.position + new Vector3(10, -20, 15));

            print("SE SPAWNEO UN OVNI");
        } 
        else
        {
            print("NO HUBO SUERTE");
        }

        estadoOvni = "Libre";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ovni"))
        {
            print("OVNI DETECTADO");

            animator.Play("Switches");

            remoteMisilInstance = Instantiate(remoteMisilPrefab, transform.position + new Vector3(0, -5, 0), transform.rotation);
            remoteMisilInstance.GetComponent<AlcanzarAltura>().ObjetoPerseguido = other.gameObject;
            remoteMisilInstance.GetComponent<RemoteMissile>().ObjetoPerseguido = other.gameObject;
        }
    }
}
