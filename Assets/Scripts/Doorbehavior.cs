using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class Doorbehavior : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Collider doorCollider;
    public UnityEvent onDoorOpened;
    public int doorCost;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && doorCost <= Money.scriptInstance.money)
        {
            Money.scriptInstance.LoseMoney(doorCost);
            _animator.SetBool("Open", true);
            doorCollider.enabled = false;
            onDoorOpened.Invoke();
        }
    }
}
