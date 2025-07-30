using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectInfo
{
    public string objectName;
    public GameObject prefab;
    public int count;

    [Tooltip("0으로 설정할 경우 자동 계산, 파티클시스템이 아닌경우 직접 설정(기본값 1초)")]
    public float returnTime = 0f;
}

public class ParticleManager : MonoBehaviour
{



    public static ParticleManager instance;

    [SerializeField] ObjectInfo[] objectInfos = null;

    [Header("오브젝트 풀의 위치")]
    [SerializeField] Transform tfPoolParent;

    public Dictionary<string, Queue<GameObject>> objectPoolList;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        objectPoolList = new Dictionary<string, Queue<GameObject>>();
        ObjectPoolinit();
    }

    public void ObjectPoolinit()
    {
        if (objectInfos != null)
        {
            for (int i = 0; i < objectInfos.Length; i++)
            {
                objectPoolList.Add(objectInfos[i].objectName, InsertQueue(objectInfos[i]));
            }
        }
    }

    public Queue<GameObject> InsertQueue(ObjectInfo obj)
    {
        Queue<GameObject> tempQ = new Queue<GameObject>();

        for (int i = 0; i < obj.count; i++)
        {
            GameObject clone = Instantiate(obj.prefab) as GameObject;
            clone.SetActive(false);
            clone.transform.SetParent(tfPoolParent);
            tempQ.Enqueue(clone);
        }

        return tempQ;
    }


    public void UseObject(string objectName, Vector3 pos,Quaternion rot)
    {

        GameObject obj = objectPoolList[objectName].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        float returnTime = GetReturnTime(obj);
        StartCoroutine(ReturnObject(objectName, obj, returnTime));

    }

    public GameObject UseObject_GhostEffect(string objectName, Vector3 pos)
    {

        GameObject obj = objectPoolList[objectName].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;

        float returnTime = GetReturnTime(obj);
        StartCoroutine(ReturnObject(objectName, obj, returnTime));
        return obj;
    }

    public float GetReturnTime(GameObject obj)
    {
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            return ps.main.duration;
        }
        else
            return 1f;
    }
    public IEnumerator ReturnObject(string objectName, GameObject obj, float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        obj.SetActive(false);
        objectPoolList[objectName].Enqueue(obj);
    }
}
