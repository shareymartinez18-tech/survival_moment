using UnityEngine;

// Asignamos valores explícitos a los Enums para evitar que se desconfigure el Inspector si modificas el orden en el futuro

public class recolectable : MonoBehaviour
{
    [Header("Configuración")]
    public CollectibleType tipoRecolectable; // Selección en el Inspector
    public int cantidadAOtorgar = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory inventarioPlayer = collision.GetComponent<Inventory>();

            if (inventarioPlayer != null)
            {
                // Sumamos según el tipo de objeto seleccionado
                switch (tipoRecolectable)
                {
                    case CollectibleType.Moneda:
                        inventarioPlayer.Monedas += cantidadAOtorgar;
                        break;
                    case CollectibleType.Gema:
                        inventarioPlayer.Gemas += cantidadAOtorgar;
                        break;
                    case CollectibleType.Llave:
                        inventarioPlayer.Llaves += cantidadAOtorgar;
                        break;
                }

                // Mensaje en consola mostrando el inventario actualizado en tiempo real
                Debug.Log($"¡Recolectado! 🪙 Monedas: {inventarioPlayer.Monedas} | 💎 Gemas: {inventarioPlayer.Gemas} | 🔑 Llaves: {inventarioPlayer.Llaves}");

                // Destruye el objeto de la escena únicamente cuando se recolectó con éxito
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("No se encontró el script de Inventario en el Player.");
            }
        }
    }
}