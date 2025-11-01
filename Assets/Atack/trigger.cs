using UnityEngine;

public class TriggerZone2D : MonoBehaviour
{
    
    [SerializeField] float moveDistance = 1.0f; // Una distancia m�s razonable que 122.1
    [SerializeField] string requiredTag = "Player"; // leave empty to accept any
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // if (string.IsNullOrEmpty(requiredTag) || other.CompareTag(requiredTag))
        //Debug.Log($"Mecagune tu pusitima madre");
        Debug.Log("Entramos");
        if (other.CompareTag(requiredTag))
        {
           Debug.Log("Nos fuimo");
           // �No uses GameObject.Find! Usa 'other'.
           // Segundo, obtenemos su Rigidbody2D para moverlo de forma segura
           Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

           if (playerRb != null)
           {
               // Calculamos la nueva posici�n
               // rb.position = su posici�n actual
               // Vector2.left = (-1, 0)
               Vector2 newPosition = playerRb.position + (Vector2.left * moveDistance);

               // Usamos MovePosition para mover el objeto de f�sicas
               playerRb.MovePosition(newPosition);

               Debug.Log($"El jugador ha salido y se ha movido {moveDistance} unidades a la izquierda.");
           }
           else
           {
               // Esto pasar�a si el "Player" no tiene un Rigidbody2D
               Debug.LogWarning("El 'Player' sali� del trigger pero no tiene Rigidbody2D.");
           }
        }
    }

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
