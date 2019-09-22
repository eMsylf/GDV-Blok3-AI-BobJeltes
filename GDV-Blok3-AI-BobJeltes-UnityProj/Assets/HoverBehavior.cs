using UnityEngine;
using Panda;

public class HoverBehavior : MonoBehaviour
{
    [Range(0.1f, .99f)]
    public float lerpSpeed;

    [Task]
    private void MoveTo(float xCord, float yCord)
    {
        Vector2 goalPosition = new Vector2(xCord, yCord);
        if (Vector2.Distance(transform.position, goalPosition) > 0f)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(xCord, yCord), lerpSpeed);
        } else
        {
            Task.current.Succeed();
        }
    }
}
