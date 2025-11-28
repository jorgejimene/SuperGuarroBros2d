using UnityEngine;

public class FollowPlayerMapLogic : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;

    // Usamos LateUpdate para un seguimiento suave
    void LateUpdate()
    {
        // 1. Comprobamos si el 'player' ha sido asignado
        //    (Si no, daría un error NullReference)
        if (player == null)
        {
            Debug.LogWarning("¡El objeto a seguir (Player) no está asignado!");
            return; // Sal de la función si no hay nada que seguir
        }

        // 2. Calculamos la nueva posición X e Y
        //    Convertimos la posición del player (Vector3) a un Vector2
        //    y le sumamos nuestro offset 2D.
        Vector2 desiredPosition = (Vector2)player.position + offset;
        
        // 3. Creamos la posición 3D final
        //    Usamos la X e Y que calculamos, pero mantenemos
        //    la Z original de este objeto (¡importante en 2D!).
        Vector3 newPosition = new Vector3(desiredPosition.x, desiredPosition.y, transform.position.z);

        // 4. Aplicamos la nueva posición
        transform.position = newPosition;
    }
}
