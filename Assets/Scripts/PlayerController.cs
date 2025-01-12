using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı
    public GameObject bulletPrefab; // Mermi prefab'ı
    public Transform[] firePoints; // Birden fazla FirePoint
    public float bulletSpeed = 10f; // Merminin hızı
    public float bulletLifetime = 2f; // Merminin ömrü (saniye)

    public int playerHealth = 3; // Oyuncunun toplam canı
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public GameObject muzzleFlashEffectPrefab; // Muzzle flash efekti prefab'ı

    private Vector2 movement;

    void Update()
    {
        // Klavye girişlerini al
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D veya Sol/Sağ okları
        float verticalInput = Input.GetAxisRaw("Vertical"); // W/S veya Yukarı/Aşağı okları

        // Hareketi hesapla
        movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;

        // Ateş etme kontrolü
        if (Input.GetKeyDown(KeyCode.Space)) // Space tuşuna basıldığında
        {
            Fire();
        }
    }

    void FixedUpdate()
    {
        // Hareketi uygula
        transform.position += (Vector3)movement * Time.fixedDeltaTime;
    }

    void Fire()
    {
        if (bulletPrefab != null && firePoints.Length > 0)
        {
            foreach (Transform firePoint in firePoints)
            {
                if (firePoint != null)
                {
                    // Mermiyi oluştur
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                    // Mermiyi ileri doğru hareket ettir
                    bullet.transform.Translate(firePoint.up * bulletSpeed * Time.deltaTime);

                    // Mermiyi belirli bir süre sonra yok et
                    Destroy(bullet, bulletLifetime);

                    // Muzzle flash efekti
                    CreateMuzzleFlash(firePoint.position, firePoint.up);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer çarpışan obje "Enemy" tag'ine sahipse
        if (collision.CompareTag("Enemy"))
        {
            // Can azalt
            playerHealth--;

            // Eğer can 0 veya altına inerse
            if (playerHealth <= 0)
            {
                GameOver();
            }

            // Düşmanı yok et
            Destroy(collision.gameObject);

            // Patlama efekti oluştur
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            }
        }
    }

    void CreateMuzzleFlash(Vector2 position, Vector2 direction)
    {
        // Muzzle flash oluşturma efekti (kısa süreli ışık efekti)
        if (muzzleFlashEffectPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashEffectPrefab, position, Quaternion.identity);
            muzzleFlash.transform.up = direction; // Muzzle flash'ın ateş edilen yönle uyumlu olması
            Destroy(muzzleFlash, 0.1f); // Muzzle flash kısa süre sonra yok olacak
        }
    }

    void GameOver()
    {
        // Oyuncuyu yok et
        Destroy(gameObject);

        FindObjectOfType<GameManager>().GameOver();
        // Oyunu durdur (isteğe bağlı)
        Debug.Log("Game Over!"); // Oyun bitti mesajı
        // Burada oyun bitiş ekranı veya yeniden başlatma mekanizması ekleyebilirsiniz.
        
    }
}
