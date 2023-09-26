using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// �Q�[���S�̂̐i�s���Ǘ����܂��B
public class timer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���b�Z�[�WUI���w�肵�܂��B")]
    private TextMeshProUGUI title = null;
    [SerializeField]
    [Tooltip("�X�R�AUI���w�肵�܂��B")]
    private TextMeshProUGUI score = null;

    // �v���C����(�b)
    float nowTime = 0;
    // �v���C���̏ꍇ��true�A����ȊO��false
    bool isPlaying = true;
    // �v���C�O�̏ꍇ��true�A�v���C�J�n��false
    static public bool isWait = true;
    // ���݂̃X�R�A
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
        // �X�R�A�\�����X�V
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

    // �S�Ẵs�[�X���Î~���܂ŏ��������s���܂��B
    IEnumerator ballCheck()
    {
        // �J�n��1�b�͖������ɑҋ@
        yield return new WaitForSeconds(1);

        // �S�Ẵs�[�X�I�u�W�F�N�g�����܂�����isWait��false�ɐݒ肵��
        // while�����𔲂���
        while (true)
        {
            bool moves = false;
            var balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (var ball in balls)
            {
                // �s�[�X���܂������Ă��邩�ǂ����𔻒�
                if (ball.GetComponent<Rigidbody2D>().velocity.magnitude > 0.3f)
                {
                    moves = true;
                    break;
                }
            }
            // �Î~��Ԃ𔻒�
            if (!moves)
            {
                isWait = false;
                break;
            }

            yield return null;
        }

        // �v���C�J�n�����o����
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
