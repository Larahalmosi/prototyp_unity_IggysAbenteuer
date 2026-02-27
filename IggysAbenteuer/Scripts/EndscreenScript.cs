using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EndscreenScript : MonoBehaviour
{    [SerializeField] private UIDocument uiDocument;
    private Label finalAgeLabel;
    private Label eatingScore;
    Rigidbody2D rb;
    private Button backToStartButton;
    public Sprite ichthyosaur1Sprite;
    public Sprite ichthyosaur2Sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {     //Debug.Log(GameStats.FinalAge);
        rb = GetComponent<Rigidbody2D>(); 
     SpriteRenderer sr = GetComponent<SpriteRenderer>();

         if (IchthyosaurSelection.SelectedId == 1)
        {
            sr.sprite = ichthyosaur1Sprite;
        }
        else if (IchthyosaurSelection.SelectedId == 2)
        {
            sr.sprite = ichthyosaur2Sprite;}
        
             var root = uiDocument.rootVisualElement;
             // Finales Alter anzeigen
        finalAgeLabel = root.Q<Label>("FinalAgeLabel");   // oder richtiger Name aus deinem UXML
        if (finalAgeLabel != null)
            finalAgeLabel.text = $"Iggy wurde <color=#FFE395> {GameStats.FinalAge}  Jahre </color> alt.";
eatingScore = root.Q<Label>("EatingScore");
 if (eatingScore != null)
    {
        eatingScore.text = $"Fische gefressen: <color=#4ECDC4>{PlayerController.totalFishEaten}</color>";
    }
        // Button holen
        backToStartButton = root.Q<Button>("BackToStartButton");
        if (backToStartButton != null)
        { 
            backToStartButton.RegisterCallback<ClickEvent>(OnBackToStartClicked);  
        }

        }
    
    private void OnBackToStartClicked(ClickEvent evt)
    {
        
        SceneManager.LoadScene("StartScene");
    }
}