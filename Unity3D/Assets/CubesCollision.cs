using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MenuToGame;

public class CubesCollision : MonoBehaviour
{
    public AudioSource audioWood;
    private float UPPER_THRESHOLD = 9.0f;
    private ParticleSystem particle;
    public Font myFont;

    void Start()
    {
        Physics.IgnoreLayerCollision(9, 9);
    }

    private IEnumerator Break(Collision collision)
    {
        particle = this.gameObject.GetComponent<ParticleSystem>();
        particle.Play();
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;
        audioWood = this.gameObject.GetComponent<AudioSource>();
        audioWood.Play();

        globalGameInfo.Shots_hit++;
        globalGameInfo.Enemies_Destroyed++;
        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);

        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MainBullet")
        {
            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }
            StartCoroutine(Break(collision));
            MenuToGame.Score++;

        }

        else if (collision.gameObject.tag == "Bullet_P2")
        {
            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }
            StartCoroutine(Break(collision));
            MenuToGame.Score2++;

        }
    }
}
