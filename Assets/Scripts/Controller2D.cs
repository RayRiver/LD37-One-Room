using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    [Range(0.001f, 0.499f)]
    [SerializeField]
    private float _skinWidth = 0.015f;

    [Range(2, 16)]
    [SerializeField]
    private int _horizontalRayCount = 4;

    [Range(2, 16)]
    [SerializeField]
    private int _verticalRayCount = 4;

    [SerializeField]
    private LayerMask _collisionMask;

    private struct RaycastOrigins
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;
        public void Reset()
        {
            above = below = left = right = false;
        }
    }

    private CollisionInfo _collisions;
    public CollisionInfo Collisions
    {
        get
        {
            return _collisions;
        }
    }

    private BoxCollider2D _collider;
    private RaycastOrigins _raycastOrigins;
    private Bounds _bounds;

    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _collisions.Reset();
        CalculateRaySpacing();
    }

    void UpdateBounds()
    {
        _bounds = _collider.bounds;
        _bounds.Expand(_skinWidth * -2);
    }

    void UpdateRaycastOrigins()
    {
        UpdateBounds();
        _raycastOrigins.bottomLeft = new Vector2(_bounds.min.x, _bounds.min.y);
        _raycastOrigins.bottomRight = new Vector2(_bounds.max.x, _bounds.min.y);
        _raycastOrigins.topLeft = new Vector2(_bounds.min.x, _bounds.max.y);
        _raycastOrigins.topRight = new Vector2(_bounds.max.x, _bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        UpdateBounds();
        _horizontalRaySpacing = _bounds.size.y / (_horizontalRayCount - 1);
        _verticalRaySpacing = _bounds.size.x / (_verticalRayCount - 1);
    }

    void DrawBody()
    {
        Debug.DrawLine(_raycastOrigins.topLeft, _raycastOrigins.topRight);
        Debug.DrawLine(_raycastOrigins.topRight, _raycastOrigins.bottomRight);
        Debug.DrawLine(_raycastOrigins.bottomRight, _raycastOrigins.bottomLeft);
        Debug.DrawLine(_raycastOrigins.bottomLeft, _raycastOrigins.topLeft);
    }

    public void Move(Vector3 move)
    {
        UpdateRaycastOrigins();

        DrawBody();

        _collisions.Reset();

        if (move.y != 0)
        {
            VerticalCollisions(ref move);
        }

        if (move.x != 0)
        {
            HorizontalCollisions(ref move);
        }

        transform.Translate(move);
    }

    void HorizontalCollisions(ref Vector3 move)
    {
        float directionX = Mathf.Sign(move.x);
        float rayLength = Mathf.Abs(move.x) + _skinWidth;

        for (var i = 0; i < _horizontalRayCount; ++i)
        {
            Vector2 origin = (directionX > 0) ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
            origin += Vector2.up * i * _horizontalRaySpacing;
            Vector2 dir = Vector2.right * directionX;

            Debug.DrawRay(origin, dir * rayLength, Color.red);

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, rayLength, _collisionMask);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    continue;
                }

                if (hit && hit.distance < rayLength)
                {
                    move.x = (hit.distance - _skinWidth) * directionX;
                    rayLength = hit.distance;

                    _collisions.left = (directionX < 0);
                    _collisions.right = (directionX > 0);
                }
            }

        }
    }

    void VerticalCollisions(ref Vector3 move)
    {
        float directionY = Mathf.Sign(move.y);
        float rayLength = Mathf.Abs(move.y) + _skinWidth;

        for (var i = 0; i < _verticalRayCount; ++i)
        {
            Vector2 origin = (directionY > 0) ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;
            origin += Vector2.right * i * _verticalRaySpacing;
            Vector2 dir = Vector2.up * directionY;

            Debug.DrawRay(origin, dir * rayLength, Color.red);

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, rayLength, _collisionMask);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    continue;
                }

                if (hit && hit.distance < rayLength)
                {
                    move.y = (hit.distance - _skinWidth) * directionY;
                    rayLength = hit.distance;

                    _collisions.below = (directionY < 0);
                    _collisions.above = (directionY > 0);
                }
            }
        }
    }
}
