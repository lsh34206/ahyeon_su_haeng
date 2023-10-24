using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    private Stat _stat;

    [SerializeField] private float _scanRange = 10;
    [SerializeField] private float _attackRange = 2;
    // Start is called before the first frame update
    void Start()
    {

        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }
    }

    protected override void UpdateIdle()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lookTarget = player;
            State = Define.State.Moving;
            return;
        }
    }
    
    protected override void UpdateMoving()
    {
        if (_lookTarget != null)
        {
            _destPos = _lookTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)
            {
                State = Define.State.Skill;
                return;
            }
        }
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            return;
        }

        NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
        nma.SetDestination(_destPos);
        nma.speed = _stat.MoveSpeed;
        //이동
        Debug.DrawRay(transform.position + Vector3.up*0.5f,dir.normalized,Color.green);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f,dir, 1.0f, LayerMask.GetMask("Block")))
        {
            if (Input.GetMouseButton(0) == false)
            {
                State = Define.State.Idle;
                return;
            }

            float movDist = Math.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * movDist;
            transform.rotation =Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),50*Time.deltaTime );
        }
    }
    
    protected override void UpdateSkill()
    {
        if(_lookTarget != null)
        {
            Vector3 dir = _lookTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation,quat,20*Time.deltaTime);
        }
    }
    void OnHitEvent()
        {
            if (_lookTarget != null)
            {
                Stat targetStat = _lookTarget.GetComponent<Stat>();
                int damage = Mathf.Max(0, _stat.Attack - targetStat.Defense);
                targetStat.Hp -= damage;
                
                if (targetStat.Hp <= 0)
                {
                    Managers.Game.Despwan(targetStat.gameObject);
                }
                if (targetStat.Hp > 0)
                {
                    float distance = (_lookTarget.transform.position - transform.position).magnitude;
                    if (distance <= _attackRange)
                        State = Define.State.Skill;
                    else
                    {  
                        State = Define.State.Moving;
                    }
                }
                else
                {
                    State = Define.State.Idle;
                }
            }
            else
            {
                State = Define.State.Idle;
            }
            
        }
    // Update is called once per frame
    void Update()
    {
        
    }
}
