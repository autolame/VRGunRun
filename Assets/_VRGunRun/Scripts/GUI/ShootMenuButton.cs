using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShootMenuButton : MonoBehaviour
{
    [SerializeField] private TextMeshPro buttonText;
    private string menu;
    private bool activated = false;
    private ShootMenuManager menuManager;

    public string Menu
    { get { return menu; } }

    private void Awake()
    {
        menu = buttonText.text;
        menuManager = transform.root.GetComponent<ShootMenuManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            menuManager.ActivateMenu(menu, name);
        }
    }
}
