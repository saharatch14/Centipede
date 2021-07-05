using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScrite : MonoBehaviour
{
    public float speed = 5f;
    public float deactive_Timer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.y += speed * Time.deltaTime;
        transform.position = temp;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Enemy" || target.tag == "blocker")
        {
            gameObject.SetActive(false);
        }
    }
}
