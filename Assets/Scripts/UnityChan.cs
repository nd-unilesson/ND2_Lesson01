using UnityEngine;
using UnityEngine.InputSystem;

public class UnityChan : MonoBehaviour
{
    public Vector2 Velocity;        // 移動速度
    public Animator Animator;       // Animatorコンポーネント
    public SpriteRenderer Renderer; // SpriteRendererコンポーネント

    public bool isWalk = false;     // 歩くフラグ
    public Vector2 mousePoint;      // マウスの座標値

    public Vector3 targetPoint;     // ターゲットの座標値
    public GameObject weapon;       // 武器オブジェクト

    public ParticleSystem footStampEffect;  // 足跡のパーティクルを読み込む

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーションを動かす
        Animator.SetFloat("Velocity", Mathf.Abs(Velocity.x) * (isWalk ? 0.5f : 1));

        // スプライトを反転させる
        if (Velocity.x <= -0.1f) Renderer.flipX = true;
        else if (0.1f <= Velocity.x) Renderer.flipX = false;

        // 座標を移動させる
        float speed = Velocity.x * 5 * (isWalk ? 0.5f : 1) * Time.deltaTime;
        transform.Translate(speed , 0, 0);

        // ターゲット（マウス）の位置を保存し続ける
        targetPoint = GetMousePoint();
        // ターゲットとの角度（ラジアン）を求める
        float diffX = targetPoint.x - transform.position.x;
        float diffY = targetPoint.y - transform.position.y;
        float radian = Mathf.Atan2(diffY, diffX);
        // 角度から向きベクトルを求める
        float dx = Mathf.Cos(radian);
        float dy = Mathf.Sin(radian);

        // 武器の角度を変える
        weapon.transform.eulerAngles = new Vector3(0, 0, radian * 180 / Mathf.PI);

        // 足跡エフェクトを制御
        FootStampControl();
    }

    // 移動入力イベント
    void OnMove(InputValue value)
    {
        Velocity = value.Get<Vector2>();
    }

    // クリック入力イベント
    void OnClick(InputValue value)
    {
        isWalk = value.Get<float>() >= 0.5f;
    }

    // マウスの座標入力イベント
    void OnMousePoint(InputValue value)
    {
        mousePoint = value.Get<Vector2>();
    }

    // === マウス座標を取得するメソッド
    // 引　数：なし
    // 戻り値：<Vector2型>の値
    Vector2 GetMousePoint()
    {
        // マウスの座標を取得
        Vector2 mousePoint = this.mousePoint;

        // メインカメラの情報を取得
        Camera camera = Camera.main;

        // [px]単位を[m]単位に変換
        Vector3 worldPoint = camera.ScreenToWorldPoint( mousePoint );

        // 変換した値を返す
        return worldPoint;
    }

    // === 足跡パーティクルの制御
    // 引数　：なし
    // 戻り値：なし(void)
    public void FootStampControl()
    {
        // 👉 三項演算子 [ 条件式 ? true : false; ]
        // 1行で書ける if文　... でも1行でしか書けない...
        // 制御が1行でできる単純な場合に使用する
        bool isStamp = Velocity.magnitude > 0.1f ? true : false;

        if(isStamp && !footStampEffect.isPlaying)
        {   // 動いていれば、エフェクトを出す
            footStampEffect.Play();
        }
        
        if(!isStamp)
        {   // 動いてない時は、エフェクトを止める
            footStampEffect.Stop();
        }
    }
}
