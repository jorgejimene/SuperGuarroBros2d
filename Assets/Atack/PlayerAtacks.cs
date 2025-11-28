using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAtacks : MonoBehaviour
{
    public AnimationPunchInterface punchLeftInterface;
    public AnimationPunchInterface punchRightInterface;
    public AnimationPunchInterface punchUpInterface;
    
    public InputActionAsset InputActions;
    private InputAction mAtackAction;
    public GameObject other;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake(){
        mAtackAction = InputActions.FindAction("Attack");
    }

    private void OnEnable()
    {
        // 3. ¡MUY IMPORTANTE! Debes activar la acción para usarla
        if (mAtackAction != null)
        {
            mAtackAction.performed += OnAttack; 
            
            // 4. Aún necesitamos activarla
            mAtackAction.Enable();
        }
    }

    // OnDisable se llama cuando el objeto se desactiva
    private void OnDisable()
    {
        // 4. Es buena práctica desactivarla
        if (mAtackAction != null)
        {
            mAtackAction.performed -= OnAttack;
            mAtackAction.Disable();
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        // El 'context' tiene toda la información
        
        // context.control.displayName nos da el nombre "bonito" del botón
        string controlQueSePulso = context.control.displayName;

        Debug.Log("¡Ataque peruano disparado por: " + controlQueSePulso + "!");

        // Ahora puedes diferenciar
        if (controlQueSePulso == "J") //Left
        {
            Debug.Log("¡Fue la tecla J!");
            punchLeftInterface.Atack();
            // Hacer la acción de la tecla L...
        }
        else if (controlQueSePulso == "L") //Right
        {
            Debug.Log("¡Fue la tecla L!");
            punchRightInterface.Atack();
            // Hacer la acción de la tecla L...
        }
         else if (controlQueSePulso == "I") //Up
        {
            Debug.Log("¡Fue la tecla I!");
            punchUpInterface.Atack();
            // Hacer la acción de la tecla L...
        }
        else if (controlQueSePulso == "Enter")
        {
            Debug.Log("¡Fue la tecla Enter!");
            // Hacer la acción de la tecla Enter...
        }
        else if (controlQueSePulso == "Left Button")
        {
            Debug.Log("¡Fue el botón izquierdo del ratón!");
            // Hacer la acción del ratón...
        }
        onVictimAtacked();
        
    }

    private void onVictimAtacked(){
        Vector3 myPos = transform.position; // The Attacker (or the Fist)
        Vector3 victimPos = other.transform.position; // The Victim

        // 2. Calculate Direction: (Target - Origin)
        Vector2 knockbackDir = (victimPos - myPos).normalized;

        // 3. (Optional) Flatten the vector if you only want horizontal push
        // knockbackDir.y = 0; 
        // knockbackDir.x = Mathf.Sign(knockbackDir.x); // Returns 1 or -1

        // 4. (Optional) Add a little "Pop Up" so they don't drag on the floor
        knockbackDir.y = 0.2f; 

        // 5. Send the force to the player
        float punchForce = 10f; // How strong is the punch?
        
        // Get the script and call the function we created in Step 1
        var victimScript = other.GetComponent<PlayerMovement>();
        if (victimScript != null)
        {
            victimScript.ApplyKnockback(knockbackDir, punchForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if(mAtackAction.WasPressedThisFrame()){
        //     Debug.Log("Ataque peruano");
        // }
    }
}
