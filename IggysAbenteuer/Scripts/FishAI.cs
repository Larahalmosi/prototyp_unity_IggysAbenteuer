using UnityEngine;

public class FishAI : MonoBehaviour
{
    public FishSpawner spawner;
    private Vector2 moveDirection = Vector2.right;
    private float changeDirectionTimer = 0f;
    private SpriteRenderer spriteRenderer;
       void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
         if (spawner == null) return;
        
        transform.Translate(moveDirection * Time.deltaTime * spawner.GetMoveSpeed(), Space.World);
        
        Vector2 pos = transform.position;
        Vector2 boundsMin = spawner.GetSwimBoundsMin();
        Vector2 boundsMax = spawner.GetSwimBoundsMax();
        
        // Grenzen + Flip!
        bool flipped = false;
        
        if (pos.x < boundsMin.x || pos.x > boundsMax.x)
        {
            moveDirection.x *= -1;
            flipped = false;
        }
        
        if (pos.y < boundsMin.y || pos.y > boundsMax.y)
        {
            moveDirection.y *= -1;
        }
        
        // SPRITE FLIP bei Links-Bewegung
        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = false;  // Nach LINKS schauen
        }
        else
        {
            spriteRenderer.flipX = true; // Nach RECHTS schauen
        }
        
        // Sanfter Richtungswechsel
        changeDirectionTimer += Time.deltaTime;
        if (changeDirectionTimer >= spawner.GetChangeDirectionInterval())
        {
            moveDirection = new Vector2(
                Random.Range(-1f, 1f), 
                Random.Range(-0.2f, 0.2f)
            ).normalized;
            changeDirectionTimer = 0f;
        }
    }
    
    public void SetSpawner(FishSpawner s) => spawner = s;
}