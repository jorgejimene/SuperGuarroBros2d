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
        pushDirection.y = 0.5f;

        // (Opcional) Normalizamos otra vez para que la fuerza total no se dispare,
        // aunque a veces mola que el golpe hacia arriba sea más fuerte.
        pushDirection = pushDirection.normalized;

        // -------------------------------

        // 1. Resetear velocidad (para que el golpe sea seco)
        playerRb.linearVelocity = Vector2.zero;

        // 2. Fuerza
        float fuerzaDelGolpe = 5; // Asegúrate de poner esto a 10 o 20 en el Inspector

        // 3. Aplicar fuerza IMPULSE
        playerRb.AddForce(pushDirection * fuerzaDelGolpe, ForceMode2D.Impulse);

        Debug.Log("¡A volar!");
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
