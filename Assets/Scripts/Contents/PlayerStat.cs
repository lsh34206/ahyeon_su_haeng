using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int _gold;
    [SerializeField] protected int _exp;
    
    public int Exp
    {
        get {
            return _exp;
        }
        set {
            _exp = value;
        }
    }
    
    public int Gold
    {
        get {
            return _gold;
        }
        set {
            _gold = value;
        }
    }
    

    
 

    // Start is called before the first frame update
    void Start()
    {
        _level = 1;
        _hp = 200;
        _maxHp =200;
        _attack = 25;
        _defense = 5;
        _moveSpeed = 5.0f;
        _exp = 0;
        _gold = 0;
        SetStat(_level);
    }

    private void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dic = Managers.Data.StatDic;
        Data.Stat stat = dic[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }
    protected override void OnDead(Stat attacker)
    {
        Debug.Log("d");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
