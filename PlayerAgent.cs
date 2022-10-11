using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    public GameObject[] palos;
    Rigidbody[] palosRigidbodys;

    public Rigidbody[] enemyPalosRigidbodys; //Creo que esto es irrelevante

    public Transform pelota;
    public Rigidbody pelotaRigidBody;

    public string nombre_ejeX;
    public string nombre_ejeY;

    public int switchControlOffset;

    float Velocidad_mov;
    float Velocidad_Angular_Max;

    // Posicion del palo que se esta moviendo.
    private int palo_utilizado;
    private bool tocando;

    public override void Initialize()
    {
        // Empezamos moviendo el palo de en medio.
        this.palo_utilizado = 1;
    }

    void Start()
    {
        Velocidad_mov = 50;
        Velocidad_Angular_Max = 10;
        
        palosRigidbodys = new Rigidbody[4];
        
        for (int i=0; i<palos.Length; i++)
        {
            palosRigidbodys[i] = palos[i].GetComponent<Rigidbody>();
            palosRigidbodys[i].maxAngularVelocity = Velocidad_Angular_Max;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // For the pelota
        sensor.AddObservation(pelota.localPosition);
        sensor.AddObservation(pelotaRigidBody.velocity);

        // For our team
        for (int i=0; i<palos.Length; i++)
        {
            sensor.AddObservation(palosRigidbodys[i].rotation.z);
            sensor.AddObservation(palosRigidbodys[i].position);
        }

        // For the other team
        for (int i=0; i<enemyPalosRigidbodys.Length; i++)
        {
            sensor.AddObservation(enemyPalosRigidbodys[i].rotation.z);
            sensor.AddObservation(enemyPalosRigidbodys[i].position);
        }
    }

    public override void Heuristic(in ActionBuffers salidaAcciones)
    {
        var acciones = salidaAcciones.ContinuousActions;
        acciones[0] = Input.GetAxis(this.nombre_ejeX);
        acciones[1] = Input.GetAxis(this.nombre_ejeY);

        // Cambiar el palo que se controla.
        var accion_discreta = salidaAcciones.DiscreteActions;
        // Se cambia el palo con las teclas de numeros
        for (int i = 1; i <= this.palos.Length; i++)
        {
            if (Input.GetKeyDown(""+(i+this.switchControlOffset)))
            {
                Debug.Log("Captando"+(i+this.switchControlOffset));
                accion_discreta[0] = i;
            }
        }

    }

    public override void OnActionReceived(ActionBuffers acc)
    {
        // Cambiar palo seleccionado, teclas 1 - num.palos
        if (acc.DiscreteActions[0] > 0)
        {
            this.palo_utilizado = acc.DiscreteActions[0] - 1;

            // Recompensa si el palo seleccionado esta cerca de la pelota.
            var ballX = this.pelotaRigidBody.transform.position.x;
            var paloX = this.palosRigidbodys[this.palo_utilizado].transform.position.x;
            if (ballX > paloX - 2.0f && ballX < paloX + 2.0f)
            {
                AddReward(0.2f);
            }
        }


        // Mover el palo seleccionado
        var palo = this.palosRigidbodys[this.palo_utilizado];
        palo.AddForce(0, 0, acc.ContinuousActions[1] * Velocidad_mov);
        palo.AddTorque(acc.ContinuousActions[0] * transform.forward * Time.deltaTime * 10000000);

        // Penalizar si hubo disparo sin tocar la pelota
        if (!tocando && (acc.ContinuousActions[0] != 0))
        {
            AddReward(-0.005f);
        }
    }

    public void TerminarPartido()
    {
        EndEpisode();
    }

    // public void GanarPartido()
    // {
    //     AddReward(9);
    // }

    // public void PerderPartido()
    // {
    //     AddReward(-9);
    // }

    public void Gol()
    {
        AddReward(9);
    }

    // public void TiroBloqueado()
    // {
    //     AddReward(1);
    // }

    public void TocoPelota()
    {
        AddReward(1);
        this.tocando = true;
    }

    public void NoTocando()
    {
        this.tocando = false;
    }

    public void AutoGol()
    {
        AddReward(-6);
    }

    // public void RestartedBall()
    // {
    //     AddReward(-1);
    // }
}


