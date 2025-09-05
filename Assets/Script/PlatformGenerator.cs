using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGenerator : MonoBehaviour
{

    //�� �÷����� ��ũ��Ʈ ����, �´����� ���� �÷��� ����, Exit�ϸ� ����(�� REturnTime�༭ ���ֱ������ Exit X), �÷��� ���� x += 23.5

    [SerializeField]private bool isGenerated = false;
    private Vector3 spawnPoint;

    private void OnEnable()
    {
        isGenerated = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if(collision.gameObject.CompareTag("Player") && isGenerated == false)
        {
            spawnPoint = transform.Find("SpawnPoint").position;
  
            isGenerated = true;
            int num = Random.Range(1, 4);
            ParticleManager.instance.UseObject($"Platform{num}",spawnPoint+new Vector3(3,0,0),Quaternion.identity);
   
        }
        else
        {
            Debug.Log("ss");
        }
    }

}
