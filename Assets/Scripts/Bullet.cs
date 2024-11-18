using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using  GobalReferences111;
public class Bullet : MonoBehaviour
{
    public float damage = 10f; // ×Óµ¯µÄÉËº¦Öµ
    private void OnCollisionEnter(Collision collision)
    {   


        //if (collision.gameObject.CompareTag("ground"))
        //{
        //    print("hit" + collision.gameObject.name + "!");
        //    createphoto(collision);
        //    Destroy(gameObject);

        //}

        if (!collision.gameObject.CompareTag("light")&&!collision.gameObject.CompareTag("weapon")&& !collision.gameObject.CompareTag("enemy"))
        {
            print("hit" + collision.gameObject.name + "!");
            createphoto(collision);
            Destroy(gameObject);

        }

    }

    void createphoto(Collision objectwehit)
    {
        ContactPoint contact = objectwehit.contacts[0];

        Vector3 offset = contact.normal * 0.01f;

        GameObject hole = Instantiate(GobalReferences.Instance.bulletImpactEffectPrefab,contact.point+offset,Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objectwehit.gameObject.transform);
    }
}