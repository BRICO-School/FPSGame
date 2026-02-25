# FPSゲーム開発：本日の実装ログまとめ (2026-02-23)

本日実装および調整した内容のまとめです。

## 1. プロジェクト基盤とフォルダ構成
- `Assets/Scripts/` 以下に `Player`, `Enemy`, `Weapon`, `Common`, `UI`, `GameManager` のフォルダを作成し、整理を行いました。

## 2. プレイヤー機能 (Phase 2)
- **移動 (`PlayerMovement.cs`)**:
    - **New Input System** に対応（ポーリング方式）。
    - 接地判定の安定化のため、水平・垂直移動の `controller.Move` 呼び出しを一回に集約。
    - ジャンプ機能（Space）の実装。
- **視点操作 (`MouseLook.cs`)**:
    - マウス入力による回転、垂直方向のクランプ（-90〜90度）、カーソルロックの実装。

## 3. 武器・射撃システム (Phase 3)
- **弾丸 (`Bullet.cs`)**:
    - Projectile（物理弾）方式。一定時間で消滅、衝突時にダメージ通知。
- **銃の基本 (`Gun.cs`)**:
    - 残弾数管理、リロード機能（コルーチンによるウェイト）、射撃ロジック。
    - **マズルフラッシュ演出**: パーティクル再生機能を追加。
- **射撃連携 (`PlayerShooting.cs`)**:
    - 左クリックで射撃、Rキーでリロードの入力紐付け。

## 4. ヘルス・ダメージシステム (Phase 4)
- **体力管理 (`Health.cs`)**:
    - 共通スクリプトとして作成。PlayerとEnemy両方に使用可能。
    - 死亡時の処理（敵はDestroy、プレイヤーはGameManager呼び出し）。

## 5. 敵キャラクター・AI (Phase 5)
- **敵AI (`EnemyAI.cs`)**:
    - **NavMeshAgent** を使用したプレイヤー追跡。
    - 射撃範囲内での自動攻撃、自動リロードロジックの実装。
    - `[RequireComponent(typeof(Gun))]` による設定ミス防止策の追加。

## 6. UI・ゲーム管理 (Phase 6)
- **GameManager (`GameManager.cs`)**:
    - シングルトン実装。ゲームオーバー時の時間停止、カーソル解放、リスタート機能。
- **HUD (`HUDManager.cs`)**:
    - 現在のHPと残弾数を表示するTMP (TextMeshPro) 連携。
- **クロスヘア**:
    - Canvas上にImageを用いた照準UIの構築（エディタ操作ガイド）。
- **ゲームオーバーUI**:
    - リトライボタンとGameManagerの連携（エディタ操作ガイド）。

## 🔧 デバッグ・調整済み項目
- **CharacterControllerの不一致**: ビジュアルと当たり判定のズレを調整。
- **ジャンプ不良**: `controller.isGrounded` の安定化対応。
- **Input Systemエラー**: `Input.GetAxis` からポーリング方式への移行。
- **マズルフラッシュの見た目**: 散弾状から「爆発風」へのパーティクル設定調整。

---
**次回の予定**: ステージの拡張、敵のバリエーション追加、SE・BGMの実装など。
