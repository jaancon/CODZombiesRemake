using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbox : MonoBehaviour
{
    [Serializable] public class Meshes
    {
        public Mesh mesh;
        public int itemPoolIndex;
    }

    public GameObject lootboxItem;
    public Mesh[] meshes;
    private MeshFilter lbiFilter;
    private bool hasOpenedBox = false;
    private bool hasStoppedBox = false;

    private void Start()
    {
        lbiFilter = lootboxItem.GetComponent<MeshFilter>();
        lbiFilter.mesh = null;
    }

    public void OpenLootBox()
    {
        if (!hasOpenedBox)
        {
            StartCoroutine(LootboxAnimation());
            hasOpenedBox = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hasOpenedBox && !hasStoppedBox)
        {
            hasStoppedBox = true;
        }
    }

    private IEnumerator LootboxAnimation()
    {
        Player playerScript = GameObject.Find("Player").GetComponent<Player>();

        while (!hasStoppedBox)
        {
            lbiFilter.mesh = meshes[UnityEngine.Random.Range(0, meshes.Length)];
            yield return new WaitForSeconds(0.3f);
        }

        int randomNumber = UnityEngine.Random.Range(0, meshes.Length);
        lbiFilter.mesh = meshes[randomNumber];
        
        string name = lbiFilter.GetComponent<MeshFilter>().mesh.name;
        name.Remove(name.Length - 9);
        Debug.Log(name);

        playerScript.SwitchLoadout(playerScript.currentEquippedItem, randomNumber);
    }
}
