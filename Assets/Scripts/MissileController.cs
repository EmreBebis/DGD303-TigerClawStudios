using UnityEngine;

public class MissileControler : MonoBehaviour
{
    public float speed = 10f; // Merminin hızı

    void Update()
    {
        // Mermiyi ileri doğru hareket ettir
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer çarpışan obje "Enemy" tag'ine sahipse
        if (collision.CompareTag("Enemy"))
        {
            // Düşmanı yok et
            Destroy(collision.gameObject);

            // Kendini (mermiyi) yok et
            Destroy(gameObject);
        }
    }
}
