using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    AudioManager audiomanager;

    private void Awake()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    [Header("Player Health Settings")]
    public int maxHealth = 3; // Oyuncunun maksimum canı
    private int currentHealth;

    public GameObject[] hearts; // Kalp UI elemanları (GameObject)

    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // Düşman gemisi prefab'ı
    public Vector2 spawnAreaMin; // Düşmanların spawn olacağı alanın minimum koordinatı
    public Vector2 spawnAreaMax; // Düşmanların spawn olacağı alanın maksimum koordinatı
    public float spawnInterval = 3f; // Düşmanların spawn aralığı (saniye başına)
    public float enemyLifetime = 5f; // Düşmanların sahnede kalacağı süre (saniye)

    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab; // Asteroid prefab'ı
    public Vector2 asteroidSpawnAreaMin; // Asteroidlerin spawn olacağı alanın minimum koordinatı
    public Vector2 asteroidSpawnAreaMax; // Asteroidlerin spawn olacağı alanın maksimum koordinatı
    public float asteroidSpawnInterval = 5f; // Asteroid spawn aralığı (saniye)
    public float asteroidLifetime = 10f; // Asteroidlerin sahnede kalacağı süre (saniye)

    [Header("UI Menus")]
    public GameObject startMenu; // Başlangıç menüsü
    public GameObject pauseMenu; // Duraklatma menüsü
    public GameObject creditsMenu; // Credits menüsü
    public GameObject gameOverMenu; // Game Over menüsü

    private float enemyTimer = 0f;
    private float asteroidTimer = 0f;
    private bool isGamePaused = false;
    private bool isGameRunning = false;

    void Start()
    {
        currentHealth = maxHealth; // Oyunu başlatmadan önce canı maksimuma ayarla

        // Menü müziğini başlat
        audiomanager.PlayMusic(audiomanager.background);

        // Kalpleri başlangıçta gizle
        HideHearts();

        Time.timeScale = 0f; // Oyunu durdur
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        creditsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    void Update()
    {
        if (isGamePaused || !isGameRunning || Time.timeScale == 0f) return;

        // Düşman gemileri spawn et
        enemyTimer += Time.deltaTime;
        if (enemyTimer >= spawnInterval)
        {
            SpawnEnemy();
            enemyTimer = 0f;
        }

        // Asteroidler spawn et
        asteroidTimer += Time.deltaTime;
        if (asteroidTimer >= asteroidSpawnInterval)
        {
            SpawnAsteroid();
            asteroidTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseMenu();
    }

    void SpawnEnemy()
    {
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Destroy(enemy, enemyLifetime);
    }

    void SpawnAsteroid()
    {
        float spawnX = Random.Range(asteroidSpawnAreaMin.x, asteroidSpawnAreaMax.x);
        float spawnY = Random.Range(asteroidSpawnAreaMin.y, asteroidSpawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        Destroy(asteroid, asteroidLifetime);
    }

    public void TakeDamage(int damage)
    {
        audiomanager.PlaySFX(audiomanager.takeDamage);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHearts();

        if (currentHealth <= 0) GameOver();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }

    private void HideHearts()
    {
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }
    }

    public void StartGame()
    {
        // Menü müziğini durdur ve oyun müziğini başlat
        audiomanager.PlayMusic(audiomanager.playing);

        startMenu.SetActive(false);
        Time.timeScale = 1f;
        isGameRunning = true;
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;
        pauseMenu.SetActive(isGamePaused);
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    public void GameOver()
    {
        audiomanager.PlaySFX(audiomanager.oyuncuExplode);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) Destroy(player); // Oyuncu gemisini sahneden kaldır

        isGameRunning = false;
        Time.timeScale = 0f;

        // Kalpleri gizle
        HideHearts();

        gameOverMenu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        // Menü müziğini başlat
        audiomanager.PlayMusic(audiomanager.background);

        // Kalpleri gizle
        HideHearts();

        // Sahneyi yeniden yükle (oyunu sıfırla)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor.");
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsMenu.SetActive(true);
    }

    public void HideCredits()
    {
        creditsMenu.SetActive(false);
    }
}
