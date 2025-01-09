using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Düşman gemisi prefab'ı
    public Vector2 spawnAreaMin; // Düşmanların spawn olacağı alanın minimum koordinatı
    public Vector2 spawnAreaMax; // Düşmanların spawn olacağı alanın maksimum koordinatı
    public float spawnInterval = 3f; // Düşmanların spawn aralığı (saniye başına)
    public float enemyLifetime = 5f; // Düşmanların sahnede kalacağı süre (saniye)

    public GameObject explosionEffectPrefab; // Patlama efekti prefab'ı
    public GameObject muzzleflashEffectPrefab;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f; // Zamanlayıcıyı sıfırla
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
}
