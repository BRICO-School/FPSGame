using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用するために追加

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // マウスカーソルを画面中央に固定し、非表示にする
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 新しいInput Systemによる入力取得 ---
        var mouse = Mouse.current;
        if (mouse == null) return;

        // マウスの移動量を取得 (デルタ値)
        Vector2 mouseDelta = mouse.delta.ReadValue();

        // 感度と時間を掛けて調整
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
        // ---------------------------------------

        // 垂直方向の回転（カメラ自体の回転）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 真上・真下で止まるように制限

        // カメラ（このスクリプトが付いているメインカメラ）を回転
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 水平方向の回転（プレイヤー本体を回転）
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
