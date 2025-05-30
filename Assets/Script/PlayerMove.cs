using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator ani;
    [SerializeField] private float speed;
    private bool isRun;

    private void Start()
    {
        ani = GetComponent<Animator>();
        isRun = false;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D)))
        {
            ani.SetBool("IsRun", true);
            isRun = true;
        }

        if (isRun == true)
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * speed);

    }

}


