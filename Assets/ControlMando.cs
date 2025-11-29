using UnityEngine;
using UnityEngine.InputSystem;

public class ControlMando : MonoBehaviour
{
    public AnimationPunchInterface punchLeftInterface;
    public AnimationPunchInterface punchRightInterface;
    public AnimationPunchInterface punchUpInterface;
    
    public InputActionAsset InputActions;
    private InputAction mAtackAction;
    public GameObject other;

    // Acciones separadas para diferentes direcciones de ataque
    private InputAction attackLeftAction;
    private InputAction attackRightAction;
    private InputAction attackUpAction;

    void Awake(){
        mAtackAction = InputActions.FindAction("Attack");
        
        // Crear acciones específicas para cada dirección
        attackLeftAction = new InputAction("AttackLeft", InputActionType.Button);
        attackRightAction = new InputAction("AttackRight", InputActionType.Button);
        attackUpAction = new InputAction("AttackUp", InputActionType.Button);
        
        // Asignar bindings SOLO para mando
        attackLeftAction.AddBinding("<Gamepad>/buttonWest");  // Botón X en Xbox, Cuadrado en PlayStation
        attackRightAction.AddBinding("<Gamepad>/buttonEast"); // Botón B en Xbox, Círculo en PlayStation
        attackUpAction.AddBinding("<Gamepad>/buttonNorth");   // Botón Y en Xbox, Triángulo en PlayStation
        
        // Alternativa: usar los gatillos o bumpers
        // attackLeftAction.AddBinding("<Gamepad>/leftShoulder");
        // attackRightAction.AddBinding("<Gamepad>/rightShoulder");
        // attackUpAction.AddBinding("<Gamepad>/leftTrigger");
    }

    private void OnEnable()
    {
        // Activar la acción general de ataque
        if (mAtackAction != null)
        {
            mAtackAction.performed += OnAttack; 
            mAtackAction.Enable();
        }
        
        // Activar y configurar las acciones específicas
        attackLeftAction.performed += OnAttackLeft;
        attackRightAction.performed += OnAttackRight;
        attackUpAction.performed += OnAttackUp;
        
        attackLeftAction.Enable();
        attackRightAction.Enable();
        attackUpAction.Enable();
    }

    private void OnDisable()
    {
        // Desactivar la acción general
        if (mAtackAction != null)
        {
            mAtackAction.performed -= OnAttack;
            mAtackAction.Disable();
        }
        
        // Desactivar las acciones específicas
        attackLeftAction.performed -= OnAttackLeft;
        attackRightAction.performed -= OnAttackRight;
        attackUpAction.performed -= OnAttackUp;
        
        attackLeftAction.Disable();
        attackRightAction.Disable();
        attackUpAction.Disable();
    }

    // Método general de ataque
    private void OnAttack(InputAction.CallbackContext context)
    {
        // Solo procesar si es un mando
        if (!(context.control.device is Gamepad))
        {
            return;
        }

        string controlQueSePulso = context.control.displayName;
        Debug.Log("¡Ataque con mando disparado por: " + controlQueSePulso + "!");

        // Usar el stick derecho para determinar dirección
        Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
        DetermineAttackDirection(rightStick);
    }

    // Métodos específicos para cada dirección con botones del mando
    private void OnAttackLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Ataque izquierdo (botón mando)");
        punchLeftInterface.Atack();
        onVictimAtacked();
    }

    private void OnAttackRight(InputAction.CallbackContext context)
    {
        Debug.Log("Ataque derecho (botón mando)");
        punchRightInterface.Atack();
        onVictimAtacked();
    }

    private void OnAttackUp(InputAction.CallbackContext context)
    {
        Debug.Log("Ataque arriba (botón mando)");
        punchUpInterface.Atack();
        onVictimAtacked();
    }

    // Método para determinar dirección basado en stick
    private void DetermineAttackDirection(Vector2 stickInput)
    {
        float deadZone = 0.5f;
        
        if (stickInput.magnitude > deadZone)
        {
            if (Mathf.Abs(stickInput.x) > Mathf.Abs(stickInput.y))
            {
                // Movimiento horizontal predominante
                if (stickInput.x > 0)
                {
                    Debug.Log("Ataque derecho (stick)");
                    punchRightInterface.Atack();
                }
                else
                {
                    Debug.Log("Ataque izquierdo (stick)");
                    punchLeftInterface.Atack();
                }
            }
            else
            {
                // Movimiento vertical predominante
                if (stickInput.y > 0)
                {
                    Debug.Log("Ataque arriba (stick)");
                    punchUpInterface.Atack();
                }
            }
            onVictimAtacked();
        }
    }

    private void onVictimAtacked(){
        if (other == null) return;

        Vector3 myPos = transform.position;
        Vector3 victimPos = other.transform.position;

        Vector2 knockbackDir = (victimPos - myPos).normalized;
        knockbackDir.y = 0.2f; 

        float punchForce = 10f;
        
        var victimScript = other.GetComponent<PlayerMovement>();
        if (victimScript != null)
        {
            victimScript.ApplyKnockback(knockbackDir, punchForce, 20);
        }
    }

    void Update()
    {
        // Detección de stick derecho en Update para ataques con stick
        if (Gamepad.current != null)
        {
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
            if (rightStick.magnitude > 0.5f && mAtackAction.WasPressedThisFrame())
            {
                DetermineAttackDirection(rightStick);
            }
        }
    }

    void OnDestroy()
    {
        // Limpiar las acciones creadas
        attackLeftAction?.Dispose();
        attackRightAction?.Dispose();
        attackUpAction?.Dispose();
    }
}