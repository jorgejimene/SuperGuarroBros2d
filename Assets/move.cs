using UnityEngine;
using UnityEngine.InputSystem;

public class move : MonoBehaviour
{
    Vector2 moveDirection;
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        var vec2 = moveDirection * (Time.deltaTime * 10);
        transform.Translate(vec2.x, 0, vec2.y);
    }
    private void HandleMovement()
    {
        float movex = 0f;
        float movey = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            movey += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movey = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movex = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movex += 1f;
        }
        Vector3 moveDir = new Vector3(movex, 0, movey).normalized;
        transform.position += moveDir * 20 * Time.deltaTime;
    }
}