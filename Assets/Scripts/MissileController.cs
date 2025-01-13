using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float speed = 10f; // Merminin hızı
    public float lifetime = 5f; // Merminin sahnede kalma süresi
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı

    void Start()
    {
        // Mermiyi belirli bir süre sonra yok et
        Destroy(gameObject, lifetime);
    }

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

            // Patlama efekti oluştur
            CreateExplosion();
        }
        // Eğer çarpışan obje "Asteroid" tag'ine sahipse
        else if (collision.CompareTag("Asteroid"))
        {
            // Asteroidi yok et
            Destroy(collision.gameObject);

            // Patlama efekti oluştur
            CreateExplosion();
        }
        else if (!collision.CompareTag("Player")) // Oyuncuya çarpmayı engelle
        {
            // Diğer objelerle çarpışınca mermiyi yok et
            Destroy(gameObject);
        }
    }

    private void CreateExplosion()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        // Kendini (mermiyi) yok et
        Destroy(gameObject);
    }
}