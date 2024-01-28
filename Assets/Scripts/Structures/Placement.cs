using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask structureLayer;
    [SerializeField] private Structure currentStructure;
    [SerializeField] private float rotationSpeed = .1f;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Structure[] structures;
    [SerializeField] private EventSystem eventSystem;
    
    private Camera _camera;
    private float _yRotation;
    private RaycastHit _hit;
    private int _selectedButton = -1;

    private void Start()
    {
        _camera = Camera.main;

        var structure = structures[UnityEngine.Random.Range(0, structures.Length)].gameObject.GetComponent<Structure>();
        buttons[0].GetComponentInParent<Slider>().value = 0f;
        buttons[0].onClick.AddListener(() =>
        {
            _selectedButton = 0;
            SetObject(structure);
        });
        
        for (var i = 1; i < buttons.Count; i++)
        {
            StartCoroutine(CountToNewStructure(structures[UnityEngine.Random.Range(0, structures.Length)], buttons[i]));
        }
    }

    private void FixedUpdate()
    {
        if (currentStructure == null) return;
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out _hit, 100f, groundLayer))
        {
            currentStructure.gameObject.SetActive(true);
        }
        else
        {
            currentStructure.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (currentStructure == null) return;
        
        if (_selectedButton >= 0 && _selectedButton < buttons.Count)
        {
            buttons[_selectedButton].Select();
        }
        
        //check if placeable. if not, change color to red.
        if (currentStructure.CheckPlaceable())
        {
            currentStructure.GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            currentStructure.GetComponent<Renderer>().material.color = Color.red;
        }
        
        currentStructure.transform.position = _hit.point;
        currentStructure.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal) * Quaternion.Euler(0, _yRotation, 0) * Quaternion.Euler(currentStructure.rotationOffset);
    }
    
    public void Rotate(InputAction.CallbackContext context)
    {
        if (currentStructure == null) return;
        var maxRotation = currentStructure.maxRotation;
        var value = context.ReadValue<float>();
        _yRotation += value * rotationSpeed;
        if (maxRotation >= 180) return;
        _yRotation = Mathf.Clamp(_yRotation, -maxRotation, maxRotation);
    }
    
    public void PressButton1(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        if (buttons[0].GetComponentInParent<Slider>().value > 0f) return;
        _selectedButton = 0;
        buttons[0].onClick.Invoke();
    }
    
    public void PressButton2(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        if (buttons[1].GetComponentInParent<Slider>().value > 0f) return;
        _selectedButton = 1;
        buttons[1].onClick.Invoke();
    }
    
    public void PressButton3(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        if (buttons[2].GetComponentInParent<Slider>().value > 0f) return;
        _selectedButton = 2;
        buttons[2].onClick.Invoke();
    }

    public void SetObject(Structure structure)
    {
        if (currentStructure != null) Destroy(currentStructure.gameObject);
        _yRotation = 0f;
        currentStructure = Instantiate(structure, Vector3.zero, Quaternion.identity);
    }
    
    public IEnumerator CountToNewStructure(Structure structure, Button button)
    {
        button.onClick.RemoveAllListeners();
        currentStructure = null;
        EventSystem.current.SetSelectedGameObject(null);
        _selectedButton = -1;
        var slider = button.GetComponentInParent<Slider>();
        var time = 5f;
        
        while (time > 0)
        {
            slider.value = time / 5f;
            yield return new WaitForSeconds(0.1f);
            time -= 0.1f;
        }
        
        slider.value = 0f;
        
        button.onClick.AddListener(() =>
        {
            _selectedButton = buttons.IndexOf(button);
            SetObject(structure);
        });
    }

    public void Place(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled || currentStructure == null || !currentStructure.gameObject.activeSelf) return;
        if (eventSystem.IsPointerOverGameObject()) return;
        if (currentStructure.Place())
        {
            currentStructure = null;
            StartCoroutine(CountToNewStructure(structures[UnityEngine.Random.Range(0, structures.Length)], buttons[_selectedButton]));
        }
    }
    
    public void ResetRotation(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        _yRotation = 0f;
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (currentStructure == null) return;
        Destroy(currentStructure.gameObject);
    }

    public void OnDrawGizmos()
    {
        if (currentStructure == null) return;
        if (_camera == null) _camera = Camera.main;
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, 1000f, groundLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.5f);
        }
    }
}