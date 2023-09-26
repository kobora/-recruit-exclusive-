using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �{�[���𔭐�����@�\��񋟂��܂��B
public class set : MonoBehaviour
{
    // 6�F�̃{�[���v���n�u���w�肵�܂��B
    public GameObject[] balls = new GameObject[6];
    // �{�[�����e���邳���̃T�E���h���w�肵�܂�
    public AudioClip PopSound;
    // �I�������{�[����ۑ����Ă����ϐ�
    List<GameObject> picBalls = new();
    // ���ݑI�𒆂̃{�[���̎��
    int ballType;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setBall(45));
        picBalls = new();
    }

    private IEnumerator setBall(int ball_n)
    {
        // �w�肵�����̃s�[�X�𐶐�
        for (int i = 0; i < ball_n; i++)
        {
            // �s�[�X�𐶐�
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
            // �^�b�`�J�n
            if (Input.GetMouseButtonDown(0))
            {
                ballType = -1;
                OnTap();
            }
            // �h���b�O��
            else if (Input.GetMouseButton(0))
            {
                isDrag();
            }
            // �^�b�`�I��
            else if (Input.GetMouseButtonUp(0))
            {
                if (picBalls.Count > 0)
                {
                    StartCoroutine(TapOff());
                }
            }
        }
    }

    // �^�b�`�J�n�̍ۂɌĂяo����܂��B
    void OnTap()
    {
        // ���C�̔��ˈʒu
        var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // ���C�ɂ���������
        var hit = Physics2D.Raycast(origin, Vector2.zero);
        if (hit)
        {
            var ballAction = hit.collider.GetComponent<Ball_action>();
            // �ŏ��ɑI�������{�[���̃^�O���L�^���Ă���
            ballType = ballAction.ballType;
            // �I�𒆂̃{�[���z������������č���̃{�[����ǉ�
            picBalls = new();
            picBalls.Add(ballAction.gameObject);
            Debug.Log(picBalls[0]);

            ballAction.SendMessage("highlight");
        }
    }

    // �h���b�O���ɌĂяo����܂��B
    void isDrag()
    {
        // ���C�̔��ˈʒu
        var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // ���C�ɂ���������
        var hit = Physics2D.Raycast(origin, Vector2.zero);
        if (hit)
        {
            var ballAction = hit.collider.GetComponent<Ball_action>();
            // ����̃{�[�����I��z��ɖ��o�^�̏ꍇ�͒ǉ�����
            if (!picBalls.Contains(ballAction.gameObject))
            {
                // ���ݑI�𒆂̃{�[����ނƈ�v����ꍇ�̂ݔz��ɒǉ�
                if (ballAction.ballType == ballType && ballType != -1)
                {
                    picBalls.Add(ballAction.gameObject);
                    ballAction.SendMessage("highlight");
                }
                // �I�𒆂̃{�[����ނƈႤ��ނ��I�����ꂽ�ꍇ�̓L�����Z������
                else if (ballAction.ballType != ballType)
                {
                    clearBall();
                }
            }
        }
    }

    // �I�𒆂̂��ׂẴ{�[����ʏ��Ԃɖ߂��āA�z����N���A�[���܂��B
    void clearBall()
    {
        foreach (var ball in picBalls)
        {
            ball.SendMessage("normalCol");
        }
        picBalls.Clear();
    }

    // �^�b�`�I���̍ۂɌĂяo����܂��B
    IEnumerator TapOff()
    {
        // 3�ȏ�̃{�[�����}�b�`���Ă����ꍇ
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
            // �������������[����
            StartCoroutine(setBall(picBalls.Count));
            picBalls.Clear();
        }
        else
        {
            clearBall();
        }
    }
}
