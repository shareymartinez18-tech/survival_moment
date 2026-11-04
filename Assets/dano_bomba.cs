using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Configuración de la Bomba")]
    public int dano = 15; // Cantidad de daño que inflige la bomba

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si el objeto que colisiona tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script de salud en el jugador (respetando la minúscula 'salud')
            salud saludPlayer = collision.GetComponent<salud>();

            if (saludPlayer != null)
            {
                // Restamos el daño a los puntos de salud
                saludPlayer.puntosDeSalud -= dano;

                // Opcional: Asegurarnos de que la salud no baje de 0 (puedes quitarlo si tienes tu propia lógica de muerte)
                if (saludPlayer.puntosDeSalud < 0)
                {
                    saludPlayer.puntosDeSalud = 0;
                }

                // Mensaje en consola para verificar el impacto
                Debug.Log($"¡El jugador pisó una bomba! Daño recibido: {dano}. Salud restante: {saludPlayer.puntosDeSalud}");
            }
            else
            {
                Debug.LogWarning("No se encontró el script 'salud' en el Player.");
            }

            // Destruye la bomba al explotar/tocar al jugador
            Destroy(gameObject);
        }
    }
}
