using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Actor _actor;

    [Header("Player Settings")]
    [SerializeField]
    private GameplayInput _gameplayInput;
    [SerializeField]
    private PlayerData _data;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private PlayerSFXData _playerSFX;

    [Header("Ability Systems")]
    [SerializeField]
    private WeaponSystem _weaponSystem;
    [SerializeField]
    private DashSystem _dashSystem;
    [SerializeField]
    private PlayerAiming _playerAiming;

    public Actor Actor => _actor;

    public GameplayInput Input => _gameplayInput;
    public PlayerData Data => _data;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public BoxCollider2D BoxCollider => _boxCollider;
    public PlayerSFXData SFX => _playerSFX;

    public WeaponSystem WeaponSystem => _weaponSystem;
    public DashSystem DashSystem => _dashSystem;
    public PlayerAiming PlayerAiming => _playerAiming;

    public Vector2 AimDirection { get; private set; }

    public int AirJumpsRemaining { get; private set; }
    public float DashCooldown { get; private set; }

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

}
