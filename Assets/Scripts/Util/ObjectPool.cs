using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // 오브젝트 풀 데이터를 정의할 데이터 모음 선언
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> Pools;

    // List의 경우 찾는데 O(n)
    // Dictionary의 경우 찾는데 O(1)
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            // 풀 생성
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab, transform);
                go.SetActive(false);

                objectPool.Enqueue(go);
            }

            // 태그 별로 딕셔너리에 추가
            // tag - queue
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if(!PoolDictionary.ContainsKey(tag))
            return null;

        
        // 생성한 갯수 이상으로 사용하는 경우에는 문제가 될 수 있음.
        GameObject go = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(go);

        // 우회 방법 : 사용 중인지 확인하고 사용
        // 필요하면 더 생성
        go.SetActive(true);
        return go;
    }
}
