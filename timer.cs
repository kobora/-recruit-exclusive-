using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ゲーム全体の進行を管理します。
public class timer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("メッセージUIを指定します。")]
    private TextMeshProUGUI title = null;
    [SerializeField]
    [Tooltip("スコアUIを指定します。")]
    private TextMeshProUGUI score = null;

    // プレイ時間(秒)
    float nowTime = 0;
    // プレイ中の場合はtrue、それ以外はfalse
    bool isPlaying = true;
    // プレイ前の場合はtrue、プレイ開始でfalse
    static public bool isWait = true;
    // 現在のスコア
    static public int nowScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true;
        isWait = true;
        GetComponent<TextMeshProUGUI>().text = "00s";
        title.text = "wait...";
        score.text = "000000";
        nowTime = 60;
        StartCoroutine(ballCheck());
    }

    // Update is called once per frame
    void Update()
    {
        // スコア表示を更新
        score.text = nowScore.ToString("d06");

        if (isPlaying)
        {
            if (nowTime <= 0)
            {
                isPlaying = false;
                isWait = true;
                title.text = "Time Up";
                title.enabled = true;
                nowTime = 0;
                StartCoroutine(reStart());
            }
            else if (!isWait)
            {
                nowTime -= Time.deltaTime;
            }
            GetComponent<TextMeshProUGUI>().text = $"{nowTime:f2}s";
        }
    }

    // 全てのピースが静止すまで処理を実行します。
    IEnumerator ballCheck()
    {
        // 開始後1秒は無条件に待機
        yield return new WaitForSeconds(1);

        // 全てのピースオブジェクトが留まったらisWaitをfalseに設定して
        // while処理を抜ける
        while (true)
        {
            bool moves = false;
            var balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (var ball in balls)
            {
                // ピースがまだ動いているかどうかを判定
                if (ball.GetComponent<Rigidbody2D>().velocity.magnitude > 0.3f)
                {
                    moves = true;
                    break;
                }
            }
            // 静止状態を判定
            if (!moves)
            {
                isWait = false;
                break;
            }

            yield return null;
        }

        // プレイ開始を演出する
        title.text = "GO!!";
        yield return new WaitForSeconds(1);
        title.enabled = false;
    }

    IEnumerator reStart()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                SceneManager.LoadScene(0);
                break;
            }
            yield return null;
        }
    }

}
