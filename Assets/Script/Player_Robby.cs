using System.Collections;
using UnityEngine;

public class Player_Robby : MonoBehaviour
{

    public enum CurDir { Left = -1, Right, Idle }
    private Animator ani;
    private Rigidbody2D rig;
    private SpriteRenderer sp;

    [SerializeField] private float idleTime;
    [SerializeField] private float walkTime;
    [SerializeField] private float walkSpeed;

    [SerializeField] private float curIdleTime;
    [SerializeField] private float curWalkTime;

    [SerializeField] CurDir curDir; // -1,0,1 (left,right,idle)
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

        curDir = CurDir.Idle;
        curIdleTime = Random.Range(0, idleTime + 1);
    }


    private void Update()
    {

        if (curIdleTime > 0 && curDir == CurDir.Idle) //Idle상태
            curIdleTime -= Time.deltaTime;
        else if (curIdleTime <= 0 && curDir == CurDir.Idle)
        {

            StartCoroutine(Walk());   // 여기서 어차피 curDir바꿔주니까 다시 안들어감

        }
    }

    public void SetDirection()
    {

        curDir = (CurDir)Random.Range(-1, 1);
        ani.SetFloat("WalkSpeed", 1f);
    }
    public IEnumerator Walk()
    {
        SetDirection();
        curWalkTime = Random.Range(1, walkTime + 1);

        while (curWalkTime >= 0)
        {
            curWalkTime -= Time.deltaTime;
            yield return null;

            sp.flipX = (int)curDir < 0 ? true : false;
            rig.velocity = ((int)curDir < 0 ? Vector2.left : Vector2.right) * walkSpeed; // curDir이 바뀌어도  OK
        }

        rig.velocity = Vector2.zero;
        curDir = CurDir.Idle;
        ani.SetFloat("WalkSpeed", 0f);
        curIdleTime = idleTime;
    }
    //랜덤한 시간 서있다가.. 움직임 (왼쪽 or 오른쪽) But 화면밖을 벗어날수는없음
    //서있는시간이 걷는시간보다 길면 좋겠음

 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RobbyBoundary"))
        {
            curDir = (int)curDir < 0 ? CurDir.Right : CurDir.Left;
        }
    }
}
