using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] private int _monsterCount = 0;
    [SerializeField] private int _keepMonsterCount = 0;

    [SerializeField] private Vector3 _spawnPos;

    [SerializeField] private float _spawnRadius = 15.0f;
    [SerializeField] private float _spawnTime = 5.0f;

    private int _reserveCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Game.OnSpawnEvent += AddmonsterCount;
    }

    public void AddmonsterCount(int value)
    {
        _monsterCount += value;
    }

    public void SetKeepMonsterCount(int count)
    {
        _keepMonsterCount = count;
    }
    // Update is called once per frame
    void Update()
    {
        while (_reserveCount + _monsterCount<_keepMonsterCount)
        {
           StartCoroutine("ReserveSpawn");
        }
        
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));

        GameObject obj = Managers.Game.Spwan(Define.WorldObject.Monster, "knight");

        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();
        Vector3 randPos;
        while (true)
        {
            Vector3 randomDir = Random.insideUnitCircle * Random.Range(0, _spawnRadius);
            randomDir.y = 0;
            randPos = _spawnPos + randomDir;

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
            {
                break;
            }

            
        }
        obj.transform.position = randPos;
        _reserveCount--;
    }
}
    

