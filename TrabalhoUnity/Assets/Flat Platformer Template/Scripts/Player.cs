using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

//Desenvolvido com aulas, próprio asset, ajuda do professor e link abaixo.
//https://unity3d.com/pt/learn/tutorials/topics/2d-game-creation/creating-basic-platformer-game
public class Player : MonoBehaviour
{
//teste	
    public float WalkSpeed;
    public float JumpForce;
    public AnimationClip _walk, _jump;
    public Animation _Legs;
    public Transform _Blade, _GroundCast;
    public Camera cam;
    public bool mirror;

    private bool _canJump, _canWalk;
    private bool _isWalk, _isJump;
    private float rot, _startScale;
    private Rigidbody2D rig;
    private Vector2 _inputAxis;
    private RaycastHit2D _hit;

    private float _initPos = 34.714f;
    private float finalPos = 0.0f;
    private float targetTime = 0.1f;
    private float deltaTime = 0.0f;
    private bool isAttaking = false;
    private float bladeAngle = 0.0f;
    private bool invert = false;

    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
    }

    void Update()
    {
        if (_hit = Physics2D.Linecast(new Vector2(_GroundCast.position.x, _GroundCast.position.y + 0.2f), _GroundCast.position))
        {
            if (!_hit.transform.CompareTag("Player"))
            {
                _canJump = true;
                _canWalk = true;
            }
        }
        else _canJump = false;

        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_inputAxis.y > 0 && _canJump)
        {
            _canWalk = false;
            _isJump = true;
        }

        if (Input.GetButtonDown("Fire1") && !isAttaking)
        {
            isAttaking = true;
            invert = false;
        }

        if (isAttaking)
        {
            deltaTime += Time.deltaTime;
            if (!invert)
            {
                bladeAngle = Mathf.LerpAngle(_initPos, finalPos, deltaTime / targetTime);
                if (deltaTime >= targetTime)
                {
                    invert = true;
                    deltaTime = 0.0f;
                }
            }
            else
            {
                bladeAngle = Mathf.LerpAngle(finalPos, _initPos, deltaTime / targetTime);
                if (deltaTime >= targetTime)
                {
                    invert = false;
                    isAttaking = false;
                    deltaTime = 0.0f;
                }
            }

            Vector3 rotation = _Blade.transform.localEulerAngles;
            rotation.z = bladeAngle;
            _Blade.transform.localEulerAngles = rotation;

        }
    }

    void FixedUpdate()
    {
        Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition) - _Blade.transform.position;
        dir.Normalize();

        if (cam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.2f)
            mirror = false;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.2f)
            mirror = true;

        if (!mirror)
        {
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }
        if (mirror)
        {
            rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }

        if (_inputAxis.x != 0)
        {
            rig.velocity = new Vector2(_inputAxis.x * WalkSpeed * Time.deltaTime, rig.velocity.y);

            if (_canWalk)
            {
                _Legs.clip = _walk;
                _Legs.Play();
            }
        }

        else
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
        }

        if (_isJump)
        {
            rig.AddForce(new Vector2(0, JumpForce));
            _Legs.clip = _jump;
            _Legs.Play();
            _canJump = false;
            _isJump = false;
        }
    }

    public bool IsMirror()
    {
        return mirror;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _GroundCast.position);
    }
}
