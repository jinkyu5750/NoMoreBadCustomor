using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private float ghostDelay;
    private float curGhostDelay;

    private bool _isGhostOn;
    public bool IsGhostOn
    {
        set => _isGhostOn = value;
    }


    // �뽬�ϸ� Player��ũ��Ʈ���� OnOff���ֱ� -> Update�� �ֱ⸶�� ��ü�� ��������Ʈ ����ȭ �� Instantiate  -> Destroy(gameObject) 
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
                GameObject ghost = ParticleManager.instance.UseObject_GhostEffect("GhostEffect", transform.position);
                ghost.GetComponent<SpriteRenderer>().sprite = sp.sprite;
                curGhostDelay = ghostDelay;
            }
        }
  
    }


}
