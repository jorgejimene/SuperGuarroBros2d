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
                    Vector3 myPosition = transform.position; // The Fist/Trap center
                    Vector3 victimPosition = other.transform.position; // The Player

                    // B. Calculate Direction: (Victim - Attacker)
                    // .normalized makes the length always 1, so only direction matters
                    Vector2 pushDirection = (victimPosition - myPosition).normalized;

                    // Optional: Add a little upward pop so they don't drag on the ground
                    // pushDirection.y = 0.5f; 

                    // C. Call the function on the player
                    // We use 'moveDistance' as the Force Strength now.
                    // You might want to increase moveDistance in the inspector (try 10 or 15)
                    //playerScript.ApplyKnockback(pushDirection, moveDistance);

                    Debug.Log($"Punch applied in direction: {pushDirection}");
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
