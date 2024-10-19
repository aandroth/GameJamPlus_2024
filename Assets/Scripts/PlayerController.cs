using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_moveSpeed = 10f;
    public float m_walkAcceleration = 10f;
    public float m_airAcceleration = 5f;
    public float m_groundDeceleration = 10f;
    public float m_jumpHeight;
    public float m_gravityForce = -25f;
    public bool m_grounded = false;
    public bool m_controlsFrozen = false;
    private Rigidbody rb = null;
    private BoxCollider2D m_boxCollider = null;
    private Vector2 m_velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_controlsFrozen)
        {
            float acc = m_grounded ? m_walkAcceleration : m_airAcceleration;
            float dec = m_grounded ? m_groundDeceleration : 0;

            if (Input.GetKey(KeyCode.D))
            {
                // Move right
                // Turn char right
                m_velocity.x = Mathf.MoveTowards(m_velocity.x, m_moveSpeed, acc * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                // Move left
                m_velocity.x = Mathf.MoveTowards(m_velocity.x, -m_moveSpeed, acc * Time.deltaTime);
            }
            else
            {
                // Stop movement
                m_velocity.x = Mathf.MoveTowards(m_velocity.x, 0, dec * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W))
            {
                // Climb up
            }
            if (Input.GetKey(KeyCode.S))
            {
                // Climb down
            }
            if (m_grounded)
            {
                m_velocity.y = 0;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Jump
                    m_velocity.y = Mathf.Sqrt(2 * m_jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    m_grounded = false;
                }
            }
        }
        if(!m_grounded)
        {
            m_velocity.y += m_gravityForce * Time.deltaTime;
        }


        transform.Translate(m_velocity * Time.deltaTime);


        // Detect collisions
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, m_boxCollider.size, 0);

        if (hits.Length == 1)
            m_grounded = false;

        foreach (Collider2D hit in hits)
        {
            if (m_boxCollider == hit) continue;


            ColliderDistance2D colliderDistance = hit.Distance(m_boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }

            if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && m_velocity.y < 0)
            {
                m_grounded = true;
            }
        }
    }
}