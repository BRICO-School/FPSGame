using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // 現在の体力を取得するプロパティ（UIなどで使用）
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Died!");
            // GameManager のシングルトンを介してゲームオーバーを呼ぶ
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} destroyed!");
            Destroy(gameObject);
        }
    }
}
