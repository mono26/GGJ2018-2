using UnityEngine;

public static class VisualEffects
{
    public static void CreateVisualEffect(GameObject _effect, Transform _targetTransform)
    {
        if (_effect != null)
        {
            GameObject.Instantiate(_effect, _targetTransform.position, _targetTransform.rotation);
        }
        return;
    }
}
