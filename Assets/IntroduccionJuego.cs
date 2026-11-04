using UnityEngine;
using TMPro;
using System.Collections; // Necesario para usar Corrutinas

public class IntroduccionJuego : MonoBehaviour
{
    [Header("Configuración de UI")]
    [Tooltip("El Panel que cubre la pantalla al inicio.")]
    public GameObject panelIntroduccion;

    [Tooltip("El componente TextMeshPro que mostrará el mensaje de la historia.")]
    public TextMeshProUGUI textoIntroduccion;

    [Tooltip("Tiempo de espera en segundos antes de pausar y mostrar la introducción al iniciar la escena.")]
    public float retrasoInicio = 0.1f;

    // Bandera para saber si el juego ya inició y está esperando la tecla E
    private bool juegoPausadoPorInicio = false;

    void Start()
    {
        // Asignamos el texto de inmediato para que esté listo
        if (textoIntroduccion != null)
        {
            textoIntroduccion.text = "Eres un gatito que se encuentra buscando un legendario churu para comer en unas ruinas abandonadas. Explora el paisaje, recolecta gemas para obtener puntos y recoge las monedas para intercambiarlas por la llave en el libro del intercambio para abrir el cofre en el centro del mapa donde se encuentra el churu legendario. Ten cuidado con no tocar las bombas o te iran quitando vida poco a poco!!!\n\nPresiona E para continuar";
        }

        // Iniciamos la corrutina para pausar un instante después de arrancar
        StartCoroutine(PausarConRetraso());
    }

    IEnumerator PausarConRetraso()
    {
        // Esperamos el tiempo configurado (por defecto 0.1 segundos)
        yield return new WaitForSecondsRealtime(retrasoInicio);

        // 1. Activamos el panel
        if (panelIntroduccion != null)
        {
            panelIntroduccion.SetActive(true);
        }

        // 2. Pausamos el tiempo del juego
        Time.timeScale = 0f;
        juegoPausadoPorInicio = true;
    }

    void Update()
    {
        // Si el juego está pausado por la introducción y el jugador presiona la tecla 'E'
        if (juegoPausadoPorInicio && Input.GetKeyDown(KeyCode.E))
        {
            ReanudarJuego();
        }
    }

    void ReanudarJuego()
    {
        juegoPausadoPorInicio = false;

        // 1. Desactivamos el panel de introducción
        if (panelIntroduccion != null)
        {
            panelIntroduccion.SetActive(false);
        }

        // 2. Reanudamos el tiempo del juego a la normalidad
        Time.timeScale = 1f;

        Debug.Log("¡Aventura iniciada!");
    }
}