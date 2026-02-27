//source: Perplexity, 
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public static class GameStats
{
    public static float FinalAge;
}
public class PlayerController : MonoBehaviour
{
 
public float age = 0f;
public float scoreIncreaseInterval = 2f;  // Alle 10 Sekunden +1 Alter
private float nextIncreaseTime = 0f;


   public float moveSpeed = 100f;          //Bewegungsgeschwindigkeit
 public float slowDownRadius = 0.5f;  // ab dieser Distanz langsam bremsen
    private Vector2 moveDirection;
    private bool hasTarget = false; 
    private Vector2 targetPosition;

    Rigidbody2D rb;
 [Header("Statistiken")]
    public static int totalFishEaten = 0; 

 [Header("IngameUI")]
    public UIDocument uiDocument;
    private VisualElement placement;
    private Label scoreText;
    private Label foodText;
    private Image foodContainer1;
     private Image foodContainer2;
    private Image foodContainer3;
    private VisualElement infoUI;
    private Image imgheart1,imgheart2,imgheart3;
    float[] heartStates = {0.2f,1.0f,};
public int maxLives = 3;
public int currentLives = 3;

    [Header("Wachstum")]
    public Sprite ichthyosaur1Sprite;
    public Sprite ichthyosaur2Sprite;
     public Sprite ichthyosauradult1Sprite;
      public Sprite ichthyosauradult2Sprite;
      private bool isAdult = false;
public float startScale = 1f; // Start-Größe (1 = normal)
public float growthRate = 0.2f;   // Wachstum pro Alterspunkt
public float maxScale = 50f;     // Maximale Größe
    
[Header("Animation")]

public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
      rb = GetComponent<Rigidbody2D>(); 
      rb.constraints = RigidbodyConstraints2D.FreezeRotation;
      var root = uiDocument.rootVisualElement;
     

    placement    = root.Q<VisualElement>("Placement");
    scoreText    = root.Q<Label>("ScoreLabel");
    foodText     = root.Q<Label>("FoodLabel");
    foodContainer1= root.Q<Image>("FoodContainer1");
     foodContainer2= root.Q<Image>("FoodContainer2");
      foodContainer3= root.Q<Image>("FoodContainer3");
    infoUI=root.Q<VisualElement>("Info");
    imgheart1= root.Q<Image>("heart1");
    imgheart2= root.Q<Image>("heart2");
    imgheart3= root.Q<Image>("heart3");

    // Ingame-HUD am Anfang ausblenden
    placement.style.display     = DisplayStyle.None;
    scoreText.style.display     = DisplayStyle.None;
    foodText.style.display      = DisplayStyle.None;
    foodContainer1.style.display = DisplayStyle.None;
    foodContainer2.style.display = DisplayStyle.None;
    foodContainer3.style.display = DisplayStyle.None;
    infoUI.style.display     = DisplayStyle.None;
    imgheart1.style.display     = DisplayStyle.None;
    imgheart2.style.display     = DisplayStyle.None;
    imgheart3.style.display     = DisplayStyle.None;

      SpriteRenderer sr = GetComponent<SpriteRenderer>();


        if (IchthyosaurSelection.SelectedId == 1)
        {
            sr.sprite = ichthyosaur1Sprite;
           // Debug.Log("Ichthyosaurus 1 ausgewählt!");
        }
        else if (IchthyosaurSelection.SelectedId == 2)
        {
            sr.sprite = ichthyosaur2Sprite;
           // Debug.Log("Ichthyosaurus 2 ausgewählt!");
        }

    transform.localScale = Vector3.one * startScale;
     targetPosition = transform.position;

    animator = GetComponent<Animator>();
    animator.SetInteger("colorVariant", IchthyosaurSelection.SelectedId); 
    animator.SetBool ("isAdult", false);
    animator.SetBool ("isEating", false);
    animator.SetBool ("isSwimming", true);
    }

void OnEnable()
{
    EnhancedTouchSupport.Enable();
}

void OnDisable()
{
    EnhancedTouchSupport.Disable();
}

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return;  // Kein Movement/Score, solange Tutorial läuft

       
         // Alters- und Wachstumslogik
    if (Time.time >= nextIncreaseTime)
    {
        age++;
        nextIncreaseTime = Time.time + scoreIncreaseInterval;
        scoreText.text = "Iggys Alter: " + age;
        CheckAdultTransformation();

        float scaleMultiplier = startScale + (age * growthRate);
        scaleMultiplier = Mathf.Clamp(scaleMultiplier, startScale, maxScale);
        transform.localScale = Vector3.one * scaleMultiplier;
    }

    // 1) Maus-Klick wie bisher
    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
    {
        SetTargetFromScreenPosition(Mouse.current.position.value);
    }

    // 2) Zusätzlich: Primären Touch als Klick behandeln
    var ts = Touch.activeTouches;
    foreach (var t in ts)
    {
        if (t.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            SetTargetFromScreenPosition(t.screenPosition);
            break; // nur erster Finger
        }
    }

    // Physik-Bewegung wie bei dir
    if (hasTarget)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance < slowDownRadius)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.05f);
            if (distance < 0.05f && rb.linearVelocity.magnitude < 0.05f)
            {
                rb.linearVelocity = Vector2.zero;
                hasTarget = false;
            }
        }
        else
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
    else
    {
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.05f);
    }
}

// Hilfsfunktion für Maus + Touch
void SetTargetFromScreenPosition(Vector2 screenPos)
{
    Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
    targetPosition = new Vector2(world.x, world.y);

    Vector2 dir = (targetPosition - (Vector2)transform.position);
    if (dir.sqrMagnitude > 0.0001f)
    {
        moveDirection = dir.normalized;
        hasTarget = true;
    }

    
}
    
    


 //kontrolliert Erwachsenen-Phase
    void CheckAdultTransformation()
{
    if (age >= 18 && !isAdult)
    {
        isAdult = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        animator.SetBool("isAdult",true);
        // FoodContainer3 einblenden (Tintenfisch)
        foodContainer3.style.display = DisplayStyle.Flex;
        foodContainer1.style.display = DisplayStyle.None;  
        foodContainer2.style.display = DisplayStyle.None;
            
        //Debug.Log("Iggy ist erwachsen geworden!");
    }

        if(age>=30) 
        {
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        GameStats.FinalAge = age;
        Destroy(gameObject);
        SceneManager.LoadScene("EndScreen");
        return;
        }
}   
public void ShowHud()
{
    placement.style.display      = DisplayStyle.Flex;
   scoreText.style.display      = DisplayStyle.Flex;
    foodText.style.display       = DisplayStyle.Flex;
    foodContainer1.style.display  = DisplayStyle.Flex;
    foodContainer2.style.display  = DisplayStyle.Flex;
    infoUI.style.display      = DisplayStyle.Flex;
   imgheart1.style.display  = DisplayStyle.Flex;
    imgheart2.style.display  = DisplayStyle.Flex;
    imgheart3.style.display  = DisplayStyle.Flex;

     foodContainer3.style.display = DisplayStyle.None;
}
void UpdateHeartsUI()
{
    imgheart1.style.opacity = heartStates[1];  
    imgheart2.style.opacity = heartStates[currentLives >= 2 ? 1 : 0];
     imgheart3.style.opacity = heartStates[currentLives >= 3 ? 1 : 0];
}

void OnTriggerEnter2D(Collider2D other)
{
 
         if (!isAdult)
    {
        if (other.CompareTag("Dapedium") || other.CompareTag("Pholidophorus"))
        {
            age += 1;
             totalFishEaten++;
            scoreText.text = "Iggys Alter: " + age;
            animator.SetBool("isEating",true);
            Invoke(nameof(StopEating), 0.5f);
            Destroy(other.gameObject);
              return;

        }
        
        else if (other.CompareTag("Hypodus")|| other.CompareTag("Gas"))
        {
            TakeDamage();  // deine Damage Funktion
        }
    }
    // ERWACHSEN: Nur Tintenfische, Temnodontosaurus Feind
    else
    {
        if (other.CompareTag("Tintenfisch"))
        {
            age += 1;
             totalFishEaten++;
            scoreText.text = "Iggys Alter: " + age;
             animator.SetBool("isEating",true);
             Invoke(nameof(StopEating), 0.2f);
            Destroy(other.gameObject);
            return;
        }
        
        else if (other.CompareTag("Temnodontosaurus")||other.CompareTag("Gas"))
        {
            TakeDamage();
        }
      
    }

    if (other.CompareTag ("Ending"))  
{
   
 // Ingame-UI ausblenden
        placement.style.display = DisplayStyle.None;
        scoreText.style.display = DisplayStyle.None;
        foodText.style.display = DisplayStyle.None;
        foodContainer1.style.display = DisplayStyle.None;
         foodContainer2.style.display = DisplayStyle.None;
          foodContainer3.style.display = DisplayStyle.None;
        infoUI.style.display = DisplayStyle.None;
        imgheart1.style.display = DisplayStyle.None;
        imgheart2.style.display = DisplayStyle.None;
        imgheart3.style.display = DisplayStyle.None;

// Alter merken, bevor der Player zerstört wird
  GameStats.FinalAge = age;
  Destroy(gameObject);
 SceneManager.LoadScene("EndScreen");
} 

}

void StopEating()
{
    animator.SetBool("isEating", false);
}
   
  void TakeDamage()
    {
        currentLives--;
        UpdateHeartsUI();
        
        if (currentLives <= 0)
        {
              GameStats.FinalAge = age;
            // Tod: Endscreen
            Destroy(gameObject);
            SceneManager.LoadScene("EndScreen");
        }
    }

 


void OnCollisionEnter2D(Collision2D collision)
{

}
}



