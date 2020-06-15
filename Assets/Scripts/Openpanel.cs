using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openpanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Panel;
    public GameObject CloPanel;

    public void OpenPanel()
    {
        if(Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    public void ClosePanel()
    {
        CloPanel.SetActive(false);
    }
}
