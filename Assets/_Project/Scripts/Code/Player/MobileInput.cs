using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static bool moveLeft;
    public static bool moveRight;
    public void LeftDown() { moveLeft = true; }
    public void LeftUp() {  moveLeft = false; }
    public void RightDown() {  moveRight = true; }
    public void RightUp() { moveRight = false; }
}
