using UnityEngine;

public class AnimationPunchInterface : MonoBehaviour
{
    public GameObject renderer;

    // Es mejor usar Awake para obtener componentes
    private void Awake()
    {
        
        if (renderer == null)
        {
            Debug.LogError("¡No se encontró un SpriteRenderer en este objeto!");
        }
    }

    private void Start()
    {
        // Empezamos con el renderer DESACTIVADO (invisible)
        if (renderer != null)
        {
            renderer.SetActive(false); 
        }
    }

    // Función para HACERLO VISIBLE (corregido)
    // 1. Añadido "void"
    // 2. Corregida la lógica
    // 3. Corregida la ortografía de "Attack"
    public void ShowAttack()
    {
        if (renderer != null)
        {
            Debug.Log("PUÑO MARICON");
            renderer.SetActive(true);  // true = VISIBLE
           
            
        }
    }

    public void Atack(){
        ShowAttack();
        // Invoke("SpawnDelay", 3);
        // HideAttack();
    }

    // También querrás una función para volver a ocultarlo
    public void HideAttack()
    {
        if (renderer != null)
        {
            Debug.Log("PUÑO MARICON HIDE");
            renderer.SetActive(false);  // false = INVISIBLE
        }
    }

    public bool isHide(){
        return  !renderer.activeInHierarchy;
    }
}