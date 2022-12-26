using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public CharacterController characterController;
    public Transform cam;
    public bool IsPause = false;
    public BarScript healthBar;
    public BarScript shieldBar;
    
    
    
    public int currentHealth;
    public int maxHealth = 100;
    public int coinNumber = 0;

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
        InitializeHealthStatus(); // initialize player's health

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; // 鼠標消失
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetBool("run", false);
        Movement();
        PauseAndResumeTheGame();
        PlayerAliveCheck();
    }

    private void Movement()//��k
    {
        // 用角度進行方向控制
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (gameObject.GetComponent<WeaponChange>().now_is_sword)
        {
            if (direction.magnitude >= 0.1f)
            {
                myAnimator.SetBool("run", true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f); // when using sword

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * speed * Time.deltaTime);

                myAnimator.SetBool("run", true);
            }
        } else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z); // when using gun
            if (direction.magnitude >= 0.1f)
            {
                myAnimator.SetBool("run", true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * speed * Time.deltaTime);

                myAnimator.SetBool("run", true);
            }
        }
    }

    // pause the game using esc key
    private void PauseAndResumeTheGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPause = !IsPause;
            Cursor.visible = (IsPause) ? true : false;
            Cursor.lockState = (IsPause) ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = (IsPause) ? 0 : 1;
            // todo: show menu when pausing the game
        }
    }

    private void InitializeHealthStatus()
    {
        healthBar.GetComponent<BarScript>().SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.GetComponent<BarScript>().SetHealth(currentHealth);
    }

    public void PlayerAliveCheck()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}