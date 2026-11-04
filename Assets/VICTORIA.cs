using UnityEngine;
using System.Collections;
using TMPro; // Necesario para usar TextMeshPro

public class KeyDoorInteraction : MonoBehaviour
{
    [Header("Configuración de Activación")]
    [Tooltip("El GameObject que se va a activar (ej. Panel de victoria, mensaje, etc.).")]
    public GameObject objetoAActivar;

    [Tooltip("El GameObject que se va a desactivar (ej. la puerta misma, o un indicador visual).")]
    public GameObject objetoADesactivar;

    [Header("Configuración de UI (TextMeshPro único)")]
    [Tooltip("Referencia al único componente TMP que usaremos para ambos mensajes.")]
    public TextMeshProUGUI textoUnico;

    [Tooltip("Tiempo en segundos que se muestra el mensaje de error.")]
    public float tiempoMostrandoError = 2f;

    // Bandera para saber si el jugador está dentro del rango de interacción
    private bool jugadorEnRango = false;
    private bool mostrandoError = false;
    private Inventory inventarioJugador;
    private Coroutine rutinaError;

    private void Start()
    {
        // Nos aseguramos de que el texto comience oculto
        if (textoUnico != null)
        {
            textoUnico.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si el que entra es el jugador
        if (collision.CompareTag("Player"))
        {
            inventarioJugador = collision.GetComponent<Inventory>();

            if (inventarioJugador != null)
            {
                jugadorEnRango = true;

                // Solo mostramos "E para interactuar" si NO estamos mostrando un error en este momento
                if (textoUnico != null && !mostrandoError)
                {
                    textoUnico.text = "E para interactuar";
                    textoUnico.gameObject.SetActive(true);
                }

                Debug.Log("Presiona 'E' para usar una llave.");
            }
            else
            {
                Debug.LogWarning("El jugador entró en rango pero no tiene el script 'Inventory'.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Si el jugador sale del área
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = false;
            inventarioJugador = null;

            // Si NO estamos mostrando un error, ocultamos el texto inmediatamente.
            // Si SÍ estamos mostrando el error, dejamos que la corrutina lo oculte cuando termine el tiempo.
            if (textoUnico != null && !mostrandoError)
            {
                textoUnico.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Si el jugador está en rango y presiona la tecla 'E'
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            if (inventarioJugador != null)
            {
                // Verificamos si tiene al menos 1 llave
                if (inventarioJugador.Llaves >= 1)
                {
                    // Consumimos 1 llave
                    inventarioJugador.Llaves -= 1;
                    Debug.Log("¡Llave utilizada! Llaves restantes: " + inventarioJugador.Llaves);

                    // Si pasamos con éxito, limpiamos estados y ocultamos texto
                    mostrandoError = false;
                    if (rutinaError != null)
                    {
                        StopCoroutine(rutinaError);
                        rutinaError = null;
                    }

                    if (textoUnico != null) 
                    {
                        textoUnico.gameObject.SetActive(false);
                    }

                    // 1. Pausamos el tiempo del juego
                    Time.timeScale = 0f;

                    // 2. Activamos el GameObject designado
                    if (objetoAActivar != null)
                    {
                        objetoAActivar.SetActive(true);
                    }

                    // 3. Desactivamos el GameObject designado (ej. la puerta)
                    if (objetoADesactivar != null)
                    {
                        objetoADesactivar.SetActive(false);
                    }

                    // Desactivamos el rango para evitar múltiples activaciones
                    jugadorEnRango = false;
                }
                else
                {
                    Debug.Log("No tienes suficientes llaves (Necesitas 1).");

                    // Mostramos el mensaje de error "Llave requerida" en el mismo TMP
                    if (textoUnico != null)
                    {
                        textoUnico.text = "Llave requerida";
                        textoUnico.gameObject.SetActive(true);
                        mostrandoError = true;

                        // Si ya había un temporizador de error corriendo, lo reiniciamos
                        if (rutinaError != null)
                        {
                            StopCoroutine(rutinaError);
                        }
                        rutinaError = StartCoroutine(ManejarTextoError());
                    }
                }
            }
        }
    }

    private IEnumerator ManejarTextoError()
    {
        // Esperamos el tiempo del error (usamos Realtime por si pausas el juego en otras partes)
        yield return new WaitForSecondsRealtime(tiempoMostrandoError);

        mostrandoError = false;
        rutinaError = null;

        // Al terminar el tiempo de error, evaluamos qué hacer con el texto:
        if (textoUnico != null)
        {
            // Si el jugador todavía sigue dentro del rango, volvemos a mostrar el prompt de interacción
            if (jugadorEnRango)
            {
                textoUnico.text = "E para interactuar";
                textoUnico.gameObject.SetActive(true);
            }
            else
            {
                // Si el jugador ya salió del rango, apagamos el texto por completo
                textoUnico.gameObject.SetActive(false);
            }
        }
    }
}