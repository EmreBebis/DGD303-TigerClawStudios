using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // D��man gemisi prefab'�
    public Vector2 spawnAreaMin; // D��manlar�n spawn olaca�� alan�n minimum koordinat�
    public Vector2 spawnAreaMax; // D��manlar�n spawn olaca�� alan�n maksimum koordinat�
    public float spawnInterval = 3f; // D��manlar�n spawn aral��� (saniye ba��na)
    public float enemyLifetime = 5f; // D��manlar�n sahnede kalaca�� s�re (saniye)

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f; // Zamanlay�c�y� s�f�rla
        }
    }

    void SpawnEnemy()
    {
        // Rastgele bir spawn pozisyonu belirle
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0); // 2D sahne i�in z ekseni 0

        // D��man� spawn et
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // D��man� belirli bir s�re sonra yok et
        Destroy(enemy, enemyLifetime);
    }
}