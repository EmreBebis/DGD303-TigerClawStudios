using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Düþman gemisi prefab'ý
    public Vector2 spawnAreaMin; // Düþmanlarýn spawn olacaðý alanýn minimum koordinatý
    public Vector2 spawnAreaMax; // Düþmanlarýn spawn olacaðý alanýn maksimum koordinatý
    public float spawnInterval = 3f; // Düþmanlarýn spawn aralýðý (saniye baþýna)
    public float enemyLifetime = 5f; // Düþmanlarýn sahnede kalacaðý süre (saniye)

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f; // Zamanlayýcýyý sýfýrla
        }
    }

    void SpawnEnemy()
    {
        // Rastgele bir spawn pozisyonu belirle
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0); // 2D sahne için z ekseni 0

        // Düþmaný spawn et
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Düþmaný belirli bir süre sonra yok et
        Destroy(enemy, enemyLifetime);
    }
}