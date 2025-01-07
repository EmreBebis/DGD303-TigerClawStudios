using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Klavye girişlerini al
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D veya Sol/Sağ okları
        float verticalInput = Input.GetAxisRaw("Vertical"); // W/S veya Yukarı/Aşağı okları

        // Hareketi hesapla
        movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
    }

    void FixedUpdate()
    {
        // Hareketi uygula
        rb.linearVelocity = movement;
    }
}
