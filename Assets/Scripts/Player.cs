using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D _body;

    float _movementSpeed;
    public float baseWalkSpeed;

    float _stamina;
    public float baseStamina;

    public delegate void OnVariableChangeDelegate(Interactable newVal);
    public static event OnVariableChangeDelegate OnVariableChange;

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

    Vector2 _InputVector2;
    Vector2 _facingVector2 = Vector2.down;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();    
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
    }

    public void OnMove(InputValue _value)
    {
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
        if (_selectedInteractable != null)
        _selectedInteractable.Interact();
    }
}
