using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using StarterAssets;

public class ArrowDamage : MonoBehaviour
{
    public float _destroyTime;
    public int _damage;
    public Transform target;
    public GameObject MainPlayer;
    public AudioSource audioSource;
    public AudioClip shieldBlockSound;
    private bool playedalready = false;

    [SerializeField]
    AudioClip arrowDamageSound;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = this.gameObject.transform;
        if (GameManager.Instance != null)
        {
            MainPlayer = GameManager.Instance.Player;
        }
      
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {

        if (MainPlayer == null)
        {
            return;
        }
        Vector3 targetDir = target.position - MainPlayer.transform.position;

        float angle = Vector3.Angle(targetDir, MainPlayer.transform.forward );
        if (GameManager.Instance.Player.GetComponent<Animator>().GetBool("isBlocking") && angle < 86 && angle > 25)
        {
            audioSource.clip = shieldBlockSound;
            audioSource.Play();
        }
       else if (collision.transform.tag == "Player")
        {

            collision.gameObject.GetComponent<ThirdPersonController>().TakeDamage(_damage);
            audioSource.clip = arrowDamageSound;
            if (!playedalready)
            {
                audioSource.Play();
                playedalready = true;
            }

        }
        _damage = 0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void SetDamage(int amount)
    {
        _damage = amount;
    }
}
