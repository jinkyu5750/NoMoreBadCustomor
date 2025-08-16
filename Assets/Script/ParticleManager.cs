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

    [Tooltip("0���� ������ ��� �ڵ� ���, ��ƼŬ�ý����� �ƴѰ�� ���� ����(�⺻�� 1��)")]
    public float returnTime = 0f;
}

public class ParticleManager : MonoBehaviour
{



    public static ParticleManager instance;

    [SerializeField] ObjectInfo[] objectInfos = null;

    [Header("������Ʈ Ǯ�� ��ġ")]
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
       
        float returnTime = GetReturnTime(objectName);
        StartCoroutine(ReturnObject(objectName, obj, returnTime));

    }

    public GameObject UseObject_GhostEffect(string objectName, Vector3 pos)
    {

        GameObject obj = objectPoolList[objectName].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;

        float returnTime = GetReturnTime(objectName);
        StartCoroutine(ReturnObject(objectName, obj, returnTime));
        return obj;
    }

    public float GetReturnTime(String objName)
    {
        ParticleSystem ps = objectPoolList[objName].Peek().GetComponent<ParticleSystem>();

        if (ps != null)
        {
            return ps.main.duration;
        }
        else
        {
            for(int i=0; i<objectInfos.Length;i++)
            {
                if (objectInfos[i].objectName == objName)
                {
                    if (objectInfos[i].returnTime != 0)
                        return objectInfos[i].returnTime;
                    else 
                        return 1f;
                                      
                }

            }


            Debug.Log("��������µ���?");
            return 0;

        }
    }
    public IEnumerator ReturnObject(string objectName, GameObject obj, float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        obj.SetActive(false);
        objectPoolList[objectName].Enqueue(obj);
    }
}
