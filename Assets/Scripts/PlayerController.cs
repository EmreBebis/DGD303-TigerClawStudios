using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    AudioManager audiomanager;

    private void Awake()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public float moveSpeed = 5f; // Hareket hızı
    public GameObject bulletPrefab; // Mermi prefab'ı
    public Transform[] firePoints; // Birden fazla ateş noktası
    public float bulletSpeed = 10f; // Merminin hızı
    public float bulletLifetime = 2f; // Merminin ömrü (saniye)

    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public GameObject muzzleFlashEffectPrefab; // Ateşleme efekti prefab'ı

    private Vector2 movement; // Hareket girdisini tutacak
    private Vector2 screenBounds; // Ekran sınırları
    private float objectWidth; // Geminin genişliği
    private float objectHeight; // Geminin yüksekliği

    void Start()
    {
        // Ekran sınırlarını hesapla
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Geminin boyutlarını hesapla
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            objectWidth = sr.bounds.size.x / 2; // Genişliğin yarısı
            objectHeight = sr.bounds.size.y / 2; // Yüksekliğin yarısı
        }
    }

    void Update()
    {
        // Klavye girişlerini al
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Hareket girdisini hesapla
        movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;

        // Ateş etme kontrolü
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void FixedUpdate()
    {
        // Hareketi uygula
        Vector3 newPosition = transform.position + (Vector3)movement * Time.fixedDeltaTime;

        // Sınırlar içinde pozisyonu kısıtla
        newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);

        // Gemiyi yeni pozisyona taşı
        transform.position = newPosition;
    }

    void Fire()
    {
        audiomanager.PlaySFX(audiomanager.playershoot);
        if (bulletPrefab != null && firePoints.Length > 0)
        {
            foreach (Transform firePoint in firePoints)
            {
                if (firePoint != null)
                {
                    // Mermiyi oluştur
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                    // Mermiyi hareket ettir
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = firePoint.up * bulletSpeed;
                    }

                    // Mermiyi belirli bir süre sonra yok et
                    Destroy(bullet, bulletLifetime);

                    // Ateşleme efekti oluştur
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
            HandleCollision(collision);
        }
        // Eğer çarpışan obje "Asteroid" tag'ine sahipse
        else if (collision.CompareTag("Asteroid"))
        {
            HandleCollision(collision);
        }
        // Eğer çarpışan obje "Pizza" tag'ine sahipse
        else if (collision.CompareTag("Pizza"))
        {
            CollectPizza(collision);
        }
    }

    private void HandleCollision(Collider2D collision)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.TakeDamage(1);
        }

        Destroy(collision.gameObject);

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
        }
    }

    private void CollectPizza(Collider2D collision)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.CollectPizza(); // Pizzayı topla
        }

        Destroy(collision.gameObject); // Pizzayı yok et
        audiomanager.PlaySFX(audiomanager.takePizzas); // Pizza toplama sesini çal
    }

    void CreateMuzzleFlash(Vector2 position, Vector2 direction)
    {
        if (muzzleFlashEffectPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashEffectPrefab, position, Quaternion.identity);
            muzzleFlash.transform.up = direction;
            Destroy(muzzleFlash, 0.1f);
        }
    }
}
