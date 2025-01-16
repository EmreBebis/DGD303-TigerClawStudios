using UnityEngine;

public class PizzaController : MonoBehaviour
{
    public float speed = 2f; // Pizzanın düşüş hızı

    void Update()
    {
        // Pizzayı aşağı doğru hareket ettir
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        // Pizza ekran dışına çıktıysa yok et
        Destroy(gameObject);
    }
}