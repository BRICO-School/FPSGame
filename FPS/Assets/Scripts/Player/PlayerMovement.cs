using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用するために追加

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        // CharacterControllerの参照を取得
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. 接地判定と速度のリセット
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 接地を維持するために少し下向きの力をかける
        }

        // 2. 入力取得（ポーリング方式）
        float x = 0;
        float z = 0;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed) x -= 1f;
            if (keyboard.dKey.isPressed) x += 1f;
            if (keyboard.wKey.isPressed) z += 1f;
            if (keyboard.sKey.isPressed) z -= 1f;
        }

        // 3. 移動方向の計数（水平方向）
        Vector3 move = transform.right * x + transform.forward * z;

        // 4. ジャンプ処理
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            // ジャンプ初速度 V = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // 5. 重力の加算
        velocity.y += gravity * Time.deltaTime;

        // 6. 全体の移動実行（水平方向 + 垂直方向）
        // 二回に分けず一回にまとめることで、地面との接触判定も安定しやすくなります
        Vector3 finalMove = (move * moveSpeed) + velocity;
        controller.Move(finalMove * Time.deltaTime);
    }
}
