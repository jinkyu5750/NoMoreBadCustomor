using UnityEngine;

public class EndlessPlatformNode : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sp;
    public float width => sp ? sp.bounds.size.x / 2 : 0.0f;
    public float height => sp ? sp.bounds.size.y / 2 : 0.0f;

    public void Destroy() => Object.Destroy(this.gameObject);

}
