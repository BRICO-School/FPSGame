using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 40f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 10;

    private Rigidbody rb;

    void Start()
    {
        // Rigidbodyの参照を取得
        rb = GetComponent<Rigidbody>();

        // 前方に発射
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

        // 一定時間後に自動消滅
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 衝突した相手が Health コンポーネントを持っているか確認
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }

        // 当たったら弾を消す
        Destroy(gameObject);
    }
}
