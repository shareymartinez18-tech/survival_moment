using UnityEngine;
using UnityEngine.UI;

public class Cambiovolumen : MonoBehaviour
{
    [Header("Controles de Configuración")]
    [SerializeField] private Slider sliderVolumen;
    [SerializeField] private Toggle toggleMutear;

    private void Start()
    {
        // 1. Inicializamos el Slider con el volumen global actual
        if (sliderVolumen != null)
        {
            sliderVolumen.value = AudioListener.volume;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        // 2. Inicializamos el Toggle con el estado de pausa global actual
        if (toggleMutear != null)
        {
            toggleMutear.isOn = AudioListener.pause;
            toggleMutear.onValueChanged.AddListener(CambiarMute);
        }
    }

    private void CambiarVolumen(float nuevoVolumen)
    {
        AudioListener.volume = nuevoVolumen;
        PlayerPrefs.SetFloat("VolumenJuego", nuevoVolumen);
        PlayerPrefs.Save();
        Debug.Log("Volumen ajustado a: " + nuevoVolumen);
    }

    private void CambiarMute(bool estaMutado)
    {
        AudioListener.pause = estaMutado;
        PlayerPrefs.SetInt("JuegoMute", estaMutado ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Música mutada: " + estaMutado);
    }
}