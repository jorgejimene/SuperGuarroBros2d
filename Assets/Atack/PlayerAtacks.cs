using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAtacks : MonoBehaviour
{
    public AnimationPunchInterface punchLeftInterface;
    public InputActionAsset InputActions;
    private InputAction mAtackAction;

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
        if (controlQueSePulso == "L")
        {
            Debug.Log("¡Fue la tecla L!");
            punchLeftInterface.Atack();
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
    }

    // Update is called once per frame
    void Update()
    {
        // if(mAtackAction.WasPressedThisFrame()){
        //     Debug.Log("Ataque peruano");
        // }
    }
}
