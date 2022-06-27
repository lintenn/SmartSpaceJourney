using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using weka.classifiers.trees;
using weka.classifiers.evaluation;
using weka.core;
using java.io;
using java.lang;
using java.util;
using weka.classifiers.functions;
using weka.classifiers;
using weka.core.converters;

public class AprendizNavegar : MonoBehaviour
{
    Classifier saberPredecirGiro, saberPredecirProductoFinal;
    weka.core.Instances casosEntrenamiento;
    Text texto;
    private string ESTADO = "Sin conocimiento";
    public GameObject PuntoObjetivo;
    public float valorMaximoAceleracion, valorMaximoGiro, paso = 1, Velocidad_Simulacion = 1;
    float mejorGiro, mejorAceleracion, productoObjetivo;
    private Vector3 posicionActual;
    private Vector3 rotacionActual;
    Rigidbody rb;

    ShipController shipController;
    private float time = 0;

    DotProduct script;

    private float ProductoInicial;
    private bool esperando = false;

    void Start()
    {
        shipController = GetComponent<ShipController>();
        script = GetComponent<DotProduct>();

        posicionActual = transform.position;
        rotacionActual = transform.eulerAngles;

        rb = GetComponent<Rigidbody>();

        Time.timeScale = Velocidad_Simulacion;                                          //...opcional: hace que se vea más rápido (recomendable hasta 5)
        
        //if (ESTADO == "Sin conocimiento") StartCoroutine("Entrenamiento");              //Lanza el proceso de entrenamiento
                                                                                    
        casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/Finales_Experiencias_Navegar.arff"));
        saberPredecirGiro = (Classifier)SerializationHelper.read("Assets/saberPredecirGiroModelo");
        saberPredecirProductoFinal = (Classifier)SerializationHelper.read("Assets/saberPredecirProductoFinalModelo");
        ESTADO = "Con conocimiento";
    }

    IEnumerator Entrenamiento()
    {

        //Uso de una tabla vacía:
        casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/Iniciales_Experiencias_Navegar.arff"));  //Lee fichero con variables. Sin instancias

        //Uso de una tabla con los datos del último entrenamiento:
        //casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/Finales_Experiencias_Navegar.arff"));    //... u otro con muchas experiencias

        if (casosEntrenamiento.numInstances() < 10)
        {
            // ENTRENAMIENTO: crea una tabla con las aceleraciones y giros necesarios dada una situación con velocidades y rotaciones iniciales y producto escalar inicial y final
            for (float velocidad = 0; velocidad <= 7.1; velocidad = velocidad + 1)
            {
                for (float rotation = 0; rotation <= 181; rotation = rotation + 5)
                {
                    for (float aceleracion = -1; aceleracion <= 1.05; aceleracion = aceleracion + 0.2f)
                    {
                        for (float giro = -1; giro <= 1.05; giro = giro + 0.2f)                    //Bucle de planificación del giro durante el entrenamiento
                        {
                            shipController.speed = 0;
                            shipController.turn = 0;
                            rb.velocity = transform.forward * 0;
                            transform.position = posicionActual;
                            transform.eulerAngles = new Vector3(rotacionActual.x, rotation, rotacionActual.z);

                            time = Time.time;
                            yield return new WaitUntil(() => Time.time - time >= 0.1);

                            rb.velocity = transform.forward * velocidad;
                            ProductoInicial = script.dot;
                            shipController.speed = aceleracion;
                            shipController.turn = giro;

                            time = Time.time;
                            yield return new WaitUntil(() => Time.time - time >= 0.25);       //... y espera a que simule la velocidad y giro

                            Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
                            print("ENTRENAMIENTO: con velocidad " + velocidad + " y dotInicial " + ProductoInicial
                            + " y aceleración " + aceleracion + " y giro " + giro + " se alcanzó productoFinal de " + script.dot);
                            casoAaprender.setDataset(casosEntrenamiento);                          //crea un registro de experiencia
                            casoAaprender.setValue(0, velocidad);                                         //guarda los datos de las fuerzas utilizadas
                            casoAaprender.setValue(1, ProductoInicial);
                            casoAaprender.setValue(2, aceleracion);
                            casoAaprender.setValue(3, giro);
                            casoAaprender.setValue(4, script.dot);
                            casosEntrenamiento.add(casoAaprender);                                 //guarda el registro en la lista casosEntrenamiento
                        }
                    }
                }
            }


            File salida = new File("Assets/Finales_Experiencias_Navegar.arff");
            if (!salida.exists())
                System.IO.File.Create(salida.getAbsoluteFile().toString()).Dispose();
            ArffSaver saver = new ArffSaver();
            saver.setInstances(casosEntrenamiento);
            saver.setFile(salida);
            saver.writeBatch();
        }


        //APRENDIZAJE CONOCIMIENTO:  
        saberPredecirGiro = new M5P();                                                //crea un algoritmo de aprendizaje M5P (árboles de regresión)
        casosEntrenamiento.setClassIndex(3);                                             //y hace que aprenda el GIRO
        saberPredecirGiro.buildClassifier(casosEntrenamiento);                        //REALIZA EL APRENDIZAJE DEL GIRO
        SerializationHelper.write("Assets/saberPredecirGiroModelo", saberPredecirGiro);

        saberPredecirProductoFinal = new M5P();                                                //crea un algoritmo de aprendizaje M5P (árboles de regresión)
        casosEntrenamiento.setClassIndex(4);                                             //y hace que aprenda el PRODUCTO ESCALAR
        saberPredecirProductoFinal.buildClassifier(casosEntrenamiento);                        //REALIZA EL APRENDIZAJE DEL PRODUCTO ESCALAR
        SerializationHelper.write("Assets/saberPredecirProductoFinalModelo", saberPredecirProductoFinal);

        productoObjetivo = 0;

        ESTADO = "Con conocimiento";

    }
    void FixedUpdate()                                                                                 //DURANTEL EL JUEGO: Aplica lo aprendido para navegar
    {
        Time.timeScale = Velocidad_Simulacion;
        if ((ESTADO == "Con conocimiento") && Time.time > 0.5)
        {
            float menorProducto = 1e9f;

            if (!esperando)
            {
                time = Time.time;
                esperando = true;
            }

            if (esperando && Time.time - time >= 0.25)
            {
                for (float aceleracion = -1; aceleracion <= 1.05; aceleracion = aceleracion + 0.2f)                                           //Bucle FOR sobre la aceleracion         
                {
                    Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());
                    casoPrueba.setDataset(casosEntrenamiento);
                    casoPrueba.setValue(0, rb.velocity.magnitude);                                         //guarda los datos de las fuerzas utilizadas
                    casoPrueba.setValue(1, script.dot);
                    casoPrueba.setValue(2, aceleracion);
                    casoPrueba.setValue(4, productoObjetivo);                                                  //y el producto escalar
                    float giro = (float)saberPredecirGiro.classifyInstance(casoPrueba);                          //Predice giro a partir del producto y aceleracion 

                    if (giro > -0.05 && giro < 0.05) giro = 0;
                    if (giro < -1) giro = -1;
                    if (giro > 1) giro = 1;

                    Instance casoPrueba2 = new Instance(casosEntrenamiento.numAttributes());
                    casoPrueba2.setDataset(casosEntrenamiento);                                                  //Utiliza el "modelo fisico aproximado"                  
                    casoPrueba2.setValue(0, rb.velocity.magnitude);                                         //guarda los datos de las fuerzas utilizadas
                    casoPrueba2.setValue(1, script.dot);
                    casoPrueba2.setValue(2, aceleracion);
                    casoPrueba2.setValue(3, giro);                                                             //Crea una registro 
                    float productoFinal = (float)saberPredecirProductoFinal.classifyInstance(casoPrueba2);     //Predice el producto resultante

                    if (Mathf.Abs(productoFinal - productoObjetivo) < menorProducto || Mathf.Abs(productoFinal) - menorProducto < 0.02)                     //Busca la aceleracion con un producto escalar menor
                    {
                        menorProducto = Mathf.Abs(productoFinal - productoObjetivo);                     //si encuentra una buena toma nota de ese producto
                        mejorAceleracion = aceleracion;                                                                       //de la fuerzas que uso, aceleracion
                        mejorGiro = giro;                                                                       //y giro
                        //print("RAZONAMIENTO: Para dotInicial" + script.dot + " y velocidad " + r.velocity.magnitude + " , una posible acción es ejercer una aceleración de = " + mejorAceleracion + " y giro = " + mejorGiro + " se alcanzaría una distancia de " + productoFinal);
                    }
                }

                shipController.speed = mejorAceleracion;
                shipController.turn = mejorGiro;

                print("DECISION REALIZADA: Se ejerció aceleración " + mejorAceleracion + " y giro " + mejorGiro);
                esperando = false;
            }
        }
        
    }
}
