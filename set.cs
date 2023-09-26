using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボールを発生する機能を提供します。
public class set : MonoBehaviour
{
    // 6色のボールプレハブを指定します。
    public GameObject[] balls = new GameObject[6];
    // ボールが弾けるさいのサウンドを指定します
    public AudioClip PopSound;
    // 選択したボールを保存しておく変数
    List<GameObject> picBalls = new();
    // 現在選択中のボールの種類
    int ballType;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setBall(45));
        picBalls = new();
    }

    private IEnumerator setBall(int ball_n)
    {
        // 指定した数のピースを生成
        for (int i = 0; i < ball_n; i++)
        {
            // ピースを生成
            var x = Random.Range(0, balls.Length);
            var ball = Instantiate(balls[x]);
            ball.transform.position = new Vector3(Random.Range(-1.7f, 1.7f), 6.5f, 0);

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!timer.isWait)
        {
            // タッチ開始
            if (Input.GetMouseButtonDown(0))
            {
                ballType = -1;
                OnTap();
            }
            // ドラッグ中
            else if (Input.GetMouseButton(0))
            {
                isDrag();
            }
            // タッチ終了
            else if (Input.GetMouseButtonUp(0))
            {
                if (picBalls.Count > 0)
                {
                    StartCoroutine(TapOff());
                }
            }
        }
    }

    // タッチ開始の際に呼び出されます。
    void OnTap()
    {
        // レイの発射位置
        var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // レイによる交差判定
        var hit = Physics2D.Raycast(origin, Vector2.zero);
        if (hit)
        {
            var ballAction = hit.collider.GetComponent<Ball_action>();
            // 最初に選択したボールのタグを記録しておく
            ballType = ballAction.ballType;
            // 選択中のボール配列を初期化して今回のボールを追加
            picBalls = new();
            picBalls.Add(ballAction.gameObject);
            Debug.Log(picBalls[0]);

            ballAction.SendMessage("highlight");
        }
    }

    // ドラッグ中に呼び出されます。
    void isDrag()
    {
        // レイの発射位置
        var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // レイによる交差判定
        var hit = Physics2D.Raycast(origin, Vector2.zero);
        if (hit)
        {
            var ballAction = hit.collider.GetComponent<Ball_action>();
            // 今回のボールが選択配列に未登録の場合は追加する
            if (!picBalls.Contains(ballAction.gameObject))
            {
                // 現在選択中のボール種類と一致する場合のみ配列に追加
                if (ballAction.ballType == ballType && ballType != -1)
                {
                    picBalls.Add(ballAction.gameObject);
                    ballAction.SendMessage("highlight");
                }
                // 選択中のボール種類と違う種類が選択された場合はキャンセルする
                else if (ballAction.ballType != ballType)
                {
                    clearBall();
                }
            }
        }
    }

    // 選択中のすべてのボールを通常状態に戻して、配列をクリアーします。
    void clearBall()
    {
        foreach (var ball in picBalls)
        {
            ball.SendMessage("normalCol");
        }
        picBalls.Clear();
    }

    // タッチ終了の際に呼び出されます。
    IEnumerator TapOff()
    {
        // 3個以上のボールがマッチしていた場合
        if (picBalls.Count >= 3)
        {
            int add_ball = 0;
            foreach (var ball in picBalls)
            {
                yield return new WaitForSeconds(0.1f);
                Destroy(ball);
                add_ball++;
                timer.nowScore += add_ball * 200;
                AudioSource.PlayClipAtPoint(PopSound, transform.position);
            }
            // 消去した個数を補充する
            StartCoroutine(setBall(picBalls.Count));
            picBalls.Clear();
        }
        else
        {
            clearBall();
        }
    }
}
