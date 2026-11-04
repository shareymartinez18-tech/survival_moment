using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Opcional, por si usas TextMeshPro en los textos

public class MainMenuManager : MonoBehaviour
{
    [Header("Escenas")]
    [Tooltip("Nombre exacto de la escena del juego a cargar al presionar 'Iniciar'.")]
    [SerializeField] private string nombreEscenaJuego = "GameScene";

    [Header("Paneles de UI")]
    [Tooltip("El panel principal del menú (los botones de Inicio, Configuración, Créditos, Salir).")]
    [SerializeField] private GameObject panelMenuPrincipal;

    [Tooltip("El panel que contiene las opciones de configuración.")]
    [SerializeField] private GameObject panelConfiguracion;

    [Tooltip("El panel que contiene los créditos del juego.")]
    [SerializeField] private GameObject panelCreditos;

    [Header("Controles de Configuración")]
    [Tooltip("Slider para controlar el volumen general o de la música.")]
    [SerializeField] private Slider sliderVolumen;

    [Tooltip("Toggle para mutear o desmutear la música.")]
    [SerializeField] private Toggle toggleMutear;

    [Tooltip("Dropdown o botón hipotético para el cambio de idioma.")]
    [SerializeField] private TMP_Dropdown dropdownIdioma;

    private void Start()
    {
        // Al iniciar, nos aseguramos de mostrar el menú principal y ocultar los subpaneles
        MostrarMenuPrincipal();

        // Inicializamos los valores de configuración si están asignados
        if (sliderVolumen != null)
        {
            sliderVolumen.value = AudioListener.volume;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (toggleMutear != null)
        {
            toggleMutear.isOn = AudioListener.pause;
            toggleMutear.onValueChanged.AddListener(CambiarMute);
        }

        if (dropdownIdioma != null)
        {
            dropdownIdioma.onValueChanged.AddListener(CambiarIdioma);
        }
    }

    // ==========================================
    // 1. BOTÓN INICIO
    // ==========================================
    public void BotonIniciarJuego()
    {
        Debug.Log("Cargando escena: " + nombreEscenaJuego);
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    // ==========================================
    // 2. BOTONES DE CONFIGURACIÓN
    // ==========================================
    public void BotonAbrirConfiguracion()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(true);
        if (panelCreditos != null) panelCreditos.SetActive(false);
    }

    public void BotonCerrarConfiguracion()
    {
        MostrarMenuPrincipal();
    }

    private void CambiarVolumen(float nuevoVolumen)
    {
        AudioListener.volume = nuevoVolumen;
        Debug.Log("Volumen ajustado a: " + nuevoVolumen);
    }

    private void CambiarMute(bool estaMutado)
    {
        AudioListener.pause = estaMutado;
        Debug.Log("Música mutada: " + estaMutado);
    }

    private void CambiarIdioma(int indexIdioma)
    {
        string idiomaSeleccionado = dropdownIdioma != null ? dropdownIdioma.options[indexIdioma].text : "Desconocido";
        Debug.Log("Idioma cambiado a: " + idiomaSeleccionado);
    }

    // ==========================================
    // 3. BOTONES DE CRÉDITOS
    // ==========================================
    public void BotonAbrirCreditos()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(true);
    }

    public void BotonCerrarCreditos()
    {
        MostrarMenuPrincipal();
    }

    // ==========================================
    // 4. BOTÓN SALIR
    // ==========================================
    public void BotonSalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        
        // Cierra la aplicación si está compilada
        Application.Quit();

        // Si estás probando en el editor de Unity, esto detiene el Play Mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // ==========================================
    // MÉTODOS AUXILIARES
    // ==========================================
    private void MostrarMenuPrincipal()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(true);
        if (panelConfiguracion != null) panelConfiguracion.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(false);
    }
}