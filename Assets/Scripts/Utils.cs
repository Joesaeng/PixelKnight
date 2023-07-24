public class Utils
{
    public static float Percent(float current, float max)
    {
        return current != 0 && max != 0 ? current / max : 0;
    }
    public static float Random0_1()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }
}
