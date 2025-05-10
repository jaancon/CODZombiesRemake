using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float muzzleVelocity;
    public GameObject bullet;
    public bool isFullAuto;
    public GameObject firePoint;
    public Animator anim;
    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    /*public void ReloadAnimationEnds()
    {
        player.ammo[player.itemsInInventory[player.currentEquippedItem].weaponType] = magazineSize;
        print("Successfully Reloaded. " + player.ammo[player.itemsInInventory[player.currentEquippedItem].weaponType]);
        anim.ResetTrigger("Reload");
    }*/
    
    public void FireAnimationBegins()
    {
        GameObject temp_bullet = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
        temp_bullet.GetComponent<Rigidbody>().velocity = temp_bullet.transform.forward * muzzleVelocity;
        temp_bullet.transform.Rotate(new Vector3(0,180,0));

        player.removeAmmo();
        anim.ResetTrigger("Fire");
        Debug.Log("Fired Weapon. ");
    }
}
