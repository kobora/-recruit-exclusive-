using UnityEngine;

// �X�̃{�[���i�s�[�X�j��\���܂��B
public class Ball_action : MonoBehaviour
{
    // ���̃{�[���̎�ނ��w�肵�܂��B
    public int ballType = 0;
    // �n�C���C�g���ꂽ���̃T�E���h���w�肵�܂��B
    public AudioClip picSound;
    // �L�����Z�����ꂽ���̃T�E���h���w�肵�܂��B
    public AudioClip CancelSound;

    // �ʏ��Ԃ̉摜��ۑ����Ă����ϐ�
    Sprite normalBall;
    // �n�C���C�g���ꂽ���̉摜���w�肵�܂��B
    public Sprite highBall;

    // Start is called before the first frame update
    void Start()
    {
        normalBall = GetComponent<SpriteRenderer>().sprite;
    }

    // ���̃s�[�X��ʏ��Ԃɐݒ肵�܂��B
    void normalCol()
    {
        GetComponent<SpriteRenderer>().sprite = normalBall;
        AudioSource.PlayClipAtPoint(CancelSound, transform.position);
    }

    // ���̃s�[�X���n�C���C�g��Ԃɐݒ肵�܂��B
    void highlight()
    {
        GetComponent<SpriteRenderer>().sprite = highBall;
        AudioSource.PlayClipAtPoint(picSound, transform.position);
    }
}
