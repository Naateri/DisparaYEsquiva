using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesCollision : MonoBehaviour
{
    public AudioSource audioWood;
    //private ParticleSystem particle;
    //private SpriteRenderer sr ;
    
    void Start()
    {
        audioWood = GetComponent<AudioSource>();
        //sr = GetComponent<SpriteRenderer>();
    }
    /*
    private IEnumerator Break()
    {
          particle = GameObject.Find("Particle System").GetComponent<ParticleSystem>();
          particle.Play();
          sr.enabled = false;
          yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        audioWood.Play();
        yield return null;

    }
    */

    private void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "MainBullet"){
            audioWood.Play();

          
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
