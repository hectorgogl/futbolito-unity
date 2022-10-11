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

        UltimoTiempo_PelotaTocada = Time.time;
        Tiempo_Restart = 10;
    }

    void Update()
    {
        if (Time.time - UltimoTiempo_PelotaTocada > Tiempo_Restart)
        {
            RestartPelota();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Pelota")
        {
            Goal(other.transform.localPosition.x);
        }
    }

    public void Goal(float pelota_Xcord) 
    {
        if (pelota_Xcord > 0)
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

        RestartMatch();

    }

    void RestartPelota()
    {
        pelota.RestartPosition();
        pelota.ApplyRandomForce();
        
        Ultimo_Equipo = "";

        UltimoTiempo_PelotaTocada = Time.time;
    }

    void RestartMatch()
    {
        RestartPelota();

        agente_azul.TerminarPartido();
        agente_rojo.TerminarPartido();
    }

    public void TouchedBall(string ultimo_jugador)
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

    public void NotTouching()
    {
        agente_rojo.NoTocando();
        agente_azul.NoTocando();
    }
}


