using UnityEngine;

public class TriggerZone2D : MonoBehaviour
{
    public AnimationPunchInterface animationObject;

    [Range(0, 1)] public float alturaDelArco = 0.8f; // 0 = Raso, 1 = 45 grados hacia arriba
    [SerializeField] float moveDistance = 1.0f; // Una distancia m�s razonable que 122.1
    [SerializeField] string requiredTag = "Player"; // leave empty to accept any
    public float fuerzaGolpe = 100f;      // Fuerza bruta (SÚBELA a 20, 30 o 50)
    public float tiempoAturdimiento = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Filtro de Tag y Estado
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;

        // Si tienes el objeto de animación, comprobamos si está visible
        if (animationObject != null && !animationObject.gameObject.activeSelf) return;

        Debug.Log("¡CONTACTO CON TRAMPA!");

        // 2. Obtener el script de movimiento del jugador
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            // --- AQUÍ ESTÁ EL CAMBIO CLAVE: CÁLCULO DE DIRECCIÓN FORZADO ---

            // A. Calculamos solo si está a la izquierda o a la derecha (-1 o 1)
            // Esto evita problemas cuando los objetos están superpuestos.
            float direccionX = Mathf.Sign(other.transform.position.x - transform.position.x);

            // B. Creamos un vector nuevo "artificial"
            // X = -1 o 1 (Empuje lateral fuerte)
            // Y = alturaDelArco (Empuje hacia arriba para despegarlo del suelo)
            Vector2 direccionFinal = new Vector2(direccionX, alturaDelArco).normalized;

            // 3. Enviamos el golpe al jugador
            // Usamos tu función ApplyKnockback
            player.ApplyKnockback(direccionFinal, fuerzaGolpe, tiempoAturdimiento);

            Debug.Log($"Golpe aplicado: Dir={direccionFinal} Fuerza={fuerzaGolpe}");
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
