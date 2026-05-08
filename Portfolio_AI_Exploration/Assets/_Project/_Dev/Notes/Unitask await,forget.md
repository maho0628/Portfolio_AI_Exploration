HitStop / UniTask 学習メモ
■ HitStop実装について

被弾時の演出としてHitStopを追加。

ただし、

Time.timeScale = 0f;

のような完全停止は、

DOTween停止
Animator停止
非同期処理停止
復帰漏れ

など事故リスクが高いため使用しない。

代わりに：

Time.timeScale = 0.05f;

のような「超低速化」で実装。

これにより、

見た目は一瞬止まったように見える
内部処理は安全に継続

できる。

■ UniTask の await 理解
await UniTask.Delay(...)

は、

「処理を停止している」のではなく、

“後で続きを再開予約している”

イメージ。

例えば：

await UniTask.Delay(TimeSpan.FromSeconds(0.03f));

の場合：

Delay開始
0.03秒後に再開予約
関数は一旦中断
指定時間後、自動で await の後ろから再開

される。

■ Forget() の理解
PlayHitStop().Forget();

は、

「非同期処理を待たずに開始する」

という意味。

重要なのは、

“処理を消しているわけではない”

こと。

内部では：

await後の再開予約
Delay
TimeScale復帰

などは正常に動作している。

Forget は、

「呼び出し元が待機しない」

だけ。

■ ignoreTimeScale の理解

通常の Delay は、

Time.timeScale

の影響を受ける。

そのため HitStop 中に普通の Delay を使うと、

Delay自体も超低速化
HitStopが異常に長くなる

問題が起きる。

そこで：

ignoreTimeScale: true

を指定。

これにより：

「ゲーム速度を無視して現実時間で待機」

できる。

つまり：

await UniTask.Delay(
    TimeSpan.FromSeconds(0.03f),
    ignoreTimeScale: true
);

は、

ゲームが超スロー状態でも、

現実時間で0.03秒後に復帰

する。

■ 今回の気付き

ゲーム演出では、

「内部完全同期」より、

“プレイヤーに自然に感じるか”

が重要。

また、

時間制御
演出
UI更新
非同期

は密接に関係しているため、

「プレイヤー体感」で確認する視点が重要。
次回：実際にAgentを動かして挙動確認
