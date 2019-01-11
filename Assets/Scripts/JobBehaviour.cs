using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct MoveJob : IJob
{
    public float dt;
    public NativeList<Vector2> positions;
    public float speed;

    public void Execute()
    {
        positions[2] = positions[0] + positions[1].normalized * speed * dt;
    }
}

public class JobBehaviour : MonoBehaviour
{
    public Rect availableZone;
    public float speed;
    private Vector2 direction;

    public void Start()
    {
        direction = Random.insideUnitCircle;
    }

    private void Update()
    {
        NativeList<Vector2> positions = new NativeList<Vector2>(3, Allocator.TempJob);
        positions.Add(transform.position);
        positions.Add(direction);
        positions.Add(Vector2.zero);
        MoveJob moveJob = new MoveJob {positions = positions, dt = Time.deltaTime, speed = speed};
        JobHandle handle = moveJob.Schedule();
        handle.Complete();
        if (!availableZone.Contains(positions[2]))
        {
            direction = -direction;
        }
        else
        {
            transform.position = positions[2];
        }
        positions.Dispose();
    }
}