using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    public ManaBar mana;
    public float PickupDistance = 0.81f;
    // Start is called before the first frame update
    void Start()
    {
        mana = GameManager.FindObjectOfType<ManaBar>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= PickupDistance && mana.GetMana() < 100)
        {
            int number = mana.GetMana();
            if (number >= 41)
            {
                mana.SetMana(100);
                mana.PlayManaSound();
                Destroy(this.gameObject);
            }
            else
            {
                mana.SetMana(number + 60);
                mana.PlayManaSound();
                Destroy(this.gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, PickupDistance);
    }
}
