using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 3.5f;
    
    private void Update()
    {
        this.transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("end"))
        {
            Destroy(this.gameObject);
        }
    }
}
