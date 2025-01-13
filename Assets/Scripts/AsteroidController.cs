using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float speed = 2f; // Asteroidin düşüş hızı
    public float minRotationSpeed = 50f; // Minimum dönüş hızı
    public float maxRotationSpeed = 150f; // Maksimum dönüş hızı
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public float explosionEffectDuration = 3f; // Patlama efektinin sahnede kalma süresi

    private float rotationSpeed; // Rastgele dönüş hızı

    void Start()
    {
        // Asteroid için rastgele bir dönüş hızı belirle
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    void Update()
    {
        // Asteroidi aşağı doğru hareket ettir
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // Asteroidi döndür
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnDestroy()
    {
        // Eğer patlama efekti prefab'ı tanımlıysa
        if (explosionEffectPrefab != null)
        {
            // Mevcut pozisyonda patlama efekti oluştur
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // Patlama efektini belirli bir süre sonra yok et
            Destroy(explosion, explosionEffectDuration);
        }
    }

    private void OnBecameInvisible()
    {
        // Asteroid ekran dışına çıktıysa yok et
        Destroy(gameObject);
    }
}