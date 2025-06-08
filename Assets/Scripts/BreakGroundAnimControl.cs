using UnityEngine;

public class BreakGroundAnimControl : MonoBehaviour
{
    [SerializeField] private Transform _object5, _object6;
    public void SetBreakAnimation()
    {
        _object5.SetParent(null);
        _object6.SetParent(null);
        _object5.gameObject.SetActive(true);
        _object6.gameObject.SetActive(true);
        Destroy(gameObject, 1.5f);
    }
}
