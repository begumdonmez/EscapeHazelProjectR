using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonLoadScene : MonoBehaviour
{
    private void Start()
    {
        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject buttonObject in allButtons)
        {
            Button button = buttonObject.GetComponent<Button>();
            string buttonName = buttonObject.gameObject.name;
            Debug.Log("Button Name : " + buttonName);
            button.onClick.AddListener(() => LoadSceenOnclick(buttonName));
        }
    }


    private void LoadSceenOnclick(string obj)
    {
        SceneManager.LoadScene(obj);
    }
}
