using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEffect : MonoBehaviour
{
    [SerializeField]
    private bool collisionEnter = true;
    public UnityEvent<GameObject> collisionEnterAction;

    [SerializeField]
    private bool collisionExit = false;
    public UnityEvent<GameObject> collisionExitAction;

    [SerializeField]
    private bool collisionStay = false;
    public UnityEvent<GameObject> collisionStayAction;

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionEnter)
        {
            collisionEnterAction.Invoke(collision.gameObject);

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collisionExit)
        {
            collisionExitAction.Invoke(collision.gameObject);

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collisionStay)
        {
            collisionStayAction.Invoke(collision.gameObject);

        }
    }
}
