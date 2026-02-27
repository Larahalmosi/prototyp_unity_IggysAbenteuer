using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IchthyosaurSelection
{
    public static int SelectedId = 0;  
}

public class IchthyosaurMenu : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    
    private void Start()
    {
        var root = uiDocument.rootVisualElement;
        
        // Finde deine 2 Ichthyosaurier-Images per Name (aus UXML)
        var ichthy1Button = root.Q<Button>("Iggy1");  // Name im UXML
        ichthy1Button.clicked += SelectFirst;

        var ichthy2Button = root.Q<Button>("Iggy2");
        ichthy2Button.clicked += SelectSecond;
    }
    
    public void SelectFirst()
    {
        IchthyosaurSelection.SelectedId = 1;
        SceneManager.LoadScene("PlayGame");
    }
    
    public void SelectSecond()
    {
        IchthyosaurSelection.SelectedId = 2;
        SceneManager.LoadScene("PlayGame");
    }
}
