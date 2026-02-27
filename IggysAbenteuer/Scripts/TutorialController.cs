//source: Perplexity
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour

{   public UIDocument tutorialDocument;
    public PlayerController player; 

  
    void Start()
    {
         // Spiel pausieren
        Time.timeScale = 0f;
         // Button aus dem Tutorial-UXML holen
        var root = tutorialDocument.rootVisualElement;
        var startButton = root.Q<Button>("WeiterButton"); // Name wie im UXML

        startButton.clicked += OnStartClicked;
      
    }

   void OnStartClicked()
    {
        // Tutorial verstecken
        tutorialDocument.rootVisualElement.style.display = DisplayStyle.None;

        // Ingame-HUD vom Player sichtbar machen
       player.ShowHud();

        // Spiel starten
        Time.timeScale = 1f;
    }
}
