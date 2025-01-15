using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float speed = 2f; // Asteroidin düşüş hızı
    public float minRotationSpeed = 50f; // Minimum dönüş hızı
    public float maxRotationSpeed = 150f; // Maksimum dönüş hızı
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public float explosionEffectDuration = 3f; // Patlama efektinin sahnede kalma süresi

    private float rotationSpeed; // Rastgele dönüş hızı
    private bool isOffScreen = false; // Asteroid ekran dışına çıktı mı kontrolü

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

    private void OnDestroy()
    {
        // Eğer asteroid ekran dışına çıktığı için yok ediliyorsa patlama efekti veya sesi tetikleme
        if (isOffScreen)
        {
            return;
        }

        // Patlama efekti oluştur
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // Patlama efektini belirli bir süre sonra yok et
            Destroy(explosion, explosionEffectDuration);
        }
    }

    private void OnBecameInvisible()
    {
        // Asteroid ekran dışına çıktığında sessizce yok et
        isOffScreen = true;
        Destroy(gameObject);
    }
}
