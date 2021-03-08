using UnityEngine;

public class Warparound
{
    public static bool Reposition(Rigidbody rb, float x, float y, float range)
    {
        float new_x = x, new_y = y;
        bool trigger = false;

        if (x > Screen.width + range)
        {
            new_x = 0;
            trigger = true;
        }
        if (x < -range)
        {
            new_x = Screen.width + range;
            trigger = true;
        }
        if (y > Screen.height + range)
        {
            new_y = 0;
            trigger = true;
        }
        if (y < -range)
        {
            new_y = Screen.height + range;
            trigger = true;
        }
        if (trigger)
        {
            rb.MovePosition(new Vector3(new_x, new_y, 0));
        }
        return trigger;
    }
}
