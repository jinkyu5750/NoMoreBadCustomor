using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [Header("풀 리필 갯수")]
    [SerializeField] int refillCount = 7;
    [Header("오브젝트 풀의 위치")]
    [SerializeField] Transform tfPoolParent;

    public Dictionary<string, Queue<GameObject>> objectPoolList;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    public void FillQueue(string objectName)
    {
        for (int i = 0; i < objectInfos.Length; i++)
        {
            if (objectInfos[i].objectName == objectName)
            {
                for (int j = 0; j < refillCount; j++)
                {
                    GameObject clone = Instantiate(objectInfos[i].prefab) as GameObject;
                    clone.SetActive(false);
                    clone.transform.SetParent(tfPoolParent);
                    objectPoolList[objectName].Enqueue(clone);
                }
            }
        }

    }
    public void UseObject(string objectName, Vector3 pos, Quaternion rot)
    {
        if (!objectPoolList.ContainsKey(objectName))
        {
            Debug.LogWarning($"Object pool for '{objectName}' not found!");
            return;
        }

        if (objectPoolList[objectName].Count == 0)
            FillQueue(objectName);

        float returnTime = GetReturnTime(objectName);

        GameObject obj = objectPoolList[objectName].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        if (objectName.StartsWith("Platform"))
        {
            ReceiptSpawner(obj);
            foreach (var e in obj.GetComponentsInChildren<Enemy>(true))
                e.SpawnEnemy();

        }
        StartCoroutine(ReturnObject(objectName, obj, returnTime));

    }


    public GameObject UseObject_GhostEffect(string objectName, Vector3 pos)
    {

        if (!objectPoolList.ContainsKey(objectName))
        {
            Debug.LogWarning($"Object pool for '{objectName}' not found!");
            return null;
        }

        if (objectPoolList[objectName].Count == 0)
            FillQueue(objectName);

        float returnTime = GetReturnTime(objectName);

        GameObject obj = objectPoolList[objectName].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;


        StartCoroutine(ReturnObject(objectName, obj, returnTime));
        return obj;
    }

    public void UseClickEffect(Vector3 pos)
    {
        if (!objectPoolList.ContainsKey("Click"))
        {
            Debug.LogWarning($"Object pool for Click not found!");
            return;

        }

        if (objectPoolList["Click"].Count == 0)
            FillQueue("Click");

        float returnTime = GetReturnTime("Click");

        GameObject obj = objectPoolList["Click"].Dequeue();

        obj.transform.SetParent(UIManager.Instance.canvas.transform, false);
        obj.SetActive(true);
        obj.GetComponent<RectTransform>().localPosition = pos;
       
        StartCoroutine(ReturnObject("Click", obj, returnTime));

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
            for (int i = 0; i < objectInfos.Length; i++)
            {
                if (objectInfos[i].objectName == objName)
                    return objectInfos[i].returnTime != 0 ? objectInfos[i].returnTime : 1f;

            }


            Debug.Log("문제생겼는데요?");
            return 0;

        }
    }
    public IEnumerator ReturnObject(string objectName, GameObject obj, float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        if (obj.activeSelf)
            obj.SetActive(false);
        objectPoolList[objectName].Enqueue(obj);
    }

    public void ReceiptSpawner(GameObject platform)
    {
        float spawnInterval = 1f;
        Transform _lines = platform.transform.Find("Pivot/Platform/Lines");
        int lineCnt = _lines.childCount;
        LineRenderer[] lines = new LineRenderer[lineCnt];


        for (int i = 0; i < lineCnt; i++)
        {
            lines[i] = _lines.GetChild(i).GetComponent<LineRenderer>();
            int vertexCnt = lines[i].positionCount;
            Vector3[] pos = new Vector3[vertexCnt];
            lines[i].GetPositions(pos);

            float lineLength = 0f;
            for (int j = 0; j < vertexCnt - 1; j++)
                lineLength += Vector3.Distance(pos[j], pos[j + 1]);

            float distanceFromSpawn = 0f;
            int currentSegment = 0;
            float segmentStartDistance = 0f;

            while (distanceFromSpawn <= lineLength)
            {
                float segmentLength = Vector3.Distance(pos[currentSegment], pos[currentSegment + 1]);
                float t = (distanceFromSpawn - segmentStartDistance) / segmentLength;
                t = Mathf.Clamp01(t);

                Vector3 spawnPos = Vector3.Lerp(pos[currentSegment], pos[currentSegment + 1], t);
                Vector3 worldSpawnPos = lines[i].transform.TransformPoint(spawnPos);

                UseObject("Receipt", worldSpawnPos, Quaternion.identity);

                distanceFromSpawn += spawnInterval;

                while (currentSegment < vertexCnt - 1 && (distanceFromSpawn - segmentStartDistance) > segmentLength)
                {
                    segmentStartDistance += segmentLength;
                    currentSegment++;
                    if (currentSegment >= vertexCnt - 1) break;
                    segmentLength = Vector3.Distance(pos[currentSegment], pos[currentSegment + 1]);
                }
            }
        }
    }
    //플랫폼에서 라인들을 다 받아와
}
