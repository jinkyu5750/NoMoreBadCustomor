using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGenerator : MonoBehaviour
{

    //°¢ ÇÃ·§Æû¿¡ ½ºÅ©¸³Æ® ºÎÂø, ¸Â´êÀ¸¸é ´ÙÀ½ ÇÃ·§Æû »ý¼º, ExitÇÏ¸é ¸®ÅÏ(°Á REturnTimeÁà¼­ ¾ø¾Ö±â·ÎÇÏÁÒ Exit X), ÇÃ·§Æû »ý¼º x += 23.5

    private bool isGenerated = false;
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
            Debug.Log("Platform Generator Error");
        }
    }

   
}
