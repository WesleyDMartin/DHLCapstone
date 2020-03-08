using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterControl : MonoBehaviour
{
    private readonly float m_backwardRunScale = 0.66f;
    private readonly float m_backwardsWalkScale = 0.16f;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    [SerializeField] private Animator m_animator;
    private readonly List<Collider> m_collisions = new List<Collider>();

    [SerializeField] private readonly ControlMode m_controlMode = ControlMode.Direct;
    private Vector3 m_currentDirection = Vector3.zero;
    private float m_currentH;

    private float m_currentV;

    private bool m_isGrounded;
    [SerializeField] private readonly float m_jumpForce = 4;

    private float m_jumpTimeStamp;
    private readonly float m_minJumpInterval = 0.25f;

    [SerializeField] private readonly float m_moveSpeed = 2;
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private readonly float m_turnSpeed = 200;

    private bool m_wasGrounded;

    private void OnCollisionEnter(Collision collision)
    {
        var contactPoints = collision.contacts;
        for (var i = 0; i < contactPoints.Length; i++)
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider)) m_collisions.Add(collision.collider);
                m_isGrounded = true;
            }
    }

    private void OnCollisionStay(Collision collision)
    {
        var contactPoints = collision.contacts;
        var validSurfaceNormal = false;
        for (var i = 0; i < contactPoints.Length; i++)
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true;
                break;
            }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider)) m_collisions.Add(collision.collider);
        }
        else
        {
            if (m_collisions.Contains(collision.collider)) m_collisions.Remove(collision.collider);
            if (m_collisions.Count == 0) m_isGrounded = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider)) m_collisions.Remove(collision.collider);
        if (m_collisions.Count == 0) m_isGrounded = false;
    }

    private void Update()
    {
        m_animator.SetBool("Grounded", m_isGrounded);

        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate();
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }

        m_wasGrounded = m_isGrounded;
    }

    private void TankUpdate()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");

        var walk = Input.GetKey(KeyCode.LeftShift);

        if (v < 0)
        {
            if (walk)
                v *= m_backwardsWalkScale;
            else
                v *= m_backwardRunScale;
        }
        else if (walk)
        {
            v *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }

    private void DirectUpdate()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");

        var camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        var direction = camera.forward * m_currentV + camera.right * m_currentH;

        var directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        var jumpCooldownOver = Time.time - m_jumpTimeStamp >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded) m_animator.SetTrigger("Land");

        if (!m_isGrounded && m_wasGrounded) m_animator.SetTrigger("Jump");
    }

    private enum ControlMode
    {
        Tank,
        Direct
    }
}