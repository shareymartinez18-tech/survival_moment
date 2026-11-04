using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class SceneLoaderButton : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Escribe exactamente el nombre de la escena a la que quieres ir.")]
    public string nombreEscena;

    // Esta es la función que debes arrastrar al evento OnClick() del botón
    public void CargarEscena()
    {
        // Validamos que el nombre no esté vacío antes de cargar
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            // IMPORTANTE: Restauramos el tiempo por si el juego estaba pausado (ej. tras un Game Over)
            Time.timeScale = 1f;
            
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena está vacío en el script de este botón.");
        }
    }
}