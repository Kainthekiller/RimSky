using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEffect : MonoBehaviour
{

    [SerializeField]
    private bool triggerEnter = true;
    public UnityEvent<Collider> triggerEnterAction;

    [SerializeField]
    private bool triggerExit = false;
    public UnityEvent<Collider> triggerExitAction;

    [SerializeField]
    private bool triggerStay = true;
    public UnityEvent<Collider> triggerStayAction;

    private void OnTriggerEnter(Collider other) {
        if (triggerEnter)
            triggerEnterAction.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {

        if (triggerStay)
            triggerStayAction.Invoke(other);
    }
    private void OnTriggerExit(Collider other) {
        if (triggerExit)
            triggerExitAction.Invoke(other);
    }


}
