using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [SerializeField] List<Menu> menus = new List<Menu>();

    void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuname)
    {
        foreach(Menu m in menus)
        {
            if(menuname == m.menuName)
            {
                 m.Open();
            }
            else if(m.isOpen)
            {
                CloseMenu(m);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        foreach(Menu m in menus)
        {
            if (m.isOpen)
            {
                CloseMenu(m);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
