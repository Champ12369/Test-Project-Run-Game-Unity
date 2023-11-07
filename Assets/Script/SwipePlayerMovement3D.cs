using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AutoMovePlayer : MonoBehaviour
{
    public followplayer followCamera;
    public float moveSpeed = 5.0f; // Adjust the speed as needed
    public float swipeSpeed = 10.0f; // Adjust the swipe speed as needed
    public float swipeStep = 2.0f; // The step length for swipes
    public float jumpForce = 10.0f; // The force of the jump
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isMoving;
    private bool isJumping;
    public TextMeshProUGUI coinCountText;
    private int coinCount;
    public Animator animator,animatorCanvas;
    bool jumpgo = false;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float minSwipeDistance =150f;
    public AudioSource source,backgroundsources;
    public AudioClip JumpSound,runsound,Slidesound, Coinsound,deathHitsound, gamoverSound,clipbackgroundrun;
    public ParticleSystem particlessystem;
    private void Start()
    {     moveSpeed = 0f; // Adjust the speed as needed
          swipeSpeed = 0f; // Adjust the swipe speed as needed
          swipeStep = 0f; // The step length for swipes
          jumpForce = 0f;
        followCamera.offset.y = 10;
        followCamera.offset.z = -21;
        animator.Play("Start");
        initialPosition = transform.position;
        targetPosition = initialPosition;
        coinCount = 0;
        ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
        emissionModule.rateOverTime = 0; // Replace 10 with your desired emission rate
        UpdateCoinCountText();
        Application.targetFrameRate = 45;
    }

    public GameObject Play;
    public void startThegame()
    {
        ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
        emissionModule.rateOverTime = 20;

        followCamera.offset.y = 10;
        followCamera.offset.z = 19;
        backgroundsources.clip = clipbackgroundrun;
        backgroundsources.Play();
        moveSpeed = 46f; // Adjust the speed as needed
        swipeSpeed = 25f; // Adjust the swipe speed as needed
        swipeStep = 6f; // The step length for swipes
        jumpForce = 12f;
        animator.Play("startrun");
        Play.SetActive(false);

    }

    IEnumerator SlideDown() 
    {

        capsuleCollider.height = 0.1f;
        capsuleCollider.radius = 0.1f;
        capsuleCollider.center = new Vector3(0, -0.5f, 0);
        yield return new WaitForSeconds(1f);
        followCamera.offset.y = 10;
        yield return new WaitForSeconds(1f);
        capsuleCollider.height = 2f;
        capsuleCollider.radius = 0.5f;
        capsuleCollider.center = new Vector3(0, 0, 0);
    }
    public CapsuleCollider capsuleCollider;
    public TextMeshProUGUI PintText;
    bool jumponetime=false;
    private void LateUpdate()
    {
        Vector3 playerPosition = this.transform.position;
        float distanceToOrigin = Vector3.Distance(playerPosition, Vector3.zero);
        int distanceInt = Mathf.RoundToInt(distanceToOrigin)-2;
        PintText.text = "" + distanceInt;
    }
    private void Update()
    {



        if (!isMoving)
        {


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEndPos = touch.position;

                    // Calculate swipe direction and distance
                    Vector2 swipeDirection = touchEndPos - touchStartPos;
                    float swipeDistance = swipeDirection.magnitude;

                    if (swipeDistance >= minSwipeDistance)
                    {
                        // Check for Up or Down swipe
                        if (swipeDirection.y > 0)
                        {

                            if(!isJumping && jumponetime == false)
                            {
                                ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
                                emissionModule.rateOverTime = 0;
                                capsuleCollider.height = 2f;
                                capsuleCollider.radius = 0.5f;
                                followCamera.offset.y =10;
                                source.loop = false;
                                source.clip = JumpSound;
                                source.Play();
                                animator.Play("Swiperight");
                                Jump();
                                jumponetime = true;
                                // Swipe Up
                                //Debug.Log("Swipe Up");


                            }
                            source.loop = false;

                            // Add your code for handling the Up swipe here
                        }
                        else
                        {
                            jumponetime = false;
                            followCamera.offset.y = 10;
                            jumpgo = false;
                            source.loop = false;
                            source.clip = Slidesound;
                            source.Play();
                            GetComponent<Rigidbody>().AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
                            StopAllCoroutines();
                            StartCoroutine(SlideDown());
                            animator.Play("Swipeleft");
                            // Swipe Down
                           // Debug.Log("Swipe Down");
                            // Add your code for handling the Down swipe here
                        }
                    }
                }
            }




            if (Input.GetKeyDown(KeyCode.DownArrow))
            {


                jumponetime = false;
                followCamera.offset.y =0;
                jumpgo = false;
                source.loop = false;
                source.clip = Slidesound;
                source.Play();
                GetComponent<Rigidbody>().AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
                StopAllCoroutines();
                StartCoroutine(SlideDown());
                animator.Play("Swipeleft");
                //MoveToTargetX(transform.position.x - swipeStep); // Swipe left
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                animator.Play("Swipeleft");
                MoveToTargetX(transform.position.x - swipeStep); // Swipe left
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                animator.Play("Swiperight");
                MoveToTargetX(transform.position.x + swipeStep); // Swipe right
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping && jumponetime==false)
            {
                ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
                emissionModule.rateOverTime = 0;
                capsuleCollider.height = 2f;
                capsuleCollider.radius = 0.5f;
                followCamera.offset.y = 10;
                source.loop = false;
                source.clip = JumpSound;
                source.Play();
                animator.Play("Swiperight");
                Jump();
                jumponetime = true;
            }
        }

        if (isMoving)
        {
            float step = swipeSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
        else
        {
            // Move the player forward along its local forward axis
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }


    public void RestartScene()
    {
        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);
    }

    

    private void FixedUpdate()
    {
        if (isJumping)
        {
            jumpgo = true;
            // Apply the jump force
            animator.Play("JumpAnimation");
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isJumping = false;
        }
    }

    private void MoveToTargetX(float x)
    {
        // Ensure the target position is within the allowed step
        x = Mathf.Clamp(x, -swipeStep, swipeStep);

        initialPosition = transform.position;
        targetPosition = new Vector3(x, transform.position.y, transform.position.z);
        isMoving = true;
    }

    private void Jump()
    {
        isJumping = true;
    }



    public float raycastDistance = 2f;

    public GameObject pagelose,buttonrestart;

    public interstialads interstialads;

    public AudioSource generalsource;
    IEnumerator Death()
    {
        source.loop = false;
        source.clip = deathHitsound;
        generalsource.Stop();
        source.Play();
        animator.Play("ForwardDeath");
        followCamera.offset.z = -50;
        yield return new WaitForSeconds(2f);
        source.loop = false;
        source.clip = gamoverSound;
        source.Play();
        pagelose.SetActive(true);
        yield return new WaitForSeconds(2f);
        interstialads.loadGogogog();
        yield return new WaitForSeconds(2f);
        buttonrestart.SetActive(true);
    }

    public ParticleSystem Hitparts;
    private void CheckForwardCollision()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {



                ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
                emissionModule.rateOverTime = 20;
                particlessystem.gameObject.SetActive(false);
                Hitparts.transform.position=this.transform.position;
                Hitparts.gameObject.SetActive(true);
                //particlessystem.gameObject.SetActive(true);
                StartCoroutine(Death());
                isMoving = true;
                moveSpeed = 0;
                swipeSpeed = 0;
                swipeStep = 0;
                
                jumpgo = false;
                //this.GetComponent<Rigidbody>().isKinematic = true;

                Debug.Log("Game Over: Hit a wall in the forward direction");
                // Handle game over logic here
            }
        }
    }

    private void CheckXCollision()
    {
        Vector3 right = transform.TransformDirection(Vector3.down);
        Vector3 left = transform.TransformDirection(Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, right, out hit, raycastDistance) || Physics.Raycast(transform.position, left, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
                emissionModule.rateOverTime = 20;
                particlessystem.gameObject.SetActive(false);
                particlessystem.gameObject.SetActive(true);
                followCamera.offset.y = 10;
                animator.Play("Player run");
                jumponetime = false;
                Debug.Log("Hit a wall on the x-axis");
                // Handle your logic for x-axis wall collision here
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            source.loop = false;
            source.clip = Coinsound;
            source.Play();
            animatorCanvas.Play("Animation");
            // Increment the coin count
            coinCount++;
            UpdateCoinCountText();

            // Destroy the collected coin
            other.gameObject.SetActive(false);
        }




    }



    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Floor") && jumpgo == true)
        {
            ParticleSystem.EmissionModule emissionModule = particlessystem.emission;
            emissionModule.rateOverTime = 20;
            particlessystem.gameObject.SetActive(false);
            particlessystem.gameObject.SetActive(true);
            source.loop = true;
            source.clip = runsound;
            source.Play();
            followCamera.offset.y = 10;
            animator.Play("Player run");
            jumponetime = false;
            jumpgo = false;

        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            // StartCoroutine(Death());
            //isMoving = true;
            // moveSpeed= 0;
            // swipeSpeed= 0;
            // swipeStep= 0;

            //this.GetComponent<Rigidbody>().isKinematic = true;

            CheckXCollision();
            CheckForwardCollision();


        }

    }
    private void UpdateCoinCountText()
    {
        if (coinCountText != null)
        {
            coinCountText.text = "" + coinCount;

            if (coinCount% 5 == 0 && coinCount!=0)
            {

                moveSpeed = moveSpeed + 1;
                followCamera.smoothfactor=followCamera.smoothfactor+0.001f;

            }
        }
    }
}


