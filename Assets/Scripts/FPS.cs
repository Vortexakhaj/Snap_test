// FPS Controller
// 1. Create a Parent Object like a 3D model
// 2. Make the Camera the user is going to use as a child and move it to the height you wish. 
// 3. Attach a Rigidbody to the parent
// 4. Drag the Camera into the m_Camera public variable slot in the inspector
// Escape Key: Escapes the mouse lock
// Mouse click after pressing escape will lock the mouse again


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPS : MonoBehaviour
{

    public MatchandSnap _matchScript;

    public float speed = 5.0f;
    public float m_MovX;
    public float m_MovY;
    public Vector3 m_moveHorizontal;
    public Vector3 m_movVertical;
    public Vector3 m_velocity;
    public Rigidbody m_Rigid;
    public float m_yRot;
    public float m_xRot;
    public float xClamp;
    public float yClamp;
    public Vector3 m_rotation;
    public Vector3 m_cameraRotation;
    public float m_lookSensitivity = 3.0f;
    public bool m_cursorIsLocked = true;

    [Header("The Camera the player looks through")]
    public Camera m_Camera;

    // Use this for initialization
    public void Start()
    {
        m_Rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            _matchScript.dragging = true;
        }
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            _matchScript.dragging = false;
        }

        
            m_MovX = Input.GetAxis("Horizontal");
            m_MovY = Input.GetAxis("Vertical");
        

        m_moveHorizontal = transform.right * m_MovX;
        m_movVertical = transform.forward * m_MovY;

        m_velocity = (m_moveHorizontal + m_movVertical).normalized * speed;

        //mouse movement 
        m_yRot = Input.GetAxisRaw("Mouse X");
        m_rotation = new Vector3(0, m_yRot, 0) * m_lookSensitivity * Time.deltaTime *speed;

        m_xRot = Input.GetAxisRaw("Mouse Y");
        m_cameraRotation = new Vector3(m_xRot, 0, 0) * m_lookSensitivity * Time.deltaTime * speed;

        //clamp
        xClamp += m_rotation.y;
        yClamp += m_cameraRotation.x;

        if(xClamp > 45.0)
        {
            xClamp = 45.0f;
            m_rotation = Vector3.zero;
        }
        else if(xClamp < -45.0f)
        {
            xClamp = -45.0f;
            m_rotation = Vector3.zero;
        }
        
        if(yClamp > 35.0)
        {
            xClamp = 35.0f;
            m_cameraRotation = Vector3.zero;
        }
        else if(yClamp < -10.0f)
        {
            yClamp = -10.0f;
            m_cameraRotation = Vector3.zero;
        }


        //apply camera rotation

        //move the actual player here
        if (m_velocity != Vector3.zero)
        {
            m_Rigid.MovePosition(m_Rigid.position + m_velocity * Time.fixedDeltaTime);
        }

        if (m_rotation != Vector3.zero)
        {
            //rotate the camera of the player
            m_Rigid.MoveRotation(m_Rigid.rotation * Quaternion.Euler(m_rotation));
        }

        if (m_Camera != null)
        {
            //negate this value so it rotates like a FPS not like a plane
            m_Camera.transform.Rotate(-m_cameraRotation);
        }

        InternalLockUpdate();

    }

    //controls the locking and unlocking of the mouse
    public void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            UnlockCursor();
        }
        else if (!m_cursorIsLocked)
        {
            LockCursor();
        }
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
