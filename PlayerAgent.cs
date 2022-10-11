// Importamos los paquetes para el proyecto
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

//Declaramos la clase que se hereda de la clase Agent
public class PlayerAgent : Agent
{
    // Declaramos los palos y sus RB
    public GameObject[] palos;
    Rigidbody[] palosRigidbodys;

    public Rigidbody[] enemyPalosRigidbodys; 

    // Obtenemos datos de la pelota y su RB
    public Transform pelota;
    public Rigidbody pelotaRigidBody;

    // Strings que nos ayudan a la hora de declarar las acciones
    public string nombre_ejeX;
    public string nombre_ejeY;
    
    // Numero que nos indica el palo que se está controlando en este momento
    public int switchControlOffset;

    // Velocidades permitidas para la pelota
    float Velocidad_mov;
    float Velocidad_Angular_Max;

    // Numero del palo que se esta usando y si este fue el ultimo en tocar la pelota
    private int palo_utilizado;
    private bool tocando;
    
    
    public override void Initialize()
    {
        // Empezamos moviendo el palo de en medio.
        this.palo_utilizado = 1;
    }

    void Start()
    {
        // Le damos valor a las velocidades
        Velocidad_mov = 50;
        Velocidad_Angular_Max = 10;
        
        // Agregamos los RB de los palos en un arreglo
        palosRigidbodys = new Rigidbody[4];
        
        for (int i=0; i<palos.Length; i++)
        {
            // Obtenemos el RB y la velociddad angular de cada palo
            palosRigidbodys[i] = palos[i].GetComponent<Rigidbody>();
            palosRigidbodys[i].maxAngularVelocity = Velocidad_Angular_Max;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observaciones de la pelota (3 de posicion y 3 de Velocidad)
        sensor.AddObservation(pelota.localPosition);
        sensor.AddObservation(pelotaRigidBody.velocity);

        // Observaciones de posicion y rotacion de los palos
        // Para un equipo
        for (int i=0; i<palos.Length; i++)
        {
            sensor.AddObservation(palosRigidbodys[i].rotation.z);
            sensor.AddObservation(palosRigidbodys[i].position);
        }

        // Para el contrario
        for (int i=0; i<enemyPalosRigidbodys.Length; i++)
        {
            sensor.AddObservation(enemyPalosRigidbodys[i].rotation.z);
            sensor.AddObservation(enemyPalosRigidbodys[i].position);
        }
    }

    // Entrenamiento Heuristico
    public override void Heuristic(in ActionBuffers salidaAcciones)
    {
        // Dos acciones continuas: mover y rotar el palo
        var acciones = salidaAcciones.ContinuousActions;
        acciones[0] = Input.GetAxis(this.nombre_ejeX);
        acciones[1] = Input.GetAxis(this.nombre_ejeY);

        // Una accion discreta: cambiar el palo que se controla.
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

    // Al anotar gol, se acaba el partido y finaliza el episodio
    public void TerminarPartido()
    {
        EndEpisode();
    }

    // Se premia si anota un Gol
    public void Gol()
    {
        AddReward(9);
    }


    // Se premia si se toca la pelota
    public void TocoPelota()
    {
        AddReward(1);
        this.tocando = true;
    }

    // Si no la toca, se reinicia
    public void NoTocando()
    {
        this.tocando = false;
    }

    // Se penaliza si se mete un autogol
    public void AutoGol()
    {
        AddReward(-6);
    }

}


