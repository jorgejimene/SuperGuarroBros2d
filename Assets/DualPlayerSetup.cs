using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAssigner : MonoBehaviour
{
    [Header("Control Assignment")]
    public bool useKeyboard = true;
    
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        AssignInputDevice();
    }

    void AssignInputDevice()
    {
        if (useKeyboard)
        {
            // Asignar teclado
            if (Keyboard.current != null)
            {
                playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", 
                    new InputDevice[] { Keyboard.current, Mouse.current });
                Debug.Log($"{gameObject.name} asignado a Teclado");
            }
        }
        else
        {
            // Asignar gamepad
            if (Gamepad.current != null)
            {
                playerInput.SwitchCurrentControlScheme("Gamepad", 
                    new InputDevice[] { Gamepad.current });
                Debug.Log($"{gameObject.name} asignado a Gamepad");
            }
            else
            {
                Debug.LogWarning("No hay gamepad disponible, usando teclado como fallback");
                playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", 
                    new InputDevice[] { Keyboard.current, Mouse.current });
            }
        }
    }

    // Método para cambiar dispositivo en tiempo de ejecución
    public void SwitchToKeyboard()
    {
        useKeyboard = true;
        AssignInputDevice();
    }

    public void SwitchToGamepad()
    {
        useKeyboard = false;
        AssignInputDevice();
    }
}