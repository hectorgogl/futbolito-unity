// Importamos los paquetes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declaramos la clase que se hereda de MonoBehaviour
public class Pelota : MonoBehaviour
{
    // Declaramos al arbitro y a los jugadores
    public Arbitro arbitro;
    PlayerAgent agente_azul;
    PlayerAgent agente_rojo;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        ApplyRandomForce();
    }

    void FixedUpdate() {
        arbitro.NotTouching();
    }

    void OnTriggerEnter(Collider other)
    {

        // Esta función le dice al arbitro quien fue el último equipo
        // en tocar la pelota
        if (other.gameObject.tag == "JugadorRojo")
        {
            arbitro.TouchedBall("JugadorRojo");
        }
        else if (other.gameObject.tag == "JugadorAzul")
        {
            arbitro.TouchedBall("JugadorAzul");
        }
    }

    public void RestartPosition()
    {
        // Pone de vuelta a una posición inicial sin velocidad
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
        rigidBody.transform.localPosition = new Vector3(0, 6, 4);
    }

    public void ApplyRandomForce()
    {
        // Generamos una fuerza para que la pelota comience a moverse.
        // Puede moverse en cualquier direccion paralela a la cancha.
        float direccionX = (Random.Range(0, 2) * 2 - 1) * Random.Range(11, 16);
        float direccionZ = (Random.Range(0, 2) * 2 - 1) * Random.Range(11, 16);
 
        rigidBody.AddForce(new Vector3(direccionX, 0, direccionZ));
    }
}
