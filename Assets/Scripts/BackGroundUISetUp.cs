using UnityEngine;

public class BackGroundUISetUp : MonoBehaviour
{
    [SerializeField] private float _actualRatio;
    [SerializeField] private float _currentRatio;
    private void Start()
    {
        _actualRatio = (float)(1080f / 1920f);
        _currentRatio = (float)Screen.width / (float)Screen.height;
        float xscaleFactor = _currentRatio / _actualRatio;
        Debug.Log("Width|" + _actualRatio + "|" + _currentRatio);
        transform.localScale = new Vector3(xscaleFactor, 1, 1);
    }

    


}
