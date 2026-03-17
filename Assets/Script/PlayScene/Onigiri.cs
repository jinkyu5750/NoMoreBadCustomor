using UnityEngine;

public class Onigiri : Enemy
{
    [SerializeField] float speed;
    float spinSpeed;
    protected override void Start()
    {
        base.Start();
        rig.velocity = Vector3.left * speed;

        spinSpeed = Random.Range(600f, 700f);
    }
    public override void SpawnEnemy()
    {
        if (gameObject.activeSelf) return;

        transform.localPosition = spawnPos;
        transform.localRotation = spawnRot;
        gameObject.SetActive(true);
        col.enabled = true;
        rig.velocity = Vector3.left * speed;

    }
    void Update()
    {
        transform.GetChild(0).Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}


