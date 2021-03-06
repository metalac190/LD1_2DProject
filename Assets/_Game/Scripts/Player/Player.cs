using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Header("Player Settings")]
    [SerializeField]
    private GameplayInput _gameplayInput;
    [SerializeField]
    private PlayerMoveData _data;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private PlayerSFXData _playerSFX;
    [SerializeField]
    private PlayerVisuals _visuals;

    [Header("Ability Systems")]
    [SerializeField]
    private WeaponSystem _weaponSystem;
    [SerializeField]
    private DashSystem _dashSystem;

    public GameplayInput Input
    {
        //TODO this is doing a scenewide search. Consider a better method for connecting the game input
        // with the player character after it's spawned
        get
        {
            if (_gameplayInput == null)
            {
                //Debug.Log("Player Search for input");
                _gameplayInput = FindObjectOfType<GameplayInput>();
                if(_gameplayInput == null)
                {
                    Debug.LogError("No gameplay input in scene");
                }
            }

            return _gameplayInput;
        }
    }

    public PlayerMoveData Data => _data;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public BoxCollider2D BoxCollider => _boxCollider;
    public PlayerSFXData SFX => _playerSFX;
    public PlayerVisuals Visuals => _visuals;

    public WeaponSystem WeaponSystem => _weaponSystem;
    public DashSystem DashSystem => _dashSystem;

    public Inventory Inventory { get; private set; } = new Inventory();

    public Vector2 AimDirection { get; private set; }

    public int AirJumpsRemaining { get; private set; }
    public float DashCooldown { get; private set; }

    public void Initialize(GameplayInput gameplayInput)
    {
        //Debug.Log("Initialize input");
        //TODO currently Initialize gets called AFTER this object States are created. This causes
        // null references for input. Determine how to send input properly when this player is spawned
        _gameplayInput = gameplayInput;
    }

    private void Awake()
    {
        ResetJumps();
    }

    public void DecreaseAirJumpsRemaining() => AirJumpsRemaining--;
    public void ResetJumps() => AirJumpsRemaining = _data.AmountOfAirJumps;

    public void SetColliderHeight(float height)
    {
        Vector2 center = BoxCollider.offset;
        Vector2 newSize = new Vector2(BoxCollider.size.x, height);

        center.y += (height - BoxCollider.size.y) / 2;

        BoxCollider.size = newSize;
        BoxCollider.offset = center;
    }

    // because our game system needs to control existence, we want to be able to play
    // this externally
    public void PlayDeathFX()
    {
        ParticleSystem deathParticle = Instantiate
            (Visuals.DeathParticlePrefab, transform.position, Quaternion.identity);
        deathParticle.Play();

        SFX.DeathSFX.PlayOneShot(transform.position);
    }
}
