using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
     public  TMP_InputField passwordInputField; // Şifre girişi için InputField referansı
        public string correctPassword = "12345";
        public TMP_Text messageText;
    
        public GameObject computerScreenUI; // Bilgisayar ekranı UI'sı
        public GameObject otherUI; // Diğer UI
    
        public void CheckPassword()
        {
            string inputPassword = passwordInputField.text;
    
            if (inputPassword == correctPassword)
            {
                // Doğru şifre girildiğinde diğer UI'ye geçiş yap
                messageText.text = "Şifre doğru!";
                messageText.color = Color.green;
               
                computerScreenUI.SetActive(false);
                otherUI.SetActive(true);
            }
            else
            {
               
                messageText.text = "Şifre yanlış!";
                messageText.color = Color.red;
                // Hatalı şifre mesajı veya işlemlerini buraya ekleyebilirsiniz
                Debug.Log("Hatalı şifre!");
    
                otherUI.SetActive(false);
            }
        }
        
       
}
