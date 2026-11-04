using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Paneles de UI")]
    [Tooltip("El Panel principal de pausa que contiene los botones.")]
    [SerializeField] private GameObject panelPausa;

    [Tooltip("El panel de configuración.")]
    [SerializeField] private GameObject panelConfiguracion;

    [Header("Escenas")]
    [Tooltip("Nombre exacto de la escena del Menú Principal.")]
    [SerializeField] private string nombreEscenaMenu = "MainMenu";

    private bool juegoPausado = false;

    private void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(false);
        Time.timeScale = 1f; // Aseguramos que el tiempo corra al iniciar
    }

    private void Update()
    {
        // Permitir pausar/despausar presionando Escape o P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    // ==========================================
    // CONTROL GENERAL DE PAUSA
    // ==========================================
    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f; // Detiene el tiempo

        if (panelPausa != null) panelPausa.SetActive(true);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(false);

        Debug.Log("Juego pausado.");
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f; // Restaura el tiempo normal

        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(false);

        Debug.Log("Juego reanudado.");
    }

    // ==========================================
    // BOTONES DE LA UI
    // ==========================================

    public void BotonContinuar() => ReanudarJuego();

    public void BotonAbrirConfiguracionPausa()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(true);
    }

    public void BotonCerrarConfiguracionPausa()
    {
        if (panelConfiguracion != null) panelConfiguracion.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(true);
    }

    public void BotonVolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}