using UnityEngine;

public class MissileControler : MonoBehaviour
{
    public float speed = 10f; // Merminin hızı

    void Update()
    {
        // Mermiyi ileri doğru hareket ettir
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}