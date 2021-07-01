using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour, Damageable
{
    private float mouseSensivity;
    [SerializeField]
    private Transform myCamera;
    [SerializeField]
    private float speed;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private CharacterController controller;
    [SerializeField]
    private float interactionDistance;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilCheck;
    [SerializeField]
    private LayerMask groundMask;
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight;
    private Vector3 velocity;
    private float maxHealth = 100f;
    private float health = 100f;
    [SerializeField]
    private Image healthBar;
    private bool isDead = false;
    [SerializeField]
    private CanvasGroup HUD;
    [SerializeField]
    private CanvasGroup DeathCanvas;
    [SerializeField]
    private CanvasGroup WinCanvas;
    [SerializeField]
    private CanvasGroup PauseMenu;
    [SerializeField]
    private GameObject InterractionOverlay;
    [SerializeField]
    private GameObject Tooltip;
    [SerializeField]
    private Text TooltipText;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip walkSound1;
    [SerializeField]
    private AudioClip walkSound2;
    [SerializeField]
    private AudioClip walkSound3;
    [SerializeField]
    private AudioClip walkSound4;
    [SerializeField]
    private AudioClip walkSound5;

    private float timeElapsed = 0f;
    private float lerpDuration = 2f;
    private bool isPaused = false;
    private bool isWon = false;
    private bool isWalking = false;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WalkCoroutine());
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
        mouseSensivity = PlayerPrefs.GetFloat("Sensivity", 200f);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isDead && !isWon)
        {
            timeElapsed = 0f;
            if (!isPaused)
            {
                // update camera
                float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
                rotationY += mouseX;
                rotationX -= mouseY;
                rotationX = Mathf.Clamp(rotationX, -85f, 90f);
                if (rotationY < 30 && rotationY > -30)
                {
                    myCamera.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
                }
                else
                {
                    transform.Rotate(Vector3.up * mouseX);
                    rotationY = Mathf.Clamp(rotationY, -30f, 30f);
                    myCamera.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
                }

                // gravity
                bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
                bool isCeiled = Physics.CheckSphere(ceilCheck.position, 0.2f, groundMask);

                velocity.y += gravity * Time.deltaTime;

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = gravity / 2;
                }
                if (isCeiled && velocity.y > 0)
                {
                    velocity.y = Mathf.Max(-velocity.y, gravity);
                }

                // update position
                float axisZ = Input.GetAxis("Horizontal");
                float axisX = Input.GetAxis("Vertical");
                if (axisX != 0 || axisZ != 0)
                {
                    // lock camera while walking
                    myCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
                    transform.Rotate(0f, rotationY, 0f);
                    rotationY = 0;

                    Vector3 direction = transform.forward * axisX + transform.right * axisZ;
                    direction = direction.normalized;
                    controller.Move(direction * speed * Time.deltaTime);
                    if (isGrounded)
                    {
                        isWalking = true;
                    }
                    else
                    {
                        isWalking = false;
                        audioSource.Stop();
                    }
                }
                else
                {
                    isWalking = false;
                    audioSource.Stop();
                }

                // jump
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
                controller.Move(velocity * Time.deltaTime);

                // interact

                bool canInterract = false;
                RaycastHit hit;
                LayerMask mask = ~LayerMask.GetMask("HoldableObject");
                if (Physics.Raycast(myCamera.position, myCamera.forward, out hit, interactionDistance, mask))
                {
                    Transform t = hit.transform;
                    while (t != null)
                    {
                        Interactable interactable = t.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            if (Input.GetButtonDown("Fire1"))
                            {
                                interactable.Interact(transform);
                            }
                            if (interactable.CanInterract(transform))
                            {
                                canInterract = true;
                            }
                        }
                        t = t.parent;
                    }
                }
                InterractionOverlay.SetActive(canInterract);


                if (Input.GetButtonDown("Cancel"))
                {
                    isPaused = true;
                    AudioListener.pause = true;
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.Confined;
                    PauseMenu.alpha = 1f;
                    PauseMenu.interactable = true;
                    HUD.alpha = 0f;
                }
            }
            else
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    Resume();
                }
            }
        }
        else if (isDead)
        {
            if (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                timeElapsed = Mathf.Clamp(timeElapsed, 0, lerpDuration);
                float rate = timeElapsed / lerpDuration;
                DeathCanvas.alpha = rate;
                HUD.alpha = 1 - rate;
            }
        }
        else
        {
            if (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                timeElapsed = Mathf.Clamp(timeElapsed, 0, lerpDuration);
                float rate = timeElapsed / lerpDuration;
                WinCanvas.alpha = rate;
                HUD.alpha = 1 - rate;
            }
        }
    }

    public IEnumerator WalkCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => isWalking == true);
            int x = Random.Range(0, 5);
            switch (x)
            {
                case 0:
                    GetComponent<AudioSource>().PlayOneShot(walkSound1);
                    break;
                case 1:
                    GetComponent<AudioSource>().PlayOneShot(walkSound2);
                    break;
                case 2:
                    GetComponent<AudioSource>().PlayOneShot(walkSound3);
                    break;
                case 3:
                    GetComponent<AudioSource>().PlayOneShot(walkSound4);
                    break;
                case 4:
                    GetComponent<AudioSource>().PlayOneShot(walkSound5);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void Damage(float dmg)
    {
        UpdateHealth(health - dmg);
    }

    private void UpdateHealth(float newHealth)
    {
        health = newHealth;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.fillAmount = health / maxHealth;
        if (health == 0)
        {
            isDead = true;
            StartCoroutine(ReturnToMenu());
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(lerpDuration + 2f);
        SceneManager.LoadScene("Menu");
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void GoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Resume()
    {
        AudioListener.pause = false;
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.alpha = 0f;
        PauseMenu.interactable = false;
        HUD.alpha = 1f;
    }

    public void Win()
    {
        isWon = true;
        StartCoroutine(ReturnToMenu());
    }

    public void HideToolTip()
    {
        Tooltip.SetActive(false);
    }

    public void ShowToolTip()
    {
        Tooltip.SetActive(true);
    }

    public void SetToolTip(int index)
    {
        switch(index)
        {
            case 0:
                TooltipText.text = "Use W,A,S,D or Z,Q,S,D to move";
                break;
            case 1:
                TooltipText.text = "Use E or left mouse button to interract with objects";
                break;
            case 2:
                HideToolTip();
                break;
            case 3:
                ShowToolTip();
                TooltipText.text = "Use space to jump";
                break;
            default:
                HideToolTip();
                break;
        }
    }

}
