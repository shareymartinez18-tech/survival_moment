using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class ScoreManager : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto del Jugador que contiene el script Inventory.")]
    public GameObject objetoJugador;

    [Header("UI (TextMeshPro)")]
    public TMP_Text textoPuntaje;

    [Header("Configuración de Puntuación")]
    [Tooltip("Cuántos puntos otorga cada Moneda.")]
    public int puntosPorMoneda = 10;
    
    [Tooltip("Cuántos puntos otorga cada Gema.")]
    public int puntosPorGema = 50;

    // Referencia al inventario del jugador
    private Inventory inventarioPlayer;
    private int puntajeTotal = 0;

    void Start()
    {
        // Validamos que se haya asignado el objeto del jugador desde el inspector
        if (objetoJugador != null)
        {
            // Obtenemos el script de inventario directamente de ese objeto
            inventarioPlayer = objetoJugador.GetComponent<Inventory>();

            if (inventarioPlayer == null)
            {
                Debug.LogWarning("ScoreManager: El objeto asignado no tiene el script 'Inventory' acoplado.");
            }
        }
        else
        {
            Debug.LogError("ScoreManager: No se ha asignado ningún objeto en la casilla 'Objeto Jugador' desde el Inspector.");
        }
    }

    void Update()
    {
        if (inventarioPlayer != null)
        {
            // Calculamos el puntaje total usando las monedas y gemas del inventario
            puntajeTotal = (inventarioPlayer.Monedas * puntosPorMoneda) + (inventarioPlayer.Gemas * puntosPorGema);

            // Actualizamos el texto en la UI
            if (textoPuntaje != null)
            {
                textoPuntaje.text = "Puntaje: " + puntajeTotal;
            }
        }
    }
}
