using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı
    public GameObject bulletPrefab; // Mermi prefab'ı
    public Transform[] firePoints; // Birden fazla FirePoint
    public float bulletSpeed = 10f; // Merminin hızı
    public float bulletLifetime = 2f; // Merminin ömrü (saniye)

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
                }
            }
        }
    }
}