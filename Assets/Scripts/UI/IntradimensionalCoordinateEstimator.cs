using TMPro;
using UnityEngine;

public class IntradimensionalCoordinateEstimator : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Transform target;
    
    // Update is called once per frame
    void Update()
    {
        float x = target.position.x;
        float y = target.position.y;
        float z = target.position.z;

        x *= 10;
        z *= 10;
        if (y < -500) y = 500;
        
        string sx = x.ToString("000.000");
        string sy = y.ToString("000.000");
        string sz = z.ToString("000.000");
        text.SetText($"INTRADIMENSIONAL COORDINATE EXISTIMATOR:\n#{sx} #{sy} #{sz}");
    }
}
