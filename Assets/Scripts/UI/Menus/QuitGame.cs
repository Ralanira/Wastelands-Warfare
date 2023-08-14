using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Menus
{
    public class QuitGame : MonoBehaviour
    {
        public void exitGame() 
        {  
            Debug.Log("exitgame");  
            Application.Quit();  
        }  
    }
}