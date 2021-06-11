using UnityEngine;

public class MenuBehavior : MonoBehaviour
{
    private float _targetPosX = 0;
    public float step;
    public void show()
    {
        _targetPosX = 100;
    }

    public void hide()
    {
        _targetPosX = 0;
    }

    private void Update()
    {
        float error = _targetPosX - transform.localPosition.x;
        if (System.Math.Abs(error) <= step)
        {
            transform.localPosition = new Vector3(_targetPosX, 0f);
            return;
        }
        float movement = Time.deltaTime * step * System.Math.Sign(_targetPosX - transform.localPosition.x);
        transform.Translate(new Vector3(movement, 0f));
    }
}
