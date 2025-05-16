using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Serializable] public class Item
    {
        public GameObject obj;
        public itemType type;
        public int inventoryIndex;
        public weaponType weaponType;
        public Vector3 originalPosition;

        public Item(GameObject _obj, itemType _type, weaponType _weaponType = weaponType.NonWeapon)
        {
            obj = _obj;
            type = _type;
            weaponType = _weaponType;
        }

        public Item() { }
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

    public Item[] itemPool;
    public Item[] itemsInInventory;

    public int inventorySize = 2;
    public int currentEquippedItem = 0;

    public Text ammoText;

    private void Start()
    {
        foreach (Item item in itemPool)
        {
            item.originalPosition = item.obj.transform.localPosition;
        }

        {
            ammo = new Dictionary<weaponType, int>
            {
                { weaponType.NonWeapon, 0 },
                { weaponType.Pistol, 3000 },
                { weaponType.Shotgun, 10 },
                { weaponType.SMG, 60 },
                { weaponType.Sniper, 5 },
                { weaponType.RPG, 2 }
            };
        }
        itemsInInventory = new Item[inventorySize];
        itemsInInventory[0] = itemPool[0];

        itemsInInventory[0].inventoryIndex = 0;

        for (int i = 1; i < itemsInInventory.Length; i++)
        {
            itemsInInventory[i] = null;
        }
    }

    private void Update()
    {
        string text = "";
        foreach (var pair in ammo)
        {
            text += pair.Key.ToString() + ", " + pair.Value.ToString() + "\n";
        }
        //ammoText.text = text;

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
        if (Input.GetMouseButton(1) && itemsInInventory[currentEquippedItem] != null)
        {
            itemsInInventory[currentEquippedItem].obj.transform.localPosition = itemsInInventory[currentEquippedItem].originalPosition +
                                                                            new Vector3(-0.3f, 0, 0);
        } else if (itemsInInventory[currentEquippedItem] != null)
        {
            itemsInInventory[currentEquippedItem].obj.transform.localPosition = itemsInInventory[currentEquippedItem].originalPosition;
        }
    }

    public void SwitchLoadout(int inventoryCurrentHand, int itemPoolNextHand)
    {
        foreach (Item item in itemsInInventory)
        {
            if (item == null) { continue; }
            if (item == itemPool[itemPoolNextHand])
            {
                SwitchWeaponInHand(item.inventoryIndex);
                return;
            }
        }

        if (itemsInInventory[currentEquippedItem] != null && itemsInInventory[currentEquippedItem].obj != null)
        {
            itemsInInventory[currentEquippedItem].obj.SetActive(false);
        }
        itemsInInventory[currentEquippedItem] = itemPool[itemPoolNextHand];
        itemsInInventory[currentEquippedItem].obj.SetActive(true);
        itemsInInventory[currentEquippedItem].inventoryIndex = currentEquippedItem;
    }
    
    public void SwitchWeaponInHand(int inventoryNextHand)
    {
        if (inventoryNextHand == currentEquippedItem) { return; }

        if (itemsInInventory[currentEquippedItem] != null &&
            itemsInInventory[currentEquippedItem].obj != null)
        {
            itemsInInventory[currentEquippedItem].obj.SetActive(false);
        }

        currentEquippedItem = inventoryNextHand;

        if (itemsInInventory[currentEquippedItem] != null &&
            itemsInInventory[currentEquippedItem].obj != null)  
        {
            itemsInInventory[currentEquippedItem].obj.SetActive(true);
        }
    }

    private void UseItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null."); 
            return; 
        }

        switch (item.type)
        {
            case itemType.Weapon:
                if (ammo[itemsInInventory[currentEquippedItem].weaponType] <= 0) { break; }
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
        print(itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().isFullAuto);
        if (itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().isFullAuto)
        {
            while (Input.GetMouseButton(0)) 
            {
                if (ammo[itemsInInventory[currentEquippedItem].weaponType] <= 0) { break; }
                itemsInInventory[currentEquippedItem].obj.GetComponent<Gun>().anim.SetTrigger("Fire");

                yield break;
            }
        }  else
        {
            if (ammo[itemPool[currentEquippedItem].weaponType] <= 0) { yield return null; }

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
        ammo[itemsInInventory[currentEquippedItem].weaponType]--;
    }

    public void DropWeapon()
    {
        itemsInInventory[currentEquippedItem] = null;
        //add a function for creating a visual for dropping the item.
    }
}
