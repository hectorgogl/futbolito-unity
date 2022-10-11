using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pelota : MonoBehaviour
{
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
        // if (other.gameObject.tag != "Untagged")
        // {
        //     Debug.Log("Ball touch: " + other.gameObject.tag);
        // }

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
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
        rigidBody.transform.localPosition = new Vector3(0, 6, 4);
    }

    public void ApplyRandomForce()
    {
        float direccionX = (Random.Range(0, 2) * 2 - 1) * Random.Range(11, 16);
        float direccionZ = (Random.Range(0, 2) * 2 - 1) * Random.Range(11, 16);
 
        rigidBody.AddForce(new Vector3(direccionX, 0, direccionZ));
    }
}
