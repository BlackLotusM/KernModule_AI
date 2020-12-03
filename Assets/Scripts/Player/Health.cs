using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public Guard es;
    [SerializeField]
    private float startHealth = 100;
    public GameObject player;
    [SerializeField]
    private float health;
    public Camera cam;
    public bool hasbeentrig;
    public Slider slide;

    void Start()
    {
        hasbeentrig = false;
        health = startHealth;
        slide.value = health;
    }

    public void doDamage(int damage)
    {
        health -= damage;
        slide.value = health;
        if (health <= 0)
        {
            if (!hasbeentrig)
            {
                trigDeath();
            }
        }
    }

    private void trigDeath()
    {
        hasbeentrig = true;
        StartCoroutine(tirg());
        Destroy(player);
        Camera t = Instantiate(cam, cam.transform.position, cam.transform.rotation);
        t.GetComponent<Camera>().enabled = true;
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;    
        
    }

    IEnumerator tirg()
    {
        es.ClosestPlayer.active = null;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("StartMenu");
    }
}
