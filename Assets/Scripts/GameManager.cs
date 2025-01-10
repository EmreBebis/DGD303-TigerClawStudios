using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // Düşman gemisi prefab'ı
    public Vector2 spawnAreaMin; // Düşmanların spawn olacağı alanın minimum koordinatı
    public Vector2 spawnAreaMax; // Düşmanların spawn olacağı alanın maksimum koordinatı
    public float spawnInterval = 3f; // Düşmanların spawn aralığı (saniye başına)
    public float enemyLifetime = 5f; // Düşmanların sahnede kalacağı süre (saniye)
    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı

    [Header("UI Menus")]
    public GameObject startMenu; // Başlangıç menüsü
    public GameObject pauseMenu; // Duraklatma menüsü

    private float timer = 0f;
    private bool isGamePaused = false;
    private bool isGameRunning = false;

    void Start()
    {
        // Oyun başlarken başlangıç menüsünü göster, oyunu duraklat
        Time.timeScale = 0f;
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        isGamePaused = false;
        isGameRunning = false;
    }

    void Update()
    {
        // Oyun duraklatılmışsa veya başlamamışsa spawn işlemini durdur
        if (isGamePaused || !isGameRunning || Time.timeScale == 0f)
            return;

        // Düşmanları belirli aralıklarla spawn et
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f; // Zamanlayıcıyı sıfırla
        }

        // Pause menüsünü aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void SpawnEnemy()
    {
        // Rastgele bir spawn pozisyonu belirle
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0); // 2D sahne için z ekseni 0

        // Düşmanı spawn et
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Düşman yok edildiğinde patlama efekti tetiklemek için EnemyController ekle
        EnemyController enemyController = enemy.AddComponent<EnemyController>();
        enemyController.explosionEffectPrefab = explosionEffectPrefab;

        // Düşmanı belirli bir süre sonra yok et
        Destroy(enemy, enemyLifetime);
    }

    public void StartGame()
    {
        // Oyunu başlat
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Oyunu devam ettir
        isGamePaused = false;
        isGameRunning = true;

        // Zamanlayıcıyı sıfırla
        timer = 0f;
    }

    public void TogglePauseMenu()
    {
        // Pause menüsünü aç/kapat
        isGamePaused = !isGamePaused;
        pauseMenu.SetActive(isGamePaused);
        Time.timeScale = isGamePaused ? 0f : 1f; // Oyun duraklat veya devam ettir
    }

    public void ReturnToMainMenu()
    {
        // Ana menüye dön
        pauseMenu.SetActive(false);
        startMenu.SetActive(true);
        Time.timeScale = 0f; // Oyunu duraklat

        // Oyunu durdur
        isGamePaused = false;
        isGameRunning = false;
    }

    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor.");
        Application.Quit();
    }
}
