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

    // int goles_azul;
    // int goles_rojo;

    void Start()
    {
        Ultimo_Equipo = "";

        // maximo_goles = 2;

        UltimoTiempo_PelotaTocada = Time.time;
        Tiempo_Restart = 10;

        // goles_azul = 0;
        // goles_rojo = 0;
    }

    void Update()
    {
        if (Time.time - UltimoTiempo_PelotaTocada > Tiempo_Restart)
        {
            // agente_azul.RestartedBall();
            // agente_rojo.RestartedBall();

            RestartPelota();
        }

        // if (goles_azul >= maximo_goles)
        // {
        //     agente_azul.GanarPartido();
        //     agente_rojo.PerderPartido();

        //     RestartMatch();
        // }
        // else if (goles_rojo >= maximo_goles)
        // {
        //     agente_azul.PerderPartido();
        //     agente_rojo.GanarPartido();

        //     RestartMatch();
        // }
    }

    void OnTriggerEnter(Collider other) //Agrege el metodo de colision hay que checar que funciona
    {
        if (other.gameObject.tag == "Pelota")
        {
            Goal(other.transform.localPosition.x);
        }
    }

    public void Goal(string pelota_Xcord) // Hay que tener dos porterias o encontrar como cambiarle el nombre a los collider
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

            // goles_azul++;
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

            // goles_rojo++;
        }

        RestartMatch();

        Debug.Log("Blue: " + goles_azul + " - Red: " + goles_rojo);
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

        // goles_azul = 0;
        // goles_rojo = 0;

        agente_azul.TerminarPartido();
        agente_rojo.TerminarPartido();
    }

    public void TouchedBall(string ultimo_jugador)
    {
        if (ultimo_jugador == "JugadorRojo")
        {
            // if (Ultimo_Equipo == "JugadorAzul")
            // {
            //     agente_rojo.TiroBloqueado();
            // }

            Ultimo_Equipo = "JugadorRojo";
            agente_rojo.TocoPelota();

            UltimoTiempo_PelotaTocada = Time.time;
        }
        else if (ultimo_jugador == "JugadorAzul")
        {
            // if (Ultimo_Equipo == "JugadorRojo")
            // {
            //     agente_azul.TiroBloqueado();
            // }

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


