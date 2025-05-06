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
        public int inventoryIndex;
        public weaponType weaponType;

        public Item(GameObject _obj, itemType _type, weaponType _weaponType = weaponType.NonWeapon)
        {
            obj = _obj;
            type = _type;
            weaponType = _weaponType;
        }
    }

    public Dictionary<weaponType, int> ammo;

    public enum itemType
    {
        Weapon,
        UsableItem
    }

    public enum weaponType
    {
        NonWeapon,
        Pistol,
        Shotgun,
        SMG,
        Sniper,
        RPG
    }

    public int SearchForItem(string name)
    {
        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if (itemsInInventory[i].obj.name == name)
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
        {
            ammo = new Dictionary<weaponType, int>
            {
                { weaponType.NonWeapon, 0 },
                { weaponType.Pistol, 30 },
                { weaponType.Shotgun, 10 },
                { weaponType.SMG, 60 },
                { weaponType.Sniper, 5 },
                { weaponType.RPG, 2 }
            };
        }
        itemsInInventory = new Item[inventorySize];
        itemsInInventory[0] = itemPool[4];
        itemsInInventory[1] = itemPool[5];

        itemsInInventory[0].inventoryIndex = 0;
        itemsInInventory[1].inventoryIndex = 1;

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
            UseItem(itemsInInventory[currentEquippedItem]);
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ReloadWeapon();
        }
    }

    public void SwitchLoadout(int currentSelectedWeapon, int switchTarget)
    {
        foreach (Item item in itemsInInventory)
        {
            if (item == null || item.obj == null) { break; }
            if (itemPool[switchTarget].obj.name == item.obj.name)
            {
                SwitchWeaponInHand(item.inventoryIndex);
                return;
            }
        }

        int tempIndex = itemsInInventory[currentSelectedWeapon].inventoryIndex;
        itemsInInventory[currentSelectedWeapon].inventoryIndex = -1;
        itemsInInventory[currentSelectedWeapon] = itemPool[switchTarget];
        itemsInInventory[currentSelectedWeapon].inventoryIndex = tempIndex;

        itemPool[currentSelectedWeapon].obj.SetActive(false);
        itemPool[switchTarget].obj.SetActive(true);
    }
    
    public void SwitchWeaponInHand(int nextSelectedGun)
    {
        if (nextSelectedGun == currentEquippedItem) { return; }
        try
        {
            if (itemsInInventory[currentEquippedItem].obj != null) 
            {
                itemsInInventory[currentEquippedItem].obj.SetActive(false);
            }

            currentEquippedItem = nextSelectedGun;

            if (itemsInInventory[currentEquippedItem].obj != null)
            {
                itemsInInventory[currentEquippedItem].obj.SetActive(true);
            }
        } catch (System.Exception e)
        {
            Debug.LogWarning(e.Message + " || this probably came because whatever slot you're trying to switch to does not exist.");
        }
    }

    private void UseItem(Item item)
    {
        if (item.obj == null)
        {
            Debug.LogWarning("Item is null."); 
            return; 
        }

        switch (item.type)
        {
            case itemType.Weapon:
                if (itemsInInventory[currentEquippedItem].ammo <= 0) { break; }
                StartCoroutine(ShootWeapon());
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
                if (itemsInInventory[currentEquippedItem].ammo <= 0) { break; }
                itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().anim.SetTrigger("Fire");

                yield return null;
            }
        }  else
        {
            if (itemPool[currentEquippedItem].ammo <= 0) { yield return null; }

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

    public void removeAmmo()
    {
        itemsInInventory[currentEquippedItem].ammo--;
    }

    public void DropWeapon()
    {
        itemsInInventory[currentEquippedItem] = null;
        //add a function for creating a visual for dropping the item.
    }
}
