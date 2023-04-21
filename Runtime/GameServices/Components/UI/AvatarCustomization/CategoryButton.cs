using UnityEngine;

public class CategoryButton : MonoBehaviour
{
    [SerializeField] private GameObject activeIcon;
    [SerializeField] private GameObject inactiveIcon;

    public void Activate(bool active)
    {
        activeIcon.SetActive(active);
        inactiveIcon.SetActive(!active);
    }
}