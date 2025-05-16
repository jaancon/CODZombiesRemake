using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField]
    private VisualEffect _visualEffect;
    public int cost;
    private Animator _animator;


    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && cost <= Money.scriptInstance.money)
        {
            _animator.SetBool("Open", true);
            gameObject.GetComponent<Lootbox>().OpenLootBox();
        }
    }

    private void OnLidLifted()
    {
        _visualEffect.SendEvent("OnPlay");
    }
}
