using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class BookCode : MonoBehaviour
{

    [SerializeField] private Button button;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text messageText; 

    private Dictionary<string, string> bookPlaces = new Dictionary<string, string>()
    {
        {"1234", "4. blok 2. raf"},
        {"1999","2. blok 3. raf"}
    };

    private void Start()
    {
        button.onClick.AddListener(GiveBookPlace);
    }

    private void GiveBookPlace()
    {
        if (bookPlaces.TryGetValue(userInput.text, out string place))
        {
            messageText.text = $"Kitap {place} da bulunmakta";
            messageText.color = Color.green;
        }
        else
        {
            messageText.text = "Böyle bir kitap yok";
            messageText.color = Color.red;
        }
    }
}