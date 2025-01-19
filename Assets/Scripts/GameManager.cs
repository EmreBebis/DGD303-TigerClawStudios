using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    AudioManager audiomanager;

    private void Awake()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    [Header("Player Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject[] hearts;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // İlk düşman gemisi prefab'ı
    public GameObject enemy2Prefab; // Yeni düşman gemisi prefab'ı
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public float spawnInterval = 3f;
    public float enemyLifetime = 5f;

    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;
    public Vector2 asteroidSpawnAreaMin;
    public Vector2 asteroidSpawnAreaMax;
    public float asteroidSpawnInterval = 5f;
    public float asteroidLifetime = 10f;

    [Header("Pizza Settings")]
    public GameObject pizzaPrefab;
    public Vector2 pizzaSpawnAreaMin;
    public Vector2 pizzaSpawnAreaMax;
    public float pizzaSpawnInterval = 3f;
    public int maxPizzaCount = 20;
    public int pizzasToWin = 15;
    public float gameDuration = 60f;

    [Header("UI Elements")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject creditsMenu;
    public GameObject gameOverMenu;
    public GameObject winMenu;
    public TMP_Text timerText;
    public TMP_Text pizzaCountText;

    private float enemyTimer = 0f;
    private float asteroidTimer = 0f;
    private float pizzaTimer = 0f;
    private float gameTimer;
    private int currentPizzaCount = 0;
    private int collectedPizzas = 0;
    private bool isGamePaused = false;
    private bool isGameRunning = false;

    void Start()
    {
        currentHealth = maxHealth;
        gameTimer = gameDuration;

        pizzaCountText.text = "Pizzas: 0 / " + pizzasToWin;
        timerText.text = "Time: " + gameDuration.ToString("F0");
        HideGameUI();

        winMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        audiomanager.PlayMusic(audiomanager.background);

        HideHearts();

        Time.timeScale = 0f;
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    void Update()
    {
        if (isGamePaused || !isGameRunning || Time.timeScale == 0f) return;

        gameTimer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Max(0, gameTimer).ToString("F0");

        if (gameTimer <= 0)
        {
            EndGame(false);
            return;
        }

        enemyTimer += Time.deltaTime;
        if (enemyTimer >= spawnInterval)
        {
            SpawnEnemy();
            enemyTimer = 0f;
        }

        asteroidTimer += Time.deltaTime;
        if (asteroidTimer >= asteroidSpawnInterval)
        {
            SpawnAsteroid();
            asteroidTimer = 0f;
        }

        pizzaTimer += Time.deltaTime;
        if (pizzaTimer >= pizzaSpawnInterval && currentPizzaCount < maxPizzaCount)
        {
            SpawnPizza();
            pizzaTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void SpawnEnemy()
    {
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        // Rastgele bir düşman türü seç
        GameObject enemyToSpawn = Random.value > 0.5f ? enemyPrefab : enemy2Prefab;

        GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
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

    void SpawnPizza()
    {
        float spawnX = Random.Range(pizzaSpawnAreaMin.x, pizzaSpawnAreaMax.x);
        float spawnY = Random.Range(pizzaSpawnAreaMin.y, pizzaSpawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        Instantiate(pizzaPrefab, spawnPosition, Quaternion.identity);
        currentPizzaCount++;
    }

    public void CollectPizza()
    {
        collectedPizzas++;
        currentPizzaCount--;
        pizzaCountText.text = "Pizzas: " + collectedPizzas + " / " + pizzasToWin;

        if (collectedPizzas >= pizzasToWin)
        {
            EndGame(true);
        }
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

    private void HideGameUI()
    {
        timerText.gameObject.SetActive(false);
        pizzaCountText.gameObject.SetActive(false);
    }

    private void ShowGameUI()
    {
        timerText.gameObject.SetActive(true);
        pizzaCountText.gameObject.SetActive(true);
    }

    void EndGame(bool hasWon)
    {
        isGameRunning = false;

        // Oyuncuyu ve sahnedeki tüm objeleri yok et
        ClearSceneObjects();

        if (hasWon)
        {
            winMenu.SetActive(true);
        }
        else
        {
            gameOverMenu.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void ClearSceneObjects()
    {
        // Oyuncuyu yok et
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) Destroy(player);

        // Sahnedeki düşmanları, asteroitleri ve pizzaları yok et
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
        {
            Destroy(asteroid);
        }
        foreach (GameObject pizza in GameObject.FindGameObjectsWithTag("Pizza"))
        {
            Destroy(pizza);
        }
    }

    public void StartGame()
    {
        audiomanager.PlayMusic(audiomanager.playing);

        startMenu.SetActive(false);
        ShowGameUI();
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
        EndGame(false);
    }

    public void ReturnToMainMenu()
    {
        audiomanager.PlayMusic(audiomanager.background);

        HideHearts();
        HideGameUI();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
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
