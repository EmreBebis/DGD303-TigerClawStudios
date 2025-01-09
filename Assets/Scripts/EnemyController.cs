using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed; // Düşmanın hareket hızı
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public float explosionEffectDuration = 3f; // Patlama efektinin sahnede kalacağı süre

    void Update()
    {
        // Düşmanı aşağı doğru hareket ettir
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnDestroy()
    {
        // Eğer patlama efekti prefab'ı tanımlıysa
        if (explosionEffectPrefab != null)
        {
            // Mevcut pozisyonda patlama efekti oluştur
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // Patlama efektini belirli bir süre sonra yok etmek
            Destroy(explosion, explosionEffectDuration);
        }
    }
}
