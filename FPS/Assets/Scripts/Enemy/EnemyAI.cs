using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Gun))] // Gunコンポーネントを必須にする
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent agent;
    private Transform player;

    [Header("Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackInterval = 1f;
    
    private float attackTimer = 0f;
    private Gun gun; // 次のステップで使用

    void Start()
    {
        // NavMeshAgentの取得
        agent = GetComponent<NavMeshAgent>();

        // プレイヤーのタグを使ってプレイヤーを探す
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // 銃のコンポーネントを取得（Phase 5-3用）
        gun = GetComponent<Gun>();
    }

    void Update()
    {
        if (player == null) return;

        // 常にプレイヤーを追いかける
        agent.SetDestination(player.position);

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 攻撃範囲内の場合
        if (distanceToPlayer <= attackRange)
        {
            // インターバルのタイマーを進める
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                Attack();
                attackTimer = 0f;
            }
        }
    }

    private void Attack()
    {
        if (gun == null)
        {
            Debug.LogWarning("Enemy attacking but Gun component is missing!");
            return;
        }

        // 1. 弾切れなら自動的にリロード
        if (gun.CurrentAmmo <= 0 && !gun.IsReloading)
        {
            StartCoroutine(gun.Reload());
            return;
        }

        // 2. リロード中でなければ射撃
        if (!gun.IsReloading)
        {
            // プレイヤーの方向を向く
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            
            gun.Shoot();
        }
    }
}
