using System;
using System.Collections;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using UnityEngine.Windows;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    public float playerSpeed = 7.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 5f;
    public float boostTimer = 5f;
    public bool isSpeedBoosted ;
    public float jumpBoostTimer = 5f;
    public bool isJumpBoosted ;
    [SerializeField] private float reloadAnimationTimer = 2f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float animationSmoothTime = 0.05f;
    [SerializeField] private float jumpAnimationPlayTransition = 0.15f;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimDistance = 1f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject reloadAnimationUi;
    [SerializeField] private GameObject aimCanvas;
    [SerializeField] private GameObject zomCameraCanvas;
    [Header("Gun")]
    [SerializeField] private ParticleSystem debuffEffect;
    [SerializeField] PlayerBulletPool playerBulletPool;
    [Header("Outfits")]
    public GameObject[] clothingArray;
    private int currentClothingIndex = 0;
    AudioManager audioManager;
    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    public static bool isReloadTime;
    private InputAction moveAction;
    public InputAction jumpAction;
    private InputAction shootAction;
    public static InputAction reloadAction;
    public static int playerHealth = 100;
    private Animator animator;
    int jumpAnimation;
    int reloadAnimation;
    int kneelAnimation;
    int shootingAnimation;
    int moveXAnimatorParameterId;
    int moveZAnimatorParameterId;
    int recoilAnimation;
    int rollAnimation;
    private bool isEffectRunning;
    public bool isDead ;
    //private bool isJumping = false;
    //public GameOverScreen gameOverScreen;
    // public float fireRate = 0.3f;
    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;
    public float offset;
    [Header("Enemy Health")]
    //public Health health;
    public bool isGameStart;

    private bool isShooting; // Buton basýlý olduðunda ateþ etme durumu
    private bool canShoot; // Ateþ edilebilir durum kontrolü
    private void Awake()
    {   // Gerekli bileþenleri al
        

        //StartCoroutine(EnablePlayer());
        BulletController.bulletSpeed = PlayerPrefs.GetInt("BulletSpeed", 50);
        currentClothingIndex = PlayerPrefs.GetInt("SelectedClothingIndex", 0);
        reloadAnimationTimer = PlayerPrefs.GetFloat("Range", 40);
        isGameStart = false;
        isEffectRunning = false;
        isShooting = false;
        canShoot = true;
        isSpeedBoosted = false;
        isReloadTime = false;
        isSpeedBoosted = false;
        isJumpBoosted = false;
        isDead = false;
        reloadAnimationUi.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        reloadAction = playerInput.actions["Reload"];
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Rifle Jump");
        // recoilAnimation = Animator.StringToHash("RifleRecoilShoot");
        moveXAnimatorParameterId = Animator.StringToHash("MoveX");
        moveZAnimatorParameterId = Animator.StringToHash("MoveZ");
        reloadAnimation = Animator.StringToHash("Reload");
        kneelAnimation = Animator.StringToHash("Kneel");
        shootingAnimation = Animator.StringToHash("Shooting");
        rollAnimation = Animator.StringToHash("Roll");
        playerHealth = 100;
        //debuffEffect.Stop();

        StopAllChildParticleSystems();



        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        
        reloadAnimationTimer /= 10;

    }


    private void OnEnable()
    {
        EnableShootAction();
    }

    private void OnDisable()
    {
        DisableShootAction();
    }

    private void EnableShootAction()
    {
        shootAction.started += OnShootStarted;
        shootAction.canceled += OnShootCanceled;
    }

    private void DisableShootAction()
    {
        shootAction.started -= OnShootStarted;
        shootAction.canceled -= OnShootCanceled;
    }
    private void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true; // Buton basýldýðýnda ateþ etmeye baþla
        StartCoroutine(ShootCoroutine()); // Ateþ etme iþlemini baþlatan bir coroutine çaðýr
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        isShooting = false; // Buton býrakýldýðýnda ateþ etmeyi durdur
    }
    //private void OnShootPerformed(InputAction.CallbackContext context)
    //{
    //    ShootGun();
    //}


    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true; // Buton basýldýðýnda ateþ etmeye baþla
            StartCoroutine(ShootCoroutine()); // Ateþ etme iþlemini baþlatan bir coroutine çaðýr
        }
        else if (context.canceled)
        {
            isShooting = false; // Buton býrakýldýðýnda ateþ etmeyi durdur
        }
    }

    // Sürekli ateþ etmeyi saðlayan coroutine
    //private IEnumerator ShootCoroutine()
    //{
    //    while (isShooting)
    //    {
    //        if (canShoot)
    //        {
    //            ShootGun(); // Ateþ etme fonksiyonunu çaðýr
    //            canShoot = false; // Bir sonraki ateþe kadar beklemek için ateþ durumunu deðiþtir
    //            yield return new WaitForSeconds(SelectedWeapon.fireRate); // 0.2 saniye bekletSelectedWeapon.fireRate * 0.01f
    //            canShoot = true; // Ateþ edilebilir duruma getir
    //        }
    //        yield return null; // Bir sonraki frame'e geç
    //    }
    //}
    private IEnumerator ShootCoroutine()
    {

        while (isShooting)
        {
            if (canShoot)
            {
                ShootGun(); // Ateþ etme fonksiyonunu çaðýr
                canShoot = false; // Bir sonraki ateþe kadar beklemek için ateþ durumunu deðiþtir
                if (SelectedWeapon.fireRate == 0)
                {
                    SelectedWeapon.fireRate = 50f;
                }
                // Fire rate deðerini saniye cinsine dönüþtür
                float secondsPerShot = 1 / (SelectedWeapon.fireRate * 0.1f);

                //Debug.Log(secondsPerShot);

                // Bekleme süresini ayarla
                yield return new WaitForSeconds(secondsPerShot);

                canShoot = true; // Ateþ edilebilir duruma getir
            }
            yield return null; // Bir sonraki frame'e geç
        }
    }

    // Diðer kodlar burada...


    // Silah ateþi iþlemlerini gerçekleþtiren fonksiyon
    private void ShootGun()
    {
        if (Bullet.bulletCount > 0 && !isReloadTime)
        {
            animator.CrossFade(shootingAnimation, 1.3f);
            Bullet.bulletCount--;

            audioManager.GunAudioSource();

            //Instantiate(muzzleFlash, barrelTransform.position, Quaternion.identity);
            RaycastHit hit;

            GameObject playerBullet = playerBulletPool.GetObject();
            if (playerBullet == null)
            {
                Debug.LogError("playerBullet nesnesi null! Kontrol edin.");
                return;
            }
            BulletController bulletController = playerBullet.GetComponent<BulletController>();
            if (bulletController == null)
            {
                Debug.LogError("BulletController bileþeni bulunamadý! playerBullet nesnesine eklendiðinden emin olun.");
                return;
            }
            playerBullet.transform.position = barrelTransform.position;
            playerBullet.transform.rotation = Quaternion.identity;
            // Rigidbody rb = playerBullet.GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 20f, ForceMode.Impulse);

            //GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            //BulletController bulletController = playerBullet.GetComponent<BulletController>();
            playerBullet.SetActive(true);


            // offset = 0.5f; // Ýsteðe baðlý, raycast'in baþlangýç noktasýnýn kameradan ne kadar önde olacaðýný belirler
           
            if (Physics.Raycast(cameraTransform.position + cameraTransform.forward * offset, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                // Iþýn bir þeye çarparsa, çarpma noktasý hedef olarak atanýyor
                bulletController.target = hit.point;
                bulletController.hit = true;
                Debug.DrawRay(cameraTransform.position + cameraTransform.forward * offset, cameraTransform.forward * hit.distance, Color.red, 2f);
            }
            else
            {
                // Iþýn bir þeye çarpmazsa, varsayýlan bir hedef belirleniyor
                bulletController.target = cameraTransform.position + cameraTransform.forward * 1000f; // Çarpma noktasý yerine ileri bir nokta belirleyin
                bulletController.hit = false;
                Debug.DrawRay(cameraTransform.position + cameraTransform.forward * offset, cameraTransform.forward * 1000f, Color.green, 2f);
            }

            //if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            //{
            //    // 5. Iþýn bir þeye çarparsa, çarpma noktasý hedef olarak atanýyor
            //    //  hit.transform.SendMessage("BuildingDamage", 30);
            //    bulletController.target = hit.point;
            //    bulletController.hit = true;
            //    Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.red, 2f);
            //}
            //else
            //{
            //    // 6. Iþýn bir þeye çarpmazsa, varsayýlan bir hedef belirleniyor

            //    bulletController.target = hit.point;//cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            //    bulletController.hit = false;
            //    Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.red, 2f);
            //}

            //  yumuþak bir geçiþ
            animator.CrossFade(recoilAnimation, jumpAnimationPlayTransition);

        }
        if (Bullet.bulletCount == 0 && Bullet.spareBulletCount > 0 && !isReloadTime)
        {

            isReloadTime = true;
            audioManager.ReloadAudioSource();

            animator.CrossFade(reloadAnimation, .3f);

                aimCanvas.SetActive(false);
                zomCameraCanvas.SetActive(false);

               

            reloadAnimationUi.SetActive(true);
            // Animasyon uzunluðunu alýn
            
            if (Bullet.spareBulletCount >= 10)
            {
                Bullet.bulletCount = 10;
                Bullet.spareBulletCount -= 10;
            }
            else if (Bullet.spareBulletCount > 0)
            {
                Bullet.bulletCount = Bullet.spareBulletCount;
                Bullet.spareBulletCount = 0;
            }
            // Reload zamanýný bitir
            StartCoroutine(EndReloadTime());

        }


    }
    private void SetAnimationSpeed(string animationName, float speed)
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        foreach (var info in clipInfo)
        {
            if (info.clip.name == animationName)
            {
                animator.SetFloat("Reload", speed);
                break;
            }
        }
    }
    bool isRunning = false;

    void Update()
    {
        if (!isDead & isGameStart)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                animator.CrossFade(rollAnimation, 1.3f);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                animator.CrossFade(shootingAnimation, 1f);
            }
            
                UpdateAim();
                UpdateRotation();
            
            
            UpdateReload();
            DrawRaycastLines();

            UpdateGroundedPlayer();
            if (controller.enabled)
            {
                UpdateMovement();
            }

            UpdateJump();
            UpdateGravityAndMove();
            
            if (isShooting)
            {
                // Sürekli ateþ etmeyi saðlayan coroutine'u baþlat
                StartCoroutine(ShootCoroutine());
            }
        }
        
    }
    void UpdateGroundedPlayer()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            // yere düþerken aniden durmasý
            playerVelocity.y = 0f;
        }
    }
    private void UpdateMovement()
    {
        // Kullanýcýnýn girdilerini al
        Vector2 input = moveAction.ReadValue<Vector2>();
        // Hareket vektörünü oluþtur
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);

        //Bu satýr, move adýnda bir Vector3 vektörü oluþturur. Bu vektör, genellikle karakterin düzlemdeki hareketini temsil eder.
        //Yani, y eksenindeki hareket ihmal edilir (0 olarak ayarlanýr) ve x-z düzlemindeki hareketi currentAnimationBlendVector'a dayalý olarak alýr.
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);

        // Kamera yönlendirmelerini dikkate alarak hareket vektörünü güncelle
        //  karakterin hareket vektörünü dünya koordinatlarýna dönüþtürülmesi
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        // karakterin dikeyde harektini engeller
        move.y = 0f;
        // input.magnitude =>  (input.x, input.y) noktasýndan baþlayýp orijine kadar olan uzaklýðý
        isDead = ScoreManager.instance.isGameOver;
        if (input.magnitude > 0.1f && !isRunning && groundedPlayer)
        {
            audioManager.RunAudioSource();
            isRunning = true;
        }
        else if (input.magnitude <= 0.1f && isRunning || !groundedPlayer)
        {
            audioManager.runAudioSource.Stop();
            isRunning = false;
        }

        // CharacterController bileþenini ile karakterin hareketi saðlanýr
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat(moveXAnimatorParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimatorParameterId, currentAnimationBlendVector.y);
        // Animator
    }
    private void UpdateReload()
    {
        if (reloadAction.triggered)
        {
            isReloadTime = true;
            if (Bullet.spareBulletCount > 0 && Bullet.bulletCount < 10)
            {
                int bulletsToReload = Mathf.Min(10 - Bullet.bulletCount, Bullet.spareBulletCount);
                Bullet.bulletCount += bulletsToReload;
                Bullet.spareBulletCount -= bulletsToReload;
                audioManager.ReloadAudioSource();
                // Yeniden yükleme animasyonunu baþlat
                animator.CrossFade(reloadAnimation, .3f);
                reloadAnimationUi.SetActive(true);
                aimCanvas.SetActive(false);
                zomCameraCanvas.SetActive(false);

                //animator.SetFloat("reloadSpeed", .9f);
                //SetAnimationSpeed("Reload", 2f);
            }
            StartCoroutine(EndReloadTime());
        }
    }
    private void UpdateAim()
    {
        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
    }
    private void UpdateJump()
    {
        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, jumpAnimationPlayTransition);
            if (jumpHeight == 4)
            {
                StartCoroutine(JumpRoutine());
            }
            else
            {
                audioManager.JumpAudioSource();
            }

        }
    }
    IEnumerator JumpRoutine()
    {
        yield return new WaitForSeconds(1.1f);
        audioManager.JumpAudioSource();

    }
    private void UpdateRotation()
    {
        // rotate towards  camera  direction
        // kameranýn yatay dönüþ açýsýný (y eksenindeki açýyý) targetAngle deðiþkenine atama
        float targetAngle = cameraTransform.eulerAngles.y;
        // sadece y ekseninde dönüþ yapýlacaðý için diðer açýlar sýfýr olarak ayarlandý
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        // karakterin mevcut dönüþünü (transform.rotation) hedef dönüþe (targetRotation) doðru yumuþak bir geçiþ yaparak günceller.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    IEnumerator EndReloadTime()
    {
        yield return new WaitForSeconds(reloadAnimationTimer); // Þarjör deðiþtirme süresi
        isReloadTime = false; // Ateþ etme engeli kaldýrýldý
        reloadAnimationUi.SetActive(false);
        aimCanvas.SetActive(true);
        zomCameraCanvas.SetActive(true);
    }
    public void TakeDamage(int damage)
    {
        //audioManager.PlayRunAudioSource(audioManager.runClip);
        //playerHealth -= damage;
        //PlayerHealth.TakeDamage(damage);
        audioManager.PlayerTakeDamageAudioSource();
        //Debug.Log(playerHealth);
        if (playerHealth <= 0)
        {
            //Time.timeScale = 0.0f;
            GameManager.Instance.SetGameOver();

        }

        StartCoroutine(EnableEffectForDuration(2f));

    }
    private void UpdateGravityAndMove()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void DisableAllClothing()
    {
        foreach (GameObject clothing in clothingArray)
        {
            clothing.SetActive(false);
        }
    }
    void StopAllChildParticleSystems()
    {
        ParticleSystem[] childParticleSystems = GetComponentsInChildren<ParticleSystem>(true);

        foreach (ParticleSystem ps in childParticleSystems)
        {
            if (ps != null)
            {
                ps.Stop();
            }
        }
    }

    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap
        //Debug.Log(gameObject.name);
        debuffEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        debuffEffect.Stop();
        isEffectRunning = false;
    }
    void DrawRaycastLines()
    {
        RaycastHit hit;

        // Oyuncu layer'ýný göz ardý etmek için bir layer mask oluþturun
        int playerLayer = LayerMask.NameToLayer("Player"); // Oyuncu layer'ýnýn adýný buraya yazýn
        int layerMask = ~(1 << playerLayer); // Oyuncu layer'ýný göz ardý edecek layer mask

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 1000f, Color.green); // Uzun bir mesafe için yeþil çizgi
        }
    }
}
// OnEnable ve OnDisable metodlarý, ateþ etme eyleminin durumunu takip eder
//private void OnEnable()
//{
//    shootAction.performed += _ => ShootGun();
//}
//private void OnDisable()
//{
//    shootAction.performed -= _ => ShootGun();
//}