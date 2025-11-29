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
        Debug.Log("Nos fuimo " + other.gameObject.name);

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

        // Truco de altura
        pushDirection.y = 0.5f;
        pushDirection = pushDirection.normalized;

        // --- AQUÍ ESTÁ EL CAMBIO ---

        // Obtenemos el script del jugador
        PlayerMovement playerScript = other.GetComponent<PlayerMovement>();

        if (playerScript != null)
        {
            float fuerzaDelGolpe = 50f; // 400 es una barbaridad, prueba con 20 o 30 primero
            float tiempoStun = 0.5f;    // Medio segundo sin control

            // EN LUGAR DE rb.AddForce, LLAMAMOS A LA FUNCIÓN DEL PLAYER
            // Esto se encarga de bloquear el movimiento, empujar y esperar.
            playerScript.ApplyKnockback(pushDirection, fuerzaDelGolpe, tiempoStun);
        }


    }
}
