using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    float _followSpeed = 0;
    [SerializeField]
    bool _isComb;
    [SerializeField]
    float _combTime;
    [SerializeField]
    float _combEndTime;

    [SerializeField]//Šm”F—p
    Transform _combPoint = default;
    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _combPoint = GameObject.Find("CombPoint").GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isComb)
        {
            _combTime += Time.unscaledDeltaTime;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            TargetMove(_combPoint, _followSpeed);
        }
        else
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;

        }
        if (_combTime >= _combEndTime)
        {

            _combTime = 0;
            _isComb = false;
        }
    }
    public void TargetMove(Transform target, float speed)
    {
        //Vector3 pos = target.transform.position + _combPos;
        //target.transform.position += new Vector3(0,0.5f,5);
        //transform.position = Vector3.MoveTowards(transform.position , pos, speed);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
    }

    public void GetDamage()
    {
        _isComb = true;
    }
}
