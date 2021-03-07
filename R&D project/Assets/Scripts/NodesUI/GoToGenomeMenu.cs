using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToGenomeMenu : MonoBehaviour
{
    [SerializeField]
    private RawImage blackScreen;

    [SerializeField]
    private FlyCamera cam;

    [SerializeField]
    private ClientsMenu dropdownClients;

    private float currentTimeScale = 0;

    private bool changedScreen, firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ActivateMenu);
    }

    private void ActivateMenu()
    {
        if (!blackScreen.enabled)
        {
            blackScreen.enabled = true;
            dropdownClients.gameObject.SetActive(true);
            dropdownClients.changeActiveState(true);

            currentTimeScale = Time.timeScale;
            Time.timeScale = 0;

            cam.hover = true;
            changedScreen = true;

            if (firstTime)
            {
                firstTime = false;
                dropdownClients.FillDropDown();
            }
        }
        else
        {
            blackScreen.enabled = false;
            dropdownClients.changeActiveState(false);
            dropdownClients.gameObject.SetActive(false);

            Time.timeScale = currentTimeScale;

            changedScreen = false;
        }
    }

    public void OnPointerEnter()
    {
        cam.hover = true;
    }

    public void OnPointerExit()
    {
        if (!changedScreen)
        {
            cam.hover = false;
        }
    }
}
