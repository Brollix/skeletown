using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private GameObject lastSelectedGameObject;

    private PlayerControls controls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            controls = new PlayerControls();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (controls != null)
        {
            controls.UI.Enable();
            controls.UI.Navigate.performed += OnNavigate;
        }
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.UI.Navigate.performed -= OnNavigate;
            controls.UI.Disable();
        }
    }

    private void Update()
    {
        if (EventSystem.current == null) return;

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        }
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            if (lastSelectedGameObject != null && lastSelectedGameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
            }
        }
    }

    public void SetDefaultSelection(GameObject defaultObj)
    {
        if (defaultObj != null)
        {
            StartCoroutine(SelectInNextFrame(defaultObj));
        }
    }

    private IEnumerator SelectInNextFrame(GameObject obj)
    {
        yield return null; 
        
        if (EventSystem.current != null && obj != null && obj.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(obj);
            lastSelectedGameObject = obj;
        }
    }
}
