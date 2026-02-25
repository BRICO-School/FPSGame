using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Gun gun;

    void Update()
    {
        if (gun == null) return;

        // --- 新しいInput Systemによる入力取得 ---
        
        // 1. 左クリックで射撃
        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            gun.Shoot();
        }

        // 2. Rキーでリロード
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
        {
            // Gun の Reload はコルーチンなので StartCoroutine で呼ぶ
            StartCoroutine(gun.Reload());
        }
        
        // ---------------------------------------
    }
}
