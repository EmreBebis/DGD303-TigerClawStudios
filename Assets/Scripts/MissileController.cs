using UnityEngine;

public class MissileController : MonoBehaviour
{
    AudioManager audiomanager;

    private void Awake()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public float speed = 10f; // Merminin hızı
    public float lifetime = 5f; // Merminin sahnede kalma süresi
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı

    private Vector2 screenBounds; // Ekran sınırlarını kontrol etmek için

    void Start()
    {
        // Ekran sınırlarını hesapla
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Mermiyi belirli bir süre sonra yok et
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mermiyi ileri doğru hareket ettir
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Ekran sınırlarını kontrol et
        if (transform.position.x > screenBounds.x || transform.position.x < -screenBounds.x ||
            transform.position.y > screenBounds.y || transform.position.y < -screenBounds.y)
        {
            Destroy(gameObject); // Ekran dışına çıkan mermiyi yok et
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer çarpışan obje "Enemy" tag'ine sahipse
        if (collision.CompareTag("Enemy"))
        {
            audiomanager.PlaySFX(audiomanager.enemyExplode);

            // Düşmanı yok et
            Destroy(collision.gameObject);

            // Patlama efekti oluştur
            CreateExplosion();
        }
        // Eğer çarpışan obje "Asteroid" tag'ine sahipse
        else if (collision.CompareTag("Asteroid"))
        {
            audiomanager.PlaySFX(audiomanager.enemyExplode);

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
