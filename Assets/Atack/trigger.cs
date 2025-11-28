using UnityEngine;

public class TriggerZone2D : MonoBehaviour
{
    public AnimationPunchInterface animationObject;
    
    [SerializeField] float moveDistance = 1.0f; // Una distancia m�s razonable que 122.1
    [SerializeField] string requiredTag = "Player"; // leave empty to accept any
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entramos");

        //if (other.CompareTag(requiredTag))
        //{
        Debug.Log("Collider ", other);
        Debug.Log("Nos fuimo "+ other.gameObject.name);

        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

        // 1. Get the PlayerMovement script
        // (Changed variable name to 'playerScript' to be less confusing than 'playerRb')
        //PlayerMovement playerScript = other.GetComponent<PlayerMovement>();

        //if (playerScript != null)
        //{
        // Only punch if the animation/object is visible
        //if (!animationObject.isHide())
        //{
        // --- THE MATH IS HERE ---

        // A. Get positions
        // ... (Tu código anterior para calcular pushDirection) ...

        // A. Obtener posiciones
        Vector3 myPosition = transform.position;
        Vector3 victimPosition = other.transform.position;

        // B. Calcular Dirección
        Vector2 pushDirection = (victimPosition - myPosition).normalized;

        // --- AQUÍ ESTÁ EL CAMBIO ---

        // 1. Decidimos cuánta fuerza/distancia usar (asegúrate de que moveDistance tenga valor, ej: 2.0f)
        // Si moveDistance es muy pequeño (0.1), apenas se notará.
        float fuerza = moveDistance;

        // 2. Calculamos el DESTINO final
        // "Donde estás ahora" + "Hacia allá" * "Tantos metros"
        Vector2 targetPosition = playerRb.position + (pushDirection * fuerza);

        // 3. Movemos al Rigidbody a esa nueva posición
        playerRb.MovePosition(targetPosition);

        Debug.Log($"Jugador movido a: {targetPosition}");
        //}



        //}
        //else
        //{
        //    Debug.LogWarning("Player found, but PlayerMovement script is missing!");
        //}
    }
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    Debug.Log("Entramos");
    //    if (other.CompareTag(requiredTag))
    //    {
    //        Debug.Log("Nos fuimo");
    //        // �No uses GameObject.Find! Usa 'other'.
    //        // Segundo, obtenemos su Rigidbody2D para moverlo de forma segura
    //        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

    //        if (playerRb != null)
    //        {
    //            // Calculamos la nueva posici�n
    //            // rb.position = su posici�n actual
    //            // Vector2.left = (-1, 0)
    //            Vector2 newPosition = playerRb.position + (Vector2.left * moveDistance);

    //            // Usamos MovePosition para mover el objeto de f�sicas
    //            playerRb.MovePosition(newPosition);

    //            Debug.Log($"El jugador ha salido y se ha movido {moveDistance} unidades a la izquierda.");
    //        }
    //        else
    //        {
    //            // Esto pasar�a si el "Player" no tiene un Rigidbody2D
    //            Debug.LogWarning("El 'Player' sali� del trigger pero no tiene Rigidbody2D.");
    //        }
    //    }
    // }
}
