using UnityEngine;

public enum MovementAxis
{
    X,
    Y
}

public class BombMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Elige si la bomba se moverá horizontalmente (X) o verticalmente (Y).")]
    public MovementAxis ejeMovimiento = MovementAxis.X;

    [Tooltip("Distancia máxima que se alejará desde su punto de origen.")]
    public float distancia = 3f;

    [Tooltip("Velocidad a la que se desplaza la bomba.")]
    public float velocidad = 2f;

    private Vector2 posicionInicial;

    void Start()
    {
        // Guardamos la posición en la que empieza la bomba (Punto A / Origen)
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Mathf.PingPong crea un ciclo suave de 0 hasta 'distancia' y vuelve a 0
        float desplazamiento = Mathf.PingPong(Time.time * velocidad, distancia);

        Vector2 nuevaPosicion = posicionInicial;

        // Movemos la bomba en el eje seleccionado sumando el desplazamiento
        if (ejeMovimiento == MovementAxis.X)
        {
            nuevaPosicion.x += desplazamiento;
        }
        else
        {
            nuevaPosicion.y += desplazamiento;
        }

        transform.position = nuevaPosicion;
    }
}
