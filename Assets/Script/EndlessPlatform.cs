using System.Collections.Generic;
using UnityEngine;



public class EndlessPlatformNodeContainer
{
    private readonly List<EndlessPlatformNode> nodes;
    public List<EndlessPlatformNode> Nodes => nodes;
    private readonly float moveSpeed;

    public EndlessPlatformNodeContainer(float moveSpeed)
    {
        nodes = new List<EndlessPlatformNode>();
        this.moveSpeed = moveSpeed;
    }


    //AddFirst는 필요없을거같음
    public void AddLast(EndlessPlatformNode node) => nodes.Add(node);
    public void Remove(EndlessPlatformNode node) => nodes.Remove(node);

    public void Move(Vector3 vec)
    {
        foreach (var node in nodes)
            node.transform.position += vec * moveSpeed;
    }
}
public class EndlessPlatform : MonoBehaviour
{

    [SerializeField] private Transform platform;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private bool pause = false;

    private EndlessPlatformNodeContainer container;
    [SerializeField] private List<EndlessPlatformNode> nodePrefabs;
    void Start()
    {
        container = new EndlessPlatformNodeContainer(moveSpeed);
        nodePrefabs = new List<EndlessPlatformNode>();
        var platformNodes = platform.GetComponentsInChildren<EndlessPlatformNode>();

        foreach (var node in platformNodes)
        {
            nodePrefabs.Add(node);
            node.gameObject.SetActive(false);

            var newNode = Instantiate(node.gameObject, node.transform.position, node.transform.rotation).GetComponent<EndlessPlatformNode>();
            container.AddLast(newNode);
            newNode.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (pause) return;

        SpawnPlatformNode();
        ClearPlatformNode();
        //  MovePlatformNode();


    }

    private void SpawnPlatformNode()
    {
        var node = container.Nodes[^1];
        if (Camerautils.InRightEdge(node))
        {
            var newNode = SpawnRandomNode(node, 1);
            container.AddLast(newNode);
            newNode.gameObject.SetActive(true);
        }

        node = container.Nodes[0];
   
        //앞부분 안함

    }

    private void ClearPlatformNode()
    {
        for (int i = container.Nodes.Count - 1; i >= 0; --i)
        {
            var node = container.Nodes[i];
            if (Camerautils.OutofView(node))
            {
                container.Remove(node);
                node.Destroy();
            }
        }
    }

    private EndlessPlatformNode SpawnRandomNode(EndlessPlatformNode cur, float shift)
    {
        var randomNode = nodePrefabs[Random.Range(0, nodePrefabs.Count)];
        var destination = cur.transform.position + Vector3.right * (shift * (cur.width + randomNode.width));
        var rotation = randomNode.transform.rotation;
        destination.y = randomNode.transform.position.y;

        return Instantiate(randomNode.gameObject, destination, rotation).GetComponent<EndlessPlatformNode>();
    }

}


public static class Camerautils
{
    private static Camera cam;
    public static Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = Camera.main;
            }

            return cam;
        }
    }

    public static float GetCamWidth() => Cam.orthographicSize * Screen.width / Screen.height;
    public static float GetCamHeight() => Cam.orthographicSize;

    public static bool OutofView(EndlessPlatformNode node, float threshold = 0.5f)
        => node.transform.position.x - node.width - threshold > Cam.transform.position.x + GetCamWidth()
        || node.transform.position.x + node.width + threshold <= Cam.transform.position.x - GetCamWidth();

    public static bool InLeftEdge(EndlessPlatformNode node, float threshold = 0.5f)
        => node.transform.position.x - node.width - threshold > Cam.transform.position.x - GetCamWidth();

    public static bool InRightEdge(EndlessPlatformNode node, float threshold = 0.5f)
    {
        Debug.Log( " 앞에꺼"+ (node.transform.position.x + node.width - threshold));
        Debug.Log(Cam.transform.position.x);
        return node.transform.position.x + node.width - threshold <= Cam.transform.position.x + GetCamWidth();
    }
       

}

