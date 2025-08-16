using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    //�� �÷����� ��ũ��Ʈ ����, �´����� ���� �÷��� ����, Exit�ϸ� ����(�� REturnTime�༭ ���ֱ������ Exit X), �÷��� ���� x += 23.5

    [SerializeField]private bool isGenerated = false;

    private void OnEnable()
    {
        isGenerated = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if(collision.gameObject.CompareTag("Player") && isGenerated == false)
        {
            isGenerated = true;
            int num = Random.Range(1, 4);
            ParticleManager.instance.UseObject($"Platform{num}", transform.parent.parent.position+new Vector3(23.5f,0,0),Quaternion.identity);
        }
        else
        {
            Debug.Log("ss");
        }
    }

}
