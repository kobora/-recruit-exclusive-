using UnityEngine;

// 個々のボール（ピース）を表します。
public class Ball_action : MonoBehaviour
{
    // このボールの種類を指定します。
    public int ballType = 0;
    // ハイライトされた時のサウンドを指定します。
    public AudioClip picSound;
    // キャンセルされた時のサウンドを指定します。
    public AudioClip CancelSound;

    // 通常状態の画像を保存しておく変数
    Sprite normalBall;
    // ハイライトされた時の画像を指定します。
    public Sprite highBall;

    // Start is called before the first frame update
    void Start()
    {
        normalBall = GetComponent<SpriteRenderer>().sprite;
    }

    // このピースを通常状態に設定します。
    void normalCol()
    {
        GetComponent<SpriteRenderer>().sprite = normalBall;
        AudioSource.PlayClipAtPoint(CancelSound, transform.position);
    }

    // このピースをハイライト状態に設定します。
    void highlight()
    {
        GetComponent<SpriteRenderer>().sprite = highBall;
        AudioSource.PlayClipAtPoint(picSound, transform.position);
    }
}
