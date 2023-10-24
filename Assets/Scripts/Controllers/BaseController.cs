using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{ [SerializeField]protected GameObject _lookTarget;
    [SerializeField]protected Vector3 _destPos;
    [SerializeField]protected Define.State _state = Define.State.Idle;

    private GameObject _player;
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    private void Update()
    {
       

        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT",0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN",0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK",0.1f,-1,0);
                    break;
            }
        }
    }
  
protected virtual void UpdateDie(){}
protected virtual void UpdateMoving(){}
protected virtual void UpdateIdle(){}
protected virtual void UpdateSkill(){}
    // Update is called once per frame
  
}
