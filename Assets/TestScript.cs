using UnityEngine;

public class TestScript : MonoBehaviour
{
    [Header("缩放设置")]
    [SerializeField] private float minScale = 0.5f;  // 最小缩放值
    [SerializeField] private float maxScale = 2.0f;  // 最大缩放值
    [SerializeField] private float scaleSpeed = 1.0f; // 缩放速度
    
    private Vector3 originalScale;
    
    void Start()
    {
        // 保存物体的原始缩放大小
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        // 使用正弦波实现平滑的缩放动画
        // Sin值在-1到1之间，转换为0到1的范围
        float sineWave = (Mathf.Sin(Time.time * scaleSpeed) + 1f) / 2f;
        
        // 将0-1的值映射到minScale-maxScale范围
        float scaleMultiplier = Mathf.Lerp(minScale, maxScale, sineWave);
        
        // 应用缩放到物体
        transform.localScale = originalScale * scaleMultiplier;
    }
}
