using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject plane;
    public Image menu;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Disable the button
        //button.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);

        // Disable the parent RawImage
        plane.gameObject.SetActive(true);

    }
}
