using UnityEngine;
using System.Collections;
using TMPro; // Necesario para usar TextMeshPro

public class ShopInteraction : MonoBehaviour
{
    [Header("Configuración de la Tienda")]
    [Tooltip("El Panel de la Tienda que se abre al interactuar.")]
    public GameObject panelTienda;

    [Tooltip("El GameObject que se va a activar tras una compra exitosa (ej. la llave entregada).")]
    public GameObject objetoAActivar;

    [Tooltip("Costo en monedas de la llave.")]
    public int costoMonedas = 5;

    [Header("Textos TMP")]
    [Tooltip("TMP del mundo/HUD para el prompt de 'E para interactuar' y el error persistente.")]
    public TextMeshProUGUI textoPromptMundo;

    [Tooltip("TMP dentro del panel de la tienda para hacer la pregunta, mostrar éxito o errores internos.")]
    public TextMeshProUGUI textoTMPPanel;

    [Tooltip("Mensaje de error cuando no hay suficientes monedas.")]
    public string mensajeError = "Monedas insuficientes";

    [Tooltip("Tiempo en segundos que se muestra el mensaje de error persistente fuera del panel.")]
    public float tiempoMostrandoError = 2f;

    [Tooltip("Tiempo en segundos que se muestra el texto de éxito en el panel antes de cerrarlo automáticamente.")]
    public float tiempoMostrarExito = 3f; // Aumentado ligeramente para dar tiempo a leer la frase completa

    // Variables de estado
    private bool jugadorEnRango = false;
    private bool tiendaAbierta = false;
    private bool mostrandoError = false;
    private bool esperandoRespuesta = false;
    private Inventory inventarioJugador;
    private Coroutine rutinaError;
    private Coroutine rutinaExito;

    private void Start()
    {
        // Estado inicial de la UI
        if (textoPromptMundo != null) textoPromptMundo.gameObject.SetActive(false);
        if (panelTienda != null) panelTienda.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inventarioJugador = collision.GetComponent<Inventory>();

            if (inventarioJugador != null)
            {
                jugadorEnRango = true;

                // Mostramos el prompt del mundo si no hay errores activos ni la tienda abierta
                if (textoPromptMundo != null && !mostrandoError && !tiendaAbierta)
                {
                    textoPromptMundo.text = "E para interactuar";
                    textoPromptMundo.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning("El jugador entró en rango pero no tiene el script 'Inventory'.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = false;
            inventarioJugador = null;

            // Si sale y no hay error activo, apagamos el prompt del mundo
            if (textoPromptMundo != null && !mostrandoError)
            {
                textoPromptMundo.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // 1. Abrir la tienda con 'E' si está en rango y la tienda está cerrada
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E) && !tiendaAbierta)
        {
            if (inventarioJugador != null)
            {
                AbrirTienda();
            }
        }

        // 2. Gestionar las opciones R (Sí) y T (No) cuando la tienda está abierta y esperando respuesta
        if (tiendaAbierta && esperandoRespuesta)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ProcesarCompraSi();
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ProcesarCompraNo();
            }
        }
    }

    private void AbrirTienda()
    {
        tiendaAbierta = true;
        esperandoRespuesta = true;

        if (panelTienda != null)
        {
            panelTienda.SetActive(true);
        }

        // Ocultamos el prompt del mundo
        if (textoPromptMundo != null)
        {
            textoPromptMundo.gameObject.SetActive(false);
        }

        // Configuramos el texto dentro del panel de la tienda
        if (textoTMPPanel != null)
        {
            textoTMPPanel.text = "¿Quieres comprar la llave para el cofre por 5 monedas?\n[R] Sí   |   [T] No";
        }

        Debug.Log("Tienda abierta. Esperando decisión (R / T).");
    }

    private void ProcesarCompraSi()
    {
        esperandoRespuesta = false; // Ya dio una respuesta, bloqueamos nuevas pulsaciones

        if (inventarioJugador.Monedas >= costoMonedas)
        {
            // ¡Tiene suficientes monedas!
            inventarioJugador.Monedas -= costoMonedas;
            Debug.Log("¡Compra exitosa! Monedas restantes: " + inventarioJugador.Monedas);

            // Activamos el objeto de recompensa
            if (objetoAActivar != null)
            {
                objetoAActivar.SetActive(true);
            }

            // Mostramos el texto de éxito solicitado en el TMP del panel
            if (textoTMPPanel != null)
            {
                textoTMPPanel.text = "Llave apareció en todo el norte del mapa";
            }

            // Esperamos un momento para que el jugador lea el mensaje y cerramos la tienda
            if (rutinaExito != null) StopCoroutine(rutinaExito);
            rutinaExito = StartCoroutine(CerrarTiendaDespuesDeExito());
        }
        else
        {
            // No tiene suficientes monedas -> Cerramos tienda y lanzamos error persistente afuera
            Debug.Log("Monedas insuficientes.");
            CerrarTienda();

            if (textoPromptMundo != null)
            {
                textoPromptMundo.text = mensajeError;
                textoPromptMundo.gameObject.SetActive(true);
                mostrandoError = true;

                if (rutinaError != null)
                {
                    StopCoroutine(rutinaError);
                }
                rutinaError = StartCoroutine(ManejarTextoErrorExterno());
            }
        }
    }

    private void ProcesarCompraNo()
    {
        Debug.Log("Compra cancelada por el jugador.");
        CerrarTienda();
    }

    private void CerrarTienda()
    {
        tiendaAbierta = false;
        esperandoRespuesta = false;

        if (panelTienda != null)
        {
            panelTienda.SetActive(false);
        }
    }

    private IEnumerator CerrarTiendaDespuesDeExito()
    {
        yield return new WaitForSecondsRealtime(tiempoMostrarExito);
        CerrarTienda();
        
        // Si el jugador sigue dentro del rango, restauramos el prompt inicial
        if (jugadorEnRango && textoPromptMundo != null)
        {
            textoPromptMundo.text = "E para interactuar";
            textoPromptMundo.gameObject.SetActive(true);
        }
    }

    private IEnumerator ManejarTextoErrorExterno()
    {
        yield return new WaitForSecondsRealtime(tiempoMostrandoError);

        mostrandoError = false;
        rutinaError = null;

        if (textoPromptMundo != null)
        {
            if (jugadorEnRango)
            {
                textoPromptMundo.text = "E para interactuar";
                textoPromptMundo.gameObject.SetActive(true);
            }
            else
            {
                textoPromptMundo.gameObject.SetActive(false);
            }
        }
    }
}