using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [Space]
    [SerializeField] private float scale = 0.15f;
    [SerializeField] private float drawSpeed = 50f;
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float rotationAngle = -15f;
    [SerializeField] private Vector3 positionOffset = new(0f, 0f, 0f);

    private int points = 100;
    private Vector3[] positions;
    private float time = 0f;
    private float currentDrawnPoints = 0;

    void Start()
    {
        lineRenderer.positionCount = 0;
        positions = new Vector3[points];
        time = 0f;
        currentDrawnPoints = 0;
    }

    void Update()
    {
        GenerateHeartPoints();

        if (currentDrawnPoints < points)
        {
            currentDrawnPoints += drawSpeed * Time.deltaTime;
            currentDrawnPoints = Mathf.Clamp(currentDrawnPoints, 0, points);

            lineRenderer.positionCount = Mathf.FloorToInt(currentDrawnPoints);
            lineRenderer.SetPositions(positions[..Mathf.FloorToInt(currentDrawnPoints)]);
        }
        else
        {
            time += Time.deltaTime * animationSpeed;
            ApplyPulse(time);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Start();
        }
    }

    void GenerateHeartPoints()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);

        for (int i = 0; i < points; i++)
        {
            float t = i * 2 * Mathf.PI / points;
            float x = 16 * Mathf.Pow(Mathf.Sin(t), 3);
            float y = 13 * Mathf.Cos(t) - 5 * Mathf.Cos(2 * t) - 2 * Mathf.Cos(3 * t) - Mathf.Cos(4 * t);
            Vector3 heartPoint = new Vector3(x, y, 0) * scale;
            positions[i] = rotation * heartPoint + positionOffset;
        }
    }

    void ApplyPulse(float tOffset)
    {
        Vector3[] animatedPositions = new Vector3[points];
        for (int i = 0; i < points; i++)
        {
            animatedPositions[i] = positions[i] * (1 + Mathf.Sin(tOffset) * 0.05f);
        }
        lineRenderer.SetPositions(animatedPositions);
    }
}