# FPSゲーム開発タスクリスト（実装レベル詳細版）

このタスクリストは `docs/シンプルなFPSゲーム.md` の要件に基づいた、実装レベルの詳細なタスク分割です。

---

## Phase 1: プロジェクト初期設定

### 1-1. フォルダ構成の整備
- [ ] `Assets/Scripts/` フォルダを作成
- [ ] `Assets/Scripts/Player/` フォルダを作成
- [ ] `Assets/Scripts/Enemy/` フォルダを作成
- [ ] `Assets/Scripts/Weapon/` フォルダを作成
- [ ] `Assets/Scripts/Common/` フォルダを作成（体力・ダメージなど共通処理）
- [ ] `Assets/Scripts/UI/` フォルダを作成
- [ ] `Assets/Scripts/GameManager/` フォルダを作成
- [ ] `Assets/Prefabs/` フォルダを作成

### 1-2. シーンの基本構成
- [ ] `MainGame` シーンに床（Plane）を配置
- [ ] 障害物（Cube等）をいくつか配置
- [ ] ライティングの基本設定（Directional Lightの配置）

### 1-3. NavMesh の設定
- [ ] 🔧**User操作**: パッケージマネージャーから `AI Navigation` パッケージをインストール
  - `Window > Package Manager > Unity Registry > AI Navigation > Install`
- [ ] 🔧**User操作**: 床オブジェクトに `NavMeshSurface` コンポーネントを追加
- [ ] 🔧**User操作**: NavMeshSurface の `Bake` ボタンを押してNavMeshを焼く

---

## Phase 2: プレイヤー基本機能

### 2-1. プレイヤーオブジェクトの作成
- [ ] 🔧**User操作**: シーンに空のGameObjectを作成し「Player」と命名
- [ ] 🔧**User操作**: Player に `CharacterController` コンポーネントをアタッチ
- [ ] 🔧**User操作**: Player の子オブジェクトとして `Main Camera` を配置（FPS視点用）
- [ ] 🔧**User操作**: Player に適切なCollider設定（CharacterControllerのRadius, Height調整）

### 2-2. 移動スクリプトの実装（`PlayerMovement.cs`）
- [ ] `PlayerMovement.cs` を `Assets/Scripts/Player/` に作成
- [ ] フィールド定義:
  - `float moveSpeed = 5f`（移動速度、Inspectorで調整可能）
  - `float jumpForce = 5f`（ジャンプ力）
  - `float gravity = -9.81f`（重力）
  - `Vector3 velocity`（現在の速度ベクトル）
  - `CharacterController controller`（参照キャッシュ）
- [ ] `Start()`: `CharacterController` の参照を `GetComponent` で取得
- [ ] `Update()`:
  - `Input.GetAxis("Horizontal")` / `Input.GetAxis("Vertical")` でWASD入力取得
  - `transform.right * x + transform.forward * z` で移動方向を算出
  - `CharacterController.Move()` で移動
  - 接地判定（`controller.isGrounded`）
  - `Input.GetButtonDown("Jump")` でジャンプ処理（`velocity.y = jumpForce`）
  - 重力の適用（`velocity.y += gravity * Time.deltaTime`）
- [ ] 🔧**User操作**: Player オブジェクトに `PlayerMovement.cs` をアタッチ

### 2-3. 視点操作スクリプトの実装（`MouseLook.cs`）
- [ ] `MouseLook.cs` を `Assets/Scripts/Player/` に作成
- [ ] フィールド定義:
  - `float mouseSensitivity = 100f`（感度）
  - `Transform playerBody`（Player本体のTransform）
  - `float xRotation = 0f`（X軸回転の累積値）
- [ ] `Start()`:
  - `Cursor.lockState = CursorLockMode.Locked`（カーソルロック）
- [ ] `Update()`:
  - `Input.GetAxis("Mouse X")` / `Input.GetAxis("Mouse Y")` でマウス移動量取得
  - `xRotation` にY軸入力を加算し、`Mathf.Clamp` で `-90〜90度` に制限
  - カメラの `localRotation` にX軸回転を適用
  - `playerBody.Rotate(Vector3.up * mouseX)` で水平回転
- [ ] 🔧**User操作**: Main Camera に `MouseLook.cs` をアタッチ
- [ ] 🔧**User操作**: InspectorでPlayerBodyフィールドにPlayerオブジェクトをドラッグ＆ドロップ

---

## Phase 3: 武器・射撃システム（共通）

### 3-1. 弾の作成
- [ ] 🔧**User操作**: シーンに小さな Sphere を作成し「Bullet」と命名
- [ ] 🔧**User操作**: Bullet に `Rigidbody` コンポーネントをアタッチ（Use Gravity = false 推奨）
- [ ] `Bullet.cs` を `Assets/Scripts/Weapon/` に作成
  - `float speed = 40f`（弾速）
  - `float lifetime = 3f`（自動消滅までの時間）
  - `int damage = 10`（ダメージ量）
  - `Start()`: `Rigidbody.velocity = transform.forward * speed`
  - `Start()`: `Destroy(gameObject, lifetime)` で一定時間後に自動消滅
  - `OnTriggerEnter(Collider other)`:
    - 相手の `Health` コンポーネントを取得し `TakeDamage(damage)` を呼ぶ
    - 弾自身を `Destroy`
- [ ] 🔧**User操作**: Bullet に Bullet.cs をアタッチ
- [ ] 🔧**User操作**: Bullet の Collider を `Is Trigger = true` に設定
- [ ] 🔧**User操作**: Bullet を `Assets/Prefabs/` にドラッグしてプレハブ化、シーンから元を削除

### 3-2. 銃スクリプトの実装（`Gun.cs`）
- [ ] `Gun.cs` を `Assets/Scripts/Weapon/` に作成
- [ ] フィールド定義:
  - `GameObject bulletPrefab`（弾のPrefab参照）
  - `Transform firePoint`（弾の発射位置）
  - `int maxAmmo = 30`（最大弾数、可変）
  - `int currentAmmo`（現在の弾数）
  - `float reloadTime = 3f`（リロード時間）
  - `bool isReloading = false`（リロード中フラグ）
- [ ] `Start()`: `currentAmmo = maxAmmo`
- [ ] `Shoot()` メソッド:
  - `isReloading == true` または `currentAmmo <= 0` なら return
  - `Instantiate(bulletPrefab, firePoint.position, firePoint.rotation)` で弾を生成
  - `currentAmmo--`
- [ ] `Reload()` メソッド（コルーチン）:
  - `isReloading == true` または `currentAmmo == maxAmmo` なら return
  - `isReloading = true`
  - `yield return new WaitForSeconds(reloadTime)` で3秒待つ
  - `currentAmmo = maxAmmo`
  - `isReloading = false`

### 3-3. プレイヤー射撃連携（`PlayerShooting.cs`）
- [ ] `PlayerShooting.cs` を `Assets/Scripts/Player/` に作成
- [ ] フィールド定義:
  - `Gun gun`（Gun コンポーネントの参照）
- [ ] `Update()`:
  - `Input.GetButtonDown("Fire1")` → `gun.Shoot()` を呼ぶ
  - `Input.GetKeyDown(KeyCode.R)` → `gun.StartCoroutine(gun.Reload())` を呼ぶ
- [ ] 🔧**User操作**: Player（またはカメラに銃を持たせる場合はカメラの子オブジェクト）に空のGameObject「FirePoint」を作成し銃口の位置に配置
- [ ] 🔧**User操作**: Player に `Gun.cs` と `PlayerShooting.cs` をアタッチ
- [ ] 🔧**User操作**: Inspectorで `bulletPrefab` に Bullet プレハブ、`firePoint` に FirePoint をセット

---

## Phase 4: ヘルス・ダメージシステム（共通）

### 4-1. 体力管理スクリプトの実装（`Health.cs`）
- [ ] `Health.cs` を `Assets/Scripts/Common/` に作成
- [ ] フィールド定義:
  - `int maxHealth = 100`（最大体力）
  - `int currentHealth`（現在の体力）
- [ ] `Start()`: `currentHealth = maxHealth`
- [ ] `TakeDamage(int amount)` メソッド:
  - `currentHealth -= amount`
  - `currentHealth <= 0` の場合 → `Die()` を呼ぶ
- [ ] `Die()` メソッド:
  - `gameObject.tag` が `"Player"` の場合 → `GameManager.Instance.GameOver()` を呼ぶ
  - それ以外（敵の場合） → `Destroy(gameObject)`
- [ ] 🔧**User操作**: Player オブジェクトの Tag を `"Player"` に設定
- [ ] 🔧**User操作**: Player と Enemy の両方に `Health.cs` をアタッチ

---

## Phase 5: 敵キャラクター

### 5-1. 敵オブジェクトの作成
- [ ] 🔧**User操作**: シーンに Capsule 等を作成し「Enemy」と命名
- [ ] 🔧**User操作**: Enemy に `NavMeshAgent` コンポーネントをアタッチ
- [ ] 🔧**User操作**: Enemy に適切な Collider が付いていることを確認

### 5-2. 敵AIスクリプトの実装（`EnemyAI.cs`）
- [ ] `EnemyAI.cs` を `Assets/Scripts/Enemy/` に作成
- [ ] フィールド定義:
  - `NavMeshAgent agent`
  - `Transform player`（プレイヤーのTransform）
  - `float attackRange = 10f`（攻撃開始距離）
  - `float attackInterval = 1f`（攻撃間隔）
  - `float attackTimer = 0f`
- [ ] `Start()`:
  - `agent = GetComponent<NavMeshAgent>()`
  - `player = GameObject.FindGameObjectWithTag("Player").transform`
- [ ] `Update()`:
  - `agent.SetDestination(player.position)` で常にプレイヤーを追跡
  - プレイヤーとの距離を計算 (`Vector3.Distance`)
  - 攻撃範囲内の場合、`attackTimer` でインターバル管理しつつ射撃

### 5-3. 敵の射撃機能
- [ ] `EnemyAI.cs` に射撃処理を追加
  - `Gun gun` フィールドを追加して参照
  - 攻撃範囲内でインターバルが経過したら `gun.Shoot()` を呼ぶ
  - リロードロジック: 弾切れ時に自動で `gun.StartCoroutine(gun.Reload())` を呼ぶ
- [ ] 🔧**User操作**: Enemy の子オブジェクトに「FirePoint」を作成（プレイヤー方向を向くように配置）
- [ ] 🔧**User操作**: Enemy に `Gun.cs` と `EnemyAI.cs` をアタッチし、Inspector で各フィールド設定

### 5-4. 敵のプレハブ化
- [ ] 🔧**User操作**: 完成した Enemy を `Assets/Prefabs/` にドラッグしてプレハブ化
- [ ] 🔧**User操作**: シーンに複数体の敵を配置

---

## Phase 6: ゲーム管理・UI

### 6-1. GameManager の実装（`GameManager.cs`）
- [ ] `GameManager.cs` を `Assets/Scripts/GameManager/` に作成
- [ ] シングルトンパターンの実装:
  - `public static GameManager Instance`
  - `Awake()`: `Instance = this`
- [ ] `GameOver()` メソッド:
  - ゲームオーバーUIパネルを表示（`SetActive(true)`）
  - `Time.timeScale = 0f` でゲームを一時停止
  - `Cursor.lockState = CursorLockMode.None` でカーソル解放
- [ ] `RestartGame()` メソッド（オプション）:
  - `Time.timeScale = 1f` に戻す
  - `SceneManager.LoadScene(SceneManager.GetActiveScene().name)` でリスタート
- [ ] 🔧**User操作**: シーンに空の GameObject「GameManager」を作成し `GameManager.cs` をアタッチ

### 6-2. ゲームオーバーUI の作成
- [ ] 🔧**User操作**: Canvas を作成
- [ ] 🔧**User操作**: Canvas 内に Panel「GameOverPanel」を作成（初期状態で非アクティブ）
  - Text「Game Over」を配置（中央に大きく表示）
  - Button「リトライ」ボタンを配置（オプション）
- [ ] 🔧**User操作**: リトライボタンの `OnClick()` に `GameManager.RestartGame()` を登録
- [ ] 🔧**User操作**: `GameManager.cs` の Inspector で `GameOverPanel` をセット

### 6-3. HUD（ヘッドアップディスプレイ）の実装（オプション）
- [ ] `HUDManager.cs` を `Assets/Scripts/UI/` に作成
  - `Text ammoText`（弾数表示用）
  - `Text healthText`（体力表示用）
  - `Update()`:  プレイヤーの `Gun` の `currentAmmo` と `Health` の `currentHealth` を取得して表示
- [ ] 🔧**User操作**: Canvas 内に弾数・体力のText UIを配置
- [ ] 🔧**User操作**: HUDManager.cs をアタッチし、各Text参照をセット

---

## Phase 7: 統合テスト・調整

### 7-1. 動作確認
- [ ] 🔧**User操作**: Unityエディタで再生し、以下を確認:
  - プレイヤーがWASDで移動できること
  - マウスで視点移動できること
  - Spaceでジャンプできること
  - 左クリックで弾が発射されること
  - Rキーでリロードできること（3秒かかること）
  - リロード中に再リロードできないこと
  - 弾が30発で撃てなくなること
  - 敵がプレイヤーを追跡すること
  - 敵が攻撃範囲内で射撃すること
  - プレイヤー・敵が弾に当たるとダメージを受けること
  - 体力が0になるとオブジェクトが消滅すること
  - プレイヤーが死亡するとゲームオーバー画面が表示されること

### 7-2. パラメータ調整
- [ ] 🔧**User操作**: 各種パラメータをInspectorで調整:
  - 移動速度、ジャンプ力
  - マウス感度
  - 弾速、弾のダメージ
  - リロード時間
  - 敵の移動速度、攻撃範囲、攻撃間隔
  - プレイヤー・敵の体力

---

## 凡例

| マーク | 意味 |
|---|---|
| `- [ ]` | 未着手タスク |
| `🔧**User操作**` | Unityエディタ上でユーザーが手動で操作する必要があるタスク |
