using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel; // ゲームオーバー時に表示するパネル

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            // シーンを跨いでも破棄されたくない場合は加えます（今回は単一シーン想定なので不要でも可）
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ゲームオーバー処理を行う
    /// </summary>
    public void GameOver()
    {
        Debug.Log("Game Over triggered!");

        // 1. ゲームオーバーUIを表示
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 2. ゲームを一時停止
        Time.timeScale = 0f;

        // 3. マウスカーソルを解放
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// ゲームを最初からやり直す
    /// </summary>
    public void RestartGame()
    {
        // 時間の流れを元に戻す
        Time.timeScale = 1f;
        
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
