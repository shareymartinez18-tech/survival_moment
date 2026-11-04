using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencias del Jugador")]
    [Tooltip("Arrastra aquí el objeto del Jugador que contiene el script 'salud'.")]
    public GameObject objetoJugador;

    [Header("Configuración de UI y Objetos")]
    [Tooltip("El Canvas o Panel de Game Over que se activará al morir.")]
    public GameObject canvasGameOver;

    [Tooltip("El GameObject que quieres desactivar al ocurrir el Game Over (ej. HUD de vida/monedas o el jugador mismo).")]
    public GameObject objetoADesactivar;

    // Referencia al script de salud del jugador
    private salud saludPlayer;
    private bool juegoTerminado = false;

    void Start()
    {
        // Validamos que el Canvas de Game Over empiece desactivado
        if (canvasGameOver != null)
        {
            canvasGameOver.SetActive(false);
        }

        // Obtenemos el script de salud del objeto asignado en el Inspector
        if (objetoJugador != null)
        {
            saludPlayer = objetoJugador.GetComponent<salud>();

            if (saludPlayer == null)
            {
                Debug.LogWarning("GameOverManager: El objeto asignado no tiene el script 'salud'.");
            }
        }
        else
        {
            Debug.LogError("GameOverManager: No se ha asignado el 'Objeto Jugador' en el Inspector.");
        }
    }

    void Update()
    {
        // Si el juego ya terminó, no seguimos evaluando
        if (juegoTerminado) return;

        if (saludPlayer != null)
        {
            // Comprobamos si la salud llega a 0 o menos
            if (saludPlayer.puntosDeSalud <= 0)
            {
                ActivarGameOver();
            }
        }
    }

    void ActivarGameOver()
    {
        juegoTerminado = true;

        // 1. Pausamos el tiempo del juego
        Time.timeScale = 0f;

        // 2. Activamos el Canvas de Game Over
        if (canvasGameOver != null)
        {
            canvasGameOver.SetActive(true);
        }

        // 3. Desactivamos el GameObject especificado en la jerarquía
        if (objetoADesactivar != null)
        {
            objetoADesactivar.SetActive(false);
        }

        Debug.Log("¡Juego Terminado! El tiempo se ha pausado.");
    }
}