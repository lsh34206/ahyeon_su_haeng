using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    public float _speed = 3.0f;
    private PlayerStat _stat;
    
    private Texture2D _attackIcon;
    private Texture2D _handIcon;
    private int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
   
    private bool _stopSkill = false;
   
   

    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == Define.State.Die)
        {
            return;
        }

        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRUn(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRUn(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                } 
                break;
        }
    }

    private void OnMouseEvent_IdleRUn(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    _destPos = hit.point;
                    State = Define.State.Moving;
                    _stopSkill = false;
                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lookTarget = hit.collider.gameObject;
                    else
                    {
                        _lookTarget = null;
                    }

                }

                break;
            case Define.MouseEvent.Press:
                if (_lookTarget != null)
                {
                    _destPos = _lookTarget.transform.position;
                }
                else
                {
                    if (raycastHit)
                        _destPos = hit.point;

                }

                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }

    void Start()
    {
        WorldObjectType = Define.WorldObject.Player;
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction += OnMouseEvent;
    }

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    private CursorType _cursorType = CursorType.None;
    public AudioClip at;
    public AudioSource ats;

    void OnHitEvent()
    {
        ats.Play();
        if (_lookTarget != null)
        {
            Stat targetStat = _lookTarget.GetComponent<Stat>();
           targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {State = Define.State.Moving;
            
        }
        Managers.Sound.Play("/Sounds/univ0001.wav", Define.Sound.Effect);
    }
    protected override void UpdateSkill()
    {
        if (_lookTarget != null)
        {
            Vector3 dir = _lookTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }
    
    void UpdateMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Cursor.SetCursor(_attackIcon,new Vector2(_attackIcon.width/5,0), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(_handIcon,new Vector2(_handIcon.width/3,0), CursorMode.Auto);

            }
        }
    }


    protected override void UpdateMoving()
    {
        if (_lookTarget != null)
        {
            _destPos = _lookTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                NavMeshAgent nma2 = gameObject.GetOrAddComponent<UnityEngine.AI.NavMeshAgent>();
                nma2.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            return;
        }
        
        //이동
        float moveDist = Math.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);

        NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
        nma.Move(dir.normalized*moveDist);
        Debug.DrawRay(transform.position + Vector3.up*0.5f,dir.normalized,Color.green);
        if(Physics.Raycast(transform.position + Vector3.up,dir,1.0f,LayerMask.GetMask("Block")))
        {
            State = Define.State.Idle;
            return;
        }
       // transform.position += dir.normalized * moveDist;
        if (dir.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
                10 * Time.deltaTime);
        }
    }
}


