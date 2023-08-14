using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.InGame
{
    public class MessageString : MonoBehaviour
    {
        public static MessageString instance;
        private TextMeshProUGUI textField;
        [SerializeField] AudioSource warningSound;
        
        private void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;

            textField = GetComponent<TextMeshProUGUI>();
        }

        public void PrintMessage(string text)
        {
            warningSound.Play();
            StopAllCoroutines();
            StartCoroutine(showMessage(text));
        }
        
        IEnumerator showMessage(string message)
        {
            textField.enabled = true;
            textField.text = message;
            yield return new WaitForSecondsRealtime(3);
            textField.enabled = false;
        }
    }
}

