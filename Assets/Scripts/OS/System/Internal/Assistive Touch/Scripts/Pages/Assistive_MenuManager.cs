using UnityEngine;

public class Assistive_MenuManager : MonoBehaviour
{
    [SerializeField] Assistive_Menu[] menus;
    public string currentMenu;

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Assistive_Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }

        menu.Open();
        currentMenu = menu.menuName;
    }

    public void CloseMenu(Assistive_Menu menu)
    {
        menu.Close();
    }
}
