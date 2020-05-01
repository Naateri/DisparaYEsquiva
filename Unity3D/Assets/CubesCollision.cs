using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesCollision : MonoBehaviour
{
    public AudioSource audioWood;
    private float UPPER_THRESHOLD = 9.0f;

    private ParticleSystem particle;
    //private SpriteRenderer sr ;
    
    void Start()
    {
        //audioWood = GetComponent<AudioSource>();
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

            if(transform.position.y >= UPPER_THRESHOLD){
                Destroy(collision.gameObject);
                return;
            }

            particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioWood = this.gameObject.GetComponent<AudioSource>();
            audioWood.Play();

            


            Destroy(collision.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            Destroy(this.gameObject, 2);
        }
    }
}
