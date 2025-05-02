using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable] public class Item
    {
        public GameObject obj;
        public itemType type;
        public int ammo;
        public bool isActive;

        public Item(GameObject _obj, itemType _type)
        {
            obj = _obj;
            type = _type;
        }
    }

    public enum itemType
    {
        Weapon,
        UsableItem
    }

    public int SearchForItem(string name)
    {
        for (int i = 0; i < itemPool.Length; i++)
        {
            if (itemPool[i].obj.name == name)
            {
                return i;
            }
        }
        return -1;
    }

    public Item[] itemPool;
    public Item[] itemsInInventory;

    public int inventorySize = 2;
    public int currentEquippedItem = 0;

    private void Start()
    {
        itemsInInventory = new Item[inventorySize];
        itemsInInventory[0] = itemPool[2];
        itemsInInventory[1] = itemPool[3];

        for (int i = 2; i < itemsInInventory.Length; i++)
        {
            itemsInInventory[i] = null;
        }

        foreach (Item item in itemPool) 
        {
            item.ammo = itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().magazineSize;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeaponInHand(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeaponInHand(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeaponInHand(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeaponInHand(3);
        }
        if (Input.GetMouseButtonDown(0))
        {
            UseItem(itemPool[currentEquippedItem]);
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ReloadWeapon();
        }
    }

    public void SwitchLoadout(int currentSelectedWeapon, int switchTarget)
    {
        itemsInInventory[currentSelectedWeapon] = itemPool[switchTarget];
        itemPool[currentSelectedWeapon].obj.SetActive(false);
        itemPool[switchTarget].obj.SetActive(true);
    }
    
    public void SwitchWeaponInHand(int nextSelectedGun)
    {
        if (itemsInInventory[nextSelectedGun] == null) { return; }
        try
        {
            itemsInInventory[currentEquippedItem].obj.SetActive(false);
            currentEquippedItem = nextSelectedGun;
            itemsInInventory[currentEquippedItem].obj.SetActive(true);
        } catch (System.Exception e)
        {
            Debug.LogError(e.Message + "|| this probably came because whatever slot you're trying to switch to does not exist.");
        }
    }

    private void UseItem(Item item)
    {
        switch (item.type)
        {
            case itemType.Weapon:
                if (itemPool[currentEquippedItem].ammo <= 0) { break; }
                StartCoroutine(ShootWeapon());
                item.ammo--;
                break;
            case itemType.UsableItem:
                break;
            default:
                Debug.LogWarning("Attempted to use item but item type was not accounted for in UseItem(Item item)");
                break;
        }
    }

    private IEnumerator ShootWeapon()
    {

        if (itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().isFullAuto)
        {
            while (Input.GetMouseButton(0)) 
            {
                itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().anim.SetTrigger("Fire");
                yield return null;
            }
        }  else
        {
            itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().anim.SetTrigger("Fire");
        }
    }

    private void ReloadWeapon() 
    {   
        if (itemsInInventory[currentEquippedItem].type == itemType.Weapon) 
        {
            itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().anim.SetTrigger("Reload");;
        }
    }
}
