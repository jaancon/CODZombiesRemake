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
    public int cost;
    Mesh currentMesh;
    Mesh previousMesh;
       
    private void Start()
    {
        currentMesh = new Mesh();
        previousMesh = new Mesh();
        lbiFilter = lootboxItem.GetComponent<MeshFilter>();
        lbiFilter.mesh = null;
    }

    public void OpenLootBox()
    {
        if (!hasOpenedBox)
        {
            StartCoroutine(LootboxAnimation());
            Money.scriptInstance.LoseMoney(cost);
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
        yield return new WaitForSeconds(1f);

        Player playerScript = GameObject.Find("Player").GetComponent<Player>();

        while (!hasStoppedBox)
        {
            previousMesh = currentMesh;
            currentMesh = meshes[UnityEngine.Random.Range(0, meshes.Length)];

            if (currentMesh == previousMesh) { yield return null; }

            lbiFilter.mesh = currentMesh;
            yield return new WaitForSeconds(0.3f);
        }

        int randomNumber = UnityEngine.Random.Range(0, meshes.Length);
        lbiFilter.mesh = meshes[randomNumber];
        
        string name = lbiFilter.GetComponent<MeshFilter>().mesh.name;
        name.Remove(name.Length - 9);
        Debug.Log(name);

        playerScript.SwitchLoadout(playerScript.currentEquippedItem, randomNumber);
        StartCoroutine(ResetBox());
        yield break;
    }

    private IEnumerator ResetBox()
    {
        gameObject.GetComponent<Animator>().SetBool("Open", false);
        yield return new WaitForSeconds(5f);
        Instantiate(gameObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
