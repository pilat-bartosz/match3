using UnityEngine;

public class Item : MonoBehaviour
{
    private GameManager _gm;
    public int _type;
    [SerializeField] private float _scale = 0.5f;
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _animationSpeed = 1f;
    private float _smallScale;

    private Vector3 _destination;
    private bool _isAnimated;
    private float _animationTime;

    private void Start()
    {
        _smallScale = _scale * .8f;
        transform.localScale = new Vector3(_scale, _scale, 1f);


        _gm = FindObjectOfType<GameManager>();
        if (!_gm) Debug.LogError("GameManager not found");
    }

    public void TeleportToPosition(Vector3 vec)
    {
        transform.position = vec;
        _destination = vec;
    }

    public void MoveAtPosition(Vector3 vec)
    {
        _destination = vec;
    }

    public void ToggleActual()
    {
        _isAnimated = !_isAnimated;
    }

    private void Update()
    {
        if (transform.position != _destination)
            transform.position = Vector3.MoveTowards(transform.position, _destination, _moveSpeed * Time.deltaTime);

        _animationTime = _isAnimated ? (_animationTime + _animationSpeed * Time.deltaTime) % 1 : 0;
        var currentScale = Mathf.Lerp(_smallScale, _scale, Mathf.Sin(_animationTime * Mathf.PI));
        transform.localScale = new Vector3(currentScale, currentScale, 1f);
    }

    private void OnMouseDown()
    {
        _gm.ItemClicked(this);
    }
}