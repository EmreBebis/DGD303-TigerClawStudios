using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f; // Düşmanın hareket hızı
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public float explosionEffectDuration = 3f; // Patlama efektinin sahnede kalacağı süre

    private bool isDestroyed = false; // Tekrarlayan patlama efektlerini önlemek için kontrol
    private bool isOffScreen = false; // Düşmanın ekran dışına çıkıp çıkmadığını kontrol eder

    void Start()
    {
        // Geminin yönünü aşağıya çevirmek
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    void Update()
    {
        // Düşmanı aşağı doğru hareket ettir
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
    }

    private void OnDestroy()
    {
        // Eğer düşman ekran dışına çıktığı için yok ediliyorsa patlama efekti veya sesi tetikleme
        if (isOffScreen)
        {
            return;
        }

        // Patlama efekti oluştur
        if (!isDestroyed && explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // Patlama efektini belirli bir süre sonra yok et
            Destroy(explosion, explosionEffectDuration);

            // Tekrar patlama oluşmasını engelle
            isDestroyed = true;
        }
    }

    private void OnBecameInvisible()
    {
        // Düşman ekran dışına çıktığında sessizce yok et
        isOffScreen = true;
        Destroy(gameObject);
    }
}
