using UnityEngine;

public class ReceiptManget : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Receipt"))
            other.GetComponent<Receipt>().SetStartMagnet();
    }
}
