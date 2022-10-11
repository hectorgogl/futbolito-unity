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
    //para tener un limite
    int maximo_goles;

    float UltimoTiempo_PelotaTocada;
    float Tiempo_Restart;

    // int goles_azul;
    // int goles_rojo;

    void Start()
    {
        Ultimo_Equipo = "";

        // maximo_goles = 2;

        UltimoTiempo_PelotaTocada = Time.time; //tener referencia de cuando tocan la pelota
        Tiempo_Restart = 10;

        // goles_azul = 0;
        // goles_rojo = 0;
    }

    void Update()
    {
        if (Time.time - UltimoTiempo_PelotaTocada > Tiempo_Restart) //lo usamos para que no se quede la pelota atascada y no se haga nada
        {
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
            Goal(other.transform.localPosition.x); //obtenemos posicion en x para evaluar el gol dependiendo en que lado de la cancha este la pelota
        }
    }

    public void Goal(float pelota_Xcord) //obtenemos cordenada y evaluamos el gol
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

        RestartMatch(); //reiniciamos cuando se meta gol para seguir el juego

        // Debug.Log("Blue: " + goles_azul + " - Red: " + goles_rojo);
    }

    void RestartPelota() //vuelve a generar la pelota
    {
        pelota.RestartPosition();
        pelota.ApplyRandomForce(); 
        
        Ultimo_Equipo = "";

        UltimoTiempo_PelotaTocada = Time.time;
    }

    void RestartMatch() //para reinciar el juego
    {
        RestartPelota();

        // goles_azul = 0;
        // goles_rojo = 0;

        agente_azul.TerminarPartido();
        agente_rojo.TerminarPartido();
    }

    public void TouchedBall(string ultimo_jugador) //saber que equipo esta tocando la pelota
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

    public void NotTouching() //saber quien no esta tocando nada
    {
        agente_rojo.NoTocando();
        agente_azul.NoTocando();
    }
}


