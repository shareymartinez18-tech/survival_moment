using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Referencias del Jugador")]
    [Tooltip("Arrastra aquí a tu objeto Player, o déjalo vacío para que lo busque automáticamente por la etiqueta 'Player'.")]
    public GameObject jugador;

    [Header("Textos de la UI (TextMeshPro)")]
    public TMP_Text textoMonedas;
    public TMP_Text textoGemas;
    public TMP_Text textoLlaves;
    public TMP_Text textoSalud;

    // Referencias a los scripts del jugador
    private Inventory inventarioPlayer;
    private salud saludPlayer; // Coincide con la minúscula de tu script

    void Start()
    {
        // Si no asignaste el jugador en el inspector, lo busca por la etiqueta "Player"
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                jugador = playerObj;
            }
        }

        if (jugador != null)
        {
            // Obtenemos los scripts del jugador
            inventarioPlayer = jugador.GetComponent<Inventory>();
            saludPlayer = jugador.GetComponent<salud>();

            if (inventarioPlayer == null)
                Debug.LogWarning("UIManager: No se encontró el script 'Inventory' en el jugador.");
            
            if (saludPlayer == null)
                Debug.LogWarning("UIManager: No se encontró el script 'salud' en el jugador.");
        }
        else
        {
            Debug.LogError("UIManager: No se encontró ningún objeto con la etiqueta 'Player' en la escena.");
        }
    }

    void Update()
    {
        // Actualizamos los textos frame a frame mostrando el nombre y el valor actual
        if (inventarioPlayer != null)
        {
            if (textoMonedas != null) textoMonedas.text = "Monedas: " + inventarioPlayer.Monedas;
            if (textoGemas != null) textoGemas.text = "Gemas: " + inventarioPlayer.Gemas;
            if (textoLlaves != null) textoLlaves.text = "Llaves: " + inventarioPlayer.Llaves;
        }

        if (saludPlayer != null)
        {
            if (textoSalud != null) textoSalud.text = "Salud: " + saludPlayer.puntosDeSalud;
        }
    }
}
