using UnityEngine;

public class SplashX_MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA; // ลาก Empty Object จุด A มาใส่
    public Transform pointB; // ลาก Empty Object จุด B มาใส่
    public float speed = 3f;

    private Vector3 targetPos;

    void Start()
    {
        targetPos = pointB.position; // เริ่มต้นให้วิ่งไปหาจุด B
    }

    void Update()
    {
        // เคลื่อนที่แพลตฟอร์มไปหาเป้าหมาย
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // ถ้าถึงจุดหมายแล้ว ให้สลับไปอีกจุด
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = (targetPos == pointA.position) ? pointB.position : pointA.position;
        }
    }

    // --- ส่วนสำคัญ: ทำให้ตัวละครเคลื่อนที่ตามแพลตฟอร์ม ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ถ้าสิ่งที่มาชนมี Tag ว่า Player (น้องเคียว)
        if (collision.gameObject.CompareTag("Player"))
        {
            // ให้ Player กลายเป็น "ลูก" ของแพลตฟอร์ม
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // เมื่อกระโดดออก ให้ยกเลิกการเป็นลูก (SetParent เป็น null)
            collision.transform.SetParent(null);
        }
    }
}