using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbitro : MonoBehaviour
{
    public Pelota pelota;
    public Transform T_pelota;
    public PlayerAgent agente_azul;
    public PlayerAgent agente_rojo;

    string Ultimo_Equipo; // Ultimo equipo que toco la pelota

    int maximo_goles;

    float UltimoTiempo_PelotaTocada;
    float Tiempo_Restart;

    void Start()
    {
        Ultimo_Equipo = "";

        UltimoTiempo_PelotaTocada = Time.time; //tener referencia de cuando tocan la pelota
        Tiempo_Restart = 10;
    }

    void Update()
    {
        if (Time.time - UltimoTiempo_PelotaTocada > Tiempo_Restart) // no se quede la pelota atascada y no se haga nada
        {
            RestartPelota();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Pelota")
        {
            Goal(other.transform.localPosition.x); //obtener coordenada x para evaluar en donde se mete el gol
        }
    }

    public void Goal(float pelota_Xcord) 
    {
        if (pelota_Xcord > 0) //estas condiciones son para saber de quien fue el gol o autogol
        {
            if (Ultimo_Equipo == "JugadorRojo")
            {
                Debug.Log("Autogol Rojo");
                agente_rojo.AutoGol();
            }
            else
            {
                Debug.Log("Gol Azul");
                agente_azul.Gol();
            }

        }
        else if (pelota_Xcord < 0)
        {
            if (Ultimo_Equipo == "JugadorAzul")
            {
                Debug.Log("Autogol Azul");
                agente_azul.AutoGol();
            }
            else
            {
                Debug.Log("Gol Rojo");
                agente_rojo.Gol();
            }

        }

        RestartMatch(); //reiniciamos cuando se meta gol para seguir el juego

    }

    void RestartPelota() //vuelve a generarse la pelota
    {
        pelota.RestartPosition();
        pelota.ApplyRandomForce();
        
        Ultimo_Equipo = "";

        UltimoTiempo_PelotaTocada = Time.time; //obtenemos tiempo de que se toco la pelota para reinciar en caso de que nadie la pueda tocar
    }

    void RestartMatch() //reinciar el juego
    {
        RestartPelota();

        agente_azul.TerminarPartido();
        agente_rojo.TerminarPartido();
    }

    public void TouchedBall(string ultimo_jugador) //para saber que equipo esta tocando la pelota
    {
        if (ultimo_jugador == "JugadorRojo")
        {

            Ultimo_Equipo = "JugadorRojo";
            agente_rojo.TocoPelota();

            UltimoTiempo_PelotaTocada = Time.time;
        }
        else if (ultimo_jugador == "JugadorAzul")
        {

            Ultimo_Equipo = "JugadorAzul";
            agente_azul.TocoPelota();

            UltimoTiempo_PelotaTocada = Time.time;
        }
    }

    public void NotTouching() //para saber quien no esta tocando la pelota
    {
        agente_rojo.NoTocando();
        agente_azul.NoTocando();
    }
}


