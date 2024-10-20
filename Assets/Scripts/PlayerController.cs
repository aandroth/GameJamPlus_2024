using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public BoxCollider2D m_debugBox;
    public Animator m_modelAnims;
    public float m_moveSpeed = 10f;
    public float m_walkAcceleration = 10f;
    public float m_airAcceleration = 5f;
    public float m_groundDeceleration = 10f;
    public float m_jumpHeight;
    public float m_gravityForce = -25f;
    public float m_frictionPower = -0.5f;
    public bool m_grounded = false;
    public bool m_controlsFrozen = false;
    public bool m_goalReached = false;
    private Rigidbody2D rb = null;
    private BoxCollider2D m_boxCollider = null;
    private Vector2 m_velocity;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        string animToPlay = "NoChange";
        if (!m_controlsFrozen)
        {
            float acc = m_grounded ? m_walkAcceleration : m_airAcceleration;
            float dec = m_grounded ? m_groundDeceleration : 0;

            if (Input.GetKey(KeyCode.D))
            {
                // Move right
                // Turn char right
                m_velocity.x = acc * Time.deltaTime;//Mathf.MoveTowards(m_velocity.x, m_moveSpeed, acc * Time.deltaTime);
                if (transform.localScale.x < 0) transform.localScale = new Vector3(1, 1, 1);
                if (!m_modelAnims.GetCurrentAnimatorStateInfo(0).IsName("Walk")) animToPlay = "Walk";
            }
            else if (Input.GetKey(KeyCode.A))
            {
                // Move left
                m_velocity.x = -acc * Time.deltaTime; //Mathf.MoveTowards(m_velocity.x, -m_moveSpeed, acc * Time.deltaTime);
                if (transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                if (!m_modelAnims.GetCurrentAnimatorStateInfo(0).IsName("Walk")) animToPlay = "Walk";
            }
            else
            {
                // Stop movement
                //m_velocity.x = Mathf.MoveTowards(m_velocity.x, 0, dec * Time.deltaTime);
                if (!m_modelAnims.GetCurrentAnimatorStateInfo(0).IsName("Idle")) animToPlay = "Idle";
            }
            if (m_grounded)
            {
                m_velocity.x = -m_velocity.x * m_frictionPower;
                m_velocity.y = 0;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Jump
                    //m_velocity.y = m_jumpHeight; //Mathf.Sqrt(2 * m_jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    m_grounded = false; 
                    rb.AddForce(new Vector2(0, m_jumpHeight));
                }
            }
        }
        if (!m_grounded)
        {
            m_velocity.y += m_gravityForce * Time.deltaTime;
            if (!m_modelAnims.GetCurrentAnimatorStateInfo(0).IsName("Jump")) animToPlay = "Jump";
        }

        // Check for ground collisions
        float velY = m_velocity.y * Time.deltaTime;

        Vector3 p = new Vector3(transform.position.x, (transform.position.y - m_boxCollider.bounds.extents.y + velY * 0.5f), transform.position.z);
        Vector2 s = new Vector2(m_boxCollider.size.x, Mathf.Abs(velY));
        m_debugBox.transform.position = p;
        m_debugBox.size = s;

        //Collider2D[] hits = Physics2D.OverlapBoxAll(m_debugBox.transform.position, m_debugBox.size, 0);
        //if (velY < 0)
        //{
        //    foreach (Collider2D hit in hits)
        //    {
        //        if (m_boxCollider == hit || m_debugBox == hit) continue;

        //        ColliderDistance2D colliderDistance = hit.Distance(m_boxCollider);

        //        if (colliderDistance.isOverlapped)
        //        {
        //            //velY += colliderDistance.pointA.y - colliderDistance.pointB.y;
        //            velY = velY < 0 ? colliderDistance.pointB.y - colliderDistance.pointA.y : colliderDistance.pointA.y - colliderDistance.pointB.y;
        //            m_grounded = true;
        //        }
        //    }
        //}

        //Debug.DrawLine(new Vector3(transform.position.x, (transform.position.y - m_boxCollider.bounds.extents.y), transform.position.z),
        //               new Vector3(transform.position.x, (transform.position.y - m_boxCollider.bounds.extents.y + velY), transform.position.z),
        //               Color.yellow, Time.deltaTime*0.5f);

        //transform.Translate(new Vector2(m_velocity.x * Time.deltaTime, velY));
        rb.AddForce(m_velocity);

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
                m_velocity.y = 0;
            }
        }

        if (animToPlay != "NoChange") m_modelAnims.Play(animToPlay);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ClockwiseTurn")
        {
            collision.GetComponent<TurningPoint>().TurnParentBuilding();
            m_velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else if (collision.tag == "Goal" && !m_goalReached)
        {
            m_velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            m_goalReached = true;
            m_modelAnims.gameObject.transform.eulerAngles = Vector3.zero;
            m_controlsFrozen = true;
            m_modelAnims.Play("Spray Paint");
            collision.GetComponent<LevelWin>().LevelWinStart();
        }
    }

    public void FinishBuildingTurn()
    {
        rb.gravityScale = 1;
    }
}