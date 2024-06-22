using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float limit = 75f;
    public float angleSpeed = 1f;

    public float time = 0.5f;
    private void Awake()
    {
        // Hàm ác-sin :v, để tính ngược lại giá trị của góc khi biết sin()
        // Debug.Log("Pha ban dau: " + Mathf.Asin(time) * Mathf.Rad2Deg);
    }
    private void Update()
    {
        // Debug.Log("Time.deltaTime = " + Time.deltaTime);
        time += Time.deltaTime;
        float angle = limit * Mathf.Sin(time * angleSpeed); // Dạng phương trình của dao động điều hòa, sin or cos đều như nhau
        // thời gian t * tốc độ góc w
        // chọn pha ban đầu thì phải chọn time, chứ không xoay góc được :V
        // Time.time: Khoảng thời gian kể từ khi bắt đầu chạy app đến frame hiện tại
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
