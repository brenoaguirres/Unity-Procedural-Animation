using UnityEngine;
using TMPro;

public class TextBoxBillboard : MonoBehaviour
{
    TextMeshProUGUI _textBox;
    Camera _mainCamera;

    private void Start()
    {
        _textBox = GetComponent<TextMeshProUGUI>();
        _textBox.gameObject.SetActive(false);
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Billboard();
    }

    private void Billboard()
    {
        if (_mainCamera != null)
        {
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                             _mainCamera.transform.rotation * Vector3.up);
        }
    }

    public void ShowDamage(float value)
    {
        _textBox.text = $"{value}";
        _textBox.color = Color.red;
        _textBox.gameObject.SetActive(true);
        Invoke("HideDamage", 1f);
    }

    void HideDamage()
    {
        _textBox.gameObject.SetActive(false);
    }
}
