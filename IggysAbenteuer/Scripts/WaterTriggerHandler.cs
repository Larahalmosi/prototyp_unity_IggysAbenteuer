//source: https://www.youtube.com/watch?v=TbGEKpdsmCI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTriggerHandler : MonoBehaviour
{
   [SerializeField] private LayerMask _waterMask;
    [SerializeField] private GameObject _splashParticles;
private EdgeCollider2D _edgeColl;
private InteractableWater _water;

    private void Awake()
    {
        _edgeColl = GetComponent<EdgeCollider2D>();
        _water = GetComponent<InteractableWater>();
          _edgeColl.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Water trigger hit by: " + collision.name);
       // Nur reagieren, wenn das Objekt im gewünschten Layer ist
        if ((_waterMask.value & (1 << collision.gameObject.layer)) == 0)
            return;

        // Rigidbody vom Objekt holen, das ins Wasser fällt (z.B. dein Player)
        Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
        if (rb == null) return;

        // Splash-Position
        Vector2 localPos       = transform.localPosition;
        Vector2 hitObjectPos   = collision.transform.position;
        Bounds hitObjectBounds = collision.bounds;

        Vector3 spawnPos;
        if (collision.transform.position.y >= _edgeColl.points[1].y + _edgeColl.offset.y + localPos.y)
        {
            // Hit von oben
            spawnPos = hitObjectPos - new Vector2(0f, hitObjectBounds.extents.y);
        }
        else
        {
            // Hit von unten
            spawnPos = hitObjectPos + new Vector2(0f, hitObjectBounds.extents.y);
        }

        Instantiate(_splashParticles, spawnPos, Quaternion.identity);

        // Splash-Kraft
        int multiplier = rb.linearVelocity.y < 0 ? -1 : 1;
        float vel = rb.linearVelocity.y * _water.ForceMultiplier;
        vel = Mathf.Clamp(Mathf.Abs(vel), 0f, _water.MaxForce);
        vel *= multiplier;

        _water.Splash(collision, vel);
    }
}