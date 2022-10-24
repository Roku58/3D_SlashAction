using UnityEngine;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class FlickAction : MonoBehaviour
{
    public enum FlickState
    {
        NONE,
        TAP,
        UP,
        RIGHT,
        DOWN,
        LEFT,
    }

    [SerializeField]
    private FlickState _nowSwipe = FlickState.NONE;
    public FlickState NowSwipe { get => _nowSwipe; set => _nowSwipe = value; }

    [SerializeField]
    float _stateReSetTime = 0.5f;
    [SerializeField]
    float _flickRange = 30f;
    [SerializeField]
    float _speed;

    private Vector3 _touchStartPos;
    private Vector3 _touchEndPos;

    private Vector3 _lineStartPos;
    private Vector3 _lineEndPos;

    GameObject _enemy;
    GameObject _target;
    LineRenderer _line;
    Animator _animator;
    CinemachineImpulseSource _impulseSource = default;


    /// <summary>�U���͈͂̒��S</summary>
    [SerializeField, Header("�U���͈͂̒��S")]
    Vector3 _attackRangeCenter = default;
    /// <summary>�U���͈͂̔��a</summary>
    [SerializeField, Header("�U���͈͂̔��a")]
    float _attackRangeRadius = 1f;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _animator = GetComponent<Animator>();
        _enemy = GameObject.FindWithTag("Enemy");
        _target = GameObject.FindWithTag("Target");
        _impulseSource = GetComponent<CinemachineImpulseSource>();

    }

    private void Update()
    {
        //if(_battele.IsBattle)
        //{
            if (_nowSwipe == FlickState.NONE)
            {
                Flick();
            }
        //}


    }

    /// <summary>
    /// �t���b�N���͎�t�ƃ��C���\��
    /// </summary>
    void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _touchStartPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _lineStartPos = Camera.main.ScreenToWorldPoint(_touchStartPos);
            _lineEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _lineStartPos.z = 0;
            _lineEndPos.z = 0;

            //Debug.Log(_touchStartPos);
            //Debug.Log(_touchEndPos);

            if (_nowSwipe == FlickState.NONE)
            {
                _line.enabled = true;

                _line.SetPosition(0, _touchStartPos);
                _line.SetPosition(1, _touchEndPos);
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _touchEndPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            _lineEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineEndPos.z = 0;
            _line.SetPosition(1, _touchEndPos);

            GetDirection();
        }
    }

    /// <summary>
    /// �t���b�N��������
    /// </summary>
    void GetDirection()
    {
        float directionX = _touchEndPos.x - _touchStartPos.x;
        float directionY = _touchEndPos.y - _touchStartPos.y;
        string Direction;

        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (_flickRange < directionX)
            {
                //�E�����Ƀt���b�N

                ChangeState(FlickState.RIGHT);
            }

            else if (-_flickRange > directionX)
            {
                //�������Ƀt���b�N

                ChangeState(FlickState.LEFT);
            }
        }

        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (_flickRange < directionY)
            {
                //������Ƀt���b�N

                ChangeState(FlickState.UP);
            }
            else if (-30 > directionY)
            {
                //�������̃t���b�N

                ChangeState(FlickState.DOWN);
            }
        }
        else
        {
            //�^�b�`�����o
            ChangeState(FlickState.TAP);
        }


    }

    /// <summary>
    /// ��ԕύX����1�񂾂��Ă΂�鏈��
    /// </summary>
    /// <param name="next"></param>
    public void ChangeState(FlickState next)
    {
        // �ȑO�̏�Ԃ�ێ�
        var prev = _nowSwipe;
        // ���̏�ԂɕύX����
        _nowSwipe = next;
        //Debug.Log($"�t���b�N���� {prev} -> {next}");
        switch (_nowSwipe)
        {
            case FlickState.NONE:
                {

                }
                break;
            case FlickState.UP:
                {
                    if(_animator)
                    {
                        _animator.SetTrigger("Attack_Up");

                    }

                    StartCoroutine(StateReSet());

                }
                break;
            case FlickState.DOWN:
                {
                    if (_animator)
                    {
                        _animator.SetTrigger("Attack_Down");

                    }
                    StartCoroutine(StateReSet());

                }
                break;
            case FlickState.RIGHT:
                {
                    if(_animator)
                    {
                        _animator.SetTrigger("Attack_Right");

                    }

                    StartCoroutine(StateReSet());

                }
                break;
            case FlickState.LEFT:
                {
                    if(_animator)
                    {
                        _animator.SetTrigger("Attack_Left");

                    }

                    StartCoroutine(StateReSet());

                }
                break;
            case FlickState.TAP:
                {
                    if(_animator)
                    {
                        _animator.SetTrigger("Attack_Tap");

                    }

                    StartCoroutine(StateReSet());
                }
                break;
        }
    }

    /// <summary>
    /// FlickState���Z�b�g����
    /// </summary>
    /// <returns></returns>
    IEnumerator StateReSet()
    {
        if(_nowSwipe != FlickState.NONE)
        {
            yield return new WaitForSeconds(_stateReSetTime);
            _line.enabled = false;
            ChangeState(FlickState.NONE);
        }
    }

    void OnDrawGizmosSelected()
    {
        // �U���͈͂�Ԃ����ŃV�[���r���[�ɕ\������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackRangeCenter(), _attackRangeRadius);
    }

    /// <summary>
    /// �U���͈͂̒��S���v�Z���Ď擾����
    /// </summary>
    /// <returns>�U���͈͂̒��S���W</returns>
    Vector3 GetAttackRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackRangeCenter.z
            + this.transform.up * _attackRangeCenter.y
            + this.transform.right * _attackRangeCenter.x;
        return center;
    }

    void Attack()
    {
        // �U���͈͂ɓ����Ă���R���C�_�[���擾����
        var hitObj = Physics.OverlapSphere(GetAttackRangeCenter(), _attackRangeRadius);

        foreach (Collider enemy in hitObj)
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                _impulseSource.GenerateImpulse(new Vector3(0, 0, -1));
            }
        }
    }


    void TargetMove()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }


}
