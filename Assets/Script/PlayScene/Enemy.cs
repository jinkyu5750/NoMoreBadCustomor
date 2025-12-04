using UnityEngine;

public class Enemy : MonoBehaviour
{
    BoxCollider2D col;
    Rigidbody2D rig;
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
    }
    public void EnemyDead()
    {

        //지금 좀 높고 느려 -> addforce힘줄이고 중력높여
        col.enabled = false;
        rig.bodyType = RigidbodyType2D.Dynamic; // 물리 활성화
        rig.gravityScale = 1.5f;
        // 튕겨나가는 힘
        Vector2 knockbackDir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;

        rig.AddForce(knockbackDir * 5f, ForceMode2D.Impulse);

        // 회전 추가
        rig.angularVelocity = Random.Range(400f, 700f); // 빙글빙글

        // 일정 시간 후 제거
        Destroy(gameObject, 2f);

    }

}
