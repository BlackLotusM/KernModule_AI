using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke : MonoBehaviour
{
	public Transform target;
	public int speed;
	public float timeRemaining = 5;
	public float timeRemaining2 = 8;
	// Use this for initialization
	void Start()
	{
		target = GameObject.FindGameObjectWithTag("enemy").transform;
		this.GetComponent<Rigidbody>().velocity = (transform.up * 10);
	}

    void FixedUpdate()
	{
		if (timeRemaining > 0)
		{
			this.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, 
				target.transform.position, speed * Time.deltaTime);
			timeRemaining -= Time.deltaTime;
        }
        else
        {
			if(timeRemaining2 > 0) { timeRemaining2 -= Time.deltaTime; } else
            {
				Destroy(this.gameObject);
            }
			Destroy(this.gameObject.GetComponent<Rigidbody>());
        }	
	}
}
