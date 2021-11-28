using UnityEngine;
using System.Collections;
using UnityEngine.AI;
namespace DigitalRuby.PyroParticles
{
    public interface ICollisionHandler
    {
        void HandleCollision(GameObject obj, Collision c);
    }

    /// <summary>
    /// This script simply allows forwarding collision events for the objects that collide with something. This
    /// allows you to have a generic collision handler and attach a collision forwarder to your child objects.
    /// In addition, you also get access to the game object that is colliding, along with the object being
    /// collided into, which is helpful.
    /// </summary>
    public class FireCollisionForwardScript : MonoBehaviour
    {
        GameObject collider;

        public ICollisionHandler CollisionHandler;
        IEnumerator NPCDisapper(GameObject npc)
        {
            yield return new WaitForSeconds(3);
            Destroy(npc.gameObject);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag != "Player")
            {
                CollisionHandler.HandleCollision(gameObject, collision);
                if (collision.transform.tag == "Spawner")
                {
                    Destroy(collision.gameObject);
                }

                if (collision.transform.tag == "Archer")
                {
                    collision.gameObject.GetComponent<EnemyHealth>().TakeDamages(100);
                }

                if (collision.transform.tag == "KingsGuard")
                {
                    collision.gameObject.GetComponent<NewKGController>().TakeDamages(150);
                }

                if (collision.transform.tag == "Boss")
                {
                    collision.gameObject.GetComponent<BossController>().TakeDamages(25);
                }

                if (collision.transform.tag == "NPCPeseant")
                {
                    Animator NPCAnimation = collision.gameObject.GetComponent<Animator>();
                    NavMeshAgent NPCnav = collision.gameObject.GetComponent<NavMeshAgent>();
                    NPCAnimation.SetTrigger("isDead");
                    NPCnav.isStopped = true;
                    StartCoroutine(NPCDisapper(collision.gameObject));
                }
            }
        }
    }
}
