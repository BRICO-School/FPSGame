using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 3f;
    [SerializeField] private ParticleSystem muzzleFlash; // 発射時のエフェクト用

    private int currentAmmo;
    private bool isReloading = false;

    // 読み取り専用のプロパティ（UIなどで使用可能）
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;

    void Start()
    {
        // 弾数を満タンに設定
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        // リロード中、または弾切れの場合は撃てない
        if (isReloading || currentAmmo <= 0)
        {
            return;
        }

        // 弾を生成
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            currentAmmo--;

            // マズルフラッシュを再生
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
        }
    }

    public IEnumerator Reload()
    {
        // リロード中、または弾が満タンの場合は何もしない
        if (isReloading || currentAmmo == maxAmmo)
        {
            yield break;
        }

        isReloading = true;
        Debug.Log("Reloading...");

        // 指定された時間待つ
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload finished!");
    }
}
