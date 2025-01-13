using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f; // Düşmanın hareket hızı
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public float explosionEffectDuration = 3f; // Patlama efektinin sahnede kalacağı süre

    private bool isDestroyed = false; // Tekrarlayan patlama efektlerini önlemek için kontrol

    void Update()
    {
        // Düşmanı aşağı doğru hareket ettir
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnDestroy()
    {
        // Eğer patlama efekti prefab'ı tanımlıysa ve henüz yok edilmediyse
        if (!isDestroyed && explosionEffectPrefab != null)
        {
            // Mevcut pozisyonda patlama efekti oluştur
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // Patlama efektini belirli bir süre sonra yok etmek
            Destroy(explosion, explosionEffectDuration);

            // Tekrar patlama oluşmasını engelle
            isDestroyed = true;
        }
    }

    private void OnBecameInvisible()
    {
        // Eğer düşman ekran dışında görünmez hale geldiyse, yok et
        Destroy(gameObject);
    }
}