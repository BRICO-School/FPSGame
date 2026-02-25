using UnityEngine;
using TMPro; // TextMeshProを使用するために必要

public class HUDManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoText;

    private Health playerHealth;
    private Gun playerGun;

    void Start()
    {
        // プレイヤーオブジェクトを探してコンポーネントを取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerGun = player.GetComponent<Gun>();
        }
    }

    void Update()
    {
        // 体力表示の更新
        if (playerHealth != null && healthText != null)
        {
            healthText.text = $"HP: {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}";
        }

        // 弾数表示の更新
        if (playerGun != null && ammoText != null)
        {
            if (playerGun.IsReloading)
            {
                ammoText.text = "Ammo: Reloading...";
            }
            else
            {
                ammoText.text = $"Ammo: {playerGun.CurrentAmmo} / {playerGun.MaxAmmo}";
            }
        }
    }
}
