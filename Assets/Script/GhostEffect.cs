using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefeb;
    private SpriteRenderer sp;
    [SerializeField] private float ghostDelay;
    private float curGhostDelay;

    private bool _isGhostOn;
    public bool IsGhostOn
    {
        set => _isGhostOn = value;
    }


    // 대쉬하면 Player스크립트에서 OnOff해주기 -> Update로 주기마다 본체의 스프라이트 동기화 후 Instantiate  -> Destroy(gameObject) 
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        curGhostDelay = ghostDelay;
    }


    void Update()
    {

        if (ghostDelay <= 0)
        {
            Debug.Log("ghostDelay must be bigger then 0");
            return;
        }

        if (_isGhostOn == true)
        {

            if (curGhostDelay > 0)
                curGhostDelay -= Time.deltaTime;
            else
            {
                GameObject ghost = Instantiate(ghostPrefeb, transform.position, transform.rotation);
                ghost.GetComponent<SpriteRenderer>().sprite = sp.sprite;
                Destroy(ghost, 1f);
                curGhostDelay = ghostDelay;
            }
        }
  
    }


}
