using UnityEngine;
using System.Collections;

// Static class for angle related helper functions
public static class csHelperFunctions 
{
	// Keep an angle in the 0-360 range
	public static float WrapAngle
        (float angle)
    {
        while (angle < 0)   angle += 360;
        while (angle > 360) angle -= 360;
        return angle;
    }
  
	// Determine the angle between a source and target vector (2D)
    public static float AngleToTarget
        (Vector2 origin, Vector2 target)
    {
        float x = target.x - origin.x;
        float y = target.y - origin.y;
        float a = (float)Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return csHelperFunctions.WrapAngle(a);		
    }       
	
	// Determine the angle from a direction vector
    public static float AngleFromVector
        (Vector2 inVector)
    {
        if (inVector == Vector2.zero) return 0.0f;
        inVector.Normalize();
        return csHelperFunctions.AngleToTarget(Vector2.zero, inVector);
    }
	
	// Determines the angle between two existing angles
    public static float GetAngle
        (float a1, float a2)
    { return (Mathf.Abs(a1 - a2)) % 360; }
	
	// Determine the shortest angle between two existing angles
    public static float GetShortAngle
        (float a1, float a2)
    {
        float angle = csHelperFunctions.GetAngle(a1, a2);

        if (angle > 180)
            angle = 360 - angle;

        return angle;
    }
}
