using System.Collections;
using System.Collections.Generic;
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
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
        _exp = 0;
        _gold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
