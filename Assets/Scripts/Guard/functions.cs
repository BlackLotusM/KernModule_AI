using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class functions : MonoBehaviour
{
    public Guard vs;
    public bool cantAt;
    public bool DidDamage;
    public int axeDamage;
    public GameObject healthManager;
    private void Update()
    {
        vs.cantAttack.active = cantAt;
        if(vs.cantAttack.active == false)
        {
            DidDamage = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(DidDamage == false)
            {
                healthManager.GetComponent<Health>().doDamage(axeDamage);
                DidDamage = true;
            }
        }
    }
}
