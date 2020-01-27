using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInventoryPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitToClose());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        
    }

    public void OnQuickInventoryButtonClicked()
    {
        Debug.Log("clicked quick inventory panel");
        GetComponent<Animator>().SetBool("IsOpen", true);
        StopAllCoroutines();
        StartCoroutine(waitToClose());
    }

    private IEnumerator waitToClose()
    {
        Debug.Log("waiting to close quick inventory panel");
        yield return new WaitForSeconds(10.0f);
        GetComponent<Animator>().SetBool("IsOpen", false);
        yield return new WaitForSeconds(0.5f);
    }
}
