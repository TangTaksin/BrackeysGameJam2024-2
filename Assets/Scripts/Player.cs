using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D _body;
    Animator _animator;

    bool _canAct;

    float _movementSpeed;
    public float baseWalkSpeed;

    float _stamina;
    public float baseStamina;

    public delegate void OnVariableChangeDelegate(Interactable newVal);
    public static OnVariableChangeDelegate OnVariableChange;

    public delegate void PlayerBoolEvent(bool _value);
    public static PlayerBoolEvent ChangePlayerCanActBool;

    [SerializeReference] Interactable _selectedInteractable;
    public Interactable selectedInteractable
    {
        get { return _selectedInteractable; }
        set
        {
            if (_selectedInteractable == value) return;
            _selectedInteractable = value;
            if (OnVariableChange != null)
                OnVariableChange?.Invoke(_selectedInteractable);
        }
    }

    [SerializeReference] List<Interactable> _InteractableList = new List<Interactable>();

    [SerializeReference] ItemData _heldItem;

    Vector2 _InputVector2;
    Vector2 _facingVector2 = Vector2.down;
    Vector2 _lastMoveDirection = Vector2.down;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ChangePlayerCanActBool += SetActionBool;
    }

    private void OnDisable()
    {
        ChangePlayerCanActBool -= SetActionBool;
    }

    void SetActionBool(bool _value)
    {
        _canAct = _value;
    }

    private void Update()
    {
        UpdateMovement();

        DecideSelectedInteractable();
    }

    private void UpdateMovement()
    {
        _movementSpeed = baseWalkSpeed;
        _body.position += _InputVector2 * _movementSpeed * Time.deltaTime;

        // Update animation parameters for movement and idle
        if (_InputVector2 != Vector2.zero)
        {
            _animator.SetFloat("Horizontal", _InputVector2.x);
            _animator.SetFloat("Vertical", _InputVector2.y);
            _animator.SetFloat("Speed", _InputVector2.sqrMagnitude);

            // Update the last direction when moving
            _lastMoveDirection = _InputVector2.normalized;
        }
        else
        {
            // Player is idle, use the last movement direction
            _animator.SetFloat("Horizontal", _lastMoveDirection.x);
            _animator.SetFloat("Vertical", _lastMoveDirection.y);
            _animator.SetFloat("Speed", 0);  // Set Speed to 0 to trigger idle
        }


    }

    public void OnMove(InputValue _value)
    {
        if (!_canAct)
            return;

        _InputVector2 = _value.Get<Vector2>();
        var vMag = _InputVector2.SqrMagnitude();

        _facingVector2 = Vector2.Lerp(_facingVector2, _InputVector2, vMag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interact = collision.GetComponent<Interactable>();

        if (interact is Interactable)
        {
            _InteractableList.Add(interact);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        var interact = collision.GetComponent<Interactable>();

        if (_InteractableList.Contains(interact))
        {
            if (_selectedInteractable == interact)
                selectedInteractable = null;

            _InteractableList.Remove(interact);

        }

        if (_InteractableList.Count == 0)
        {
            selectedInteractable = null;
        }
    }

    void DecideSelectedInteractable()
    {
        if (_InteractableList.Count == 0)
            return;

        float ClosestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach (var interact in _InteractableList)
        {
            if (!interact.isInteractable)
                continue;

            Vector3 DifferenceToTarget = interact.transform.position - currentPosition;
            float DistanceToTarget = DifferenceToTarget.sqrMagnitude;

            if (DistanceToTarget < ClosestDistance)
            {
                ClosestDistance = DistanceToTarget;
                selectedInteractable = interact;
            }
        }
    }

    public void OnInteract()
    {
        if (!_canAct)
            return;

        if (_selectedInteractable != null)
        {
            if (_heldItem != null)
            {
                var used = _heldItem.UseItem(_selectedInteractable);
                if (used)
                {
                    _stamina -= _heldItem.cost;
                    _heldItem = null;
                }
                else
                    _selectedInteractable.Interact(this);
            }
            else
                _selectedInteractable.Interact(this);
        }

    }

    public void SetItem(ItemData _item)
    {
        _heldItem = _item;
    }
}
