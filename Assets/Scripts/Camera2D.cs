using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera2D : MonoBehaviour
{
    [System.Serializable]
    public struct SubCameraInfo
    {
        public string name;
        public bool valid;
        public LayerMask cullingMask;
        public int depth;

        [Range(0, 1)]
        public float followRateX;
        [Range(0, 1)]
        public float followRateY;
    }

    [Range(0, 1)]
    [SerializeField]
    private float _followLerpRate = 1.0f;

    [SerializeField]
    private GameObject _subCameraPrefab;

    [SerializeField]
    private SubCameraInfo[] _subCameraInfoList;

    private GameObject _target;
    private Camera _camera;
    private bool _hasBounds = false;
    private Bounds _bounds;

    private Dictionary<Camera, SubCameraInfo> _cameraDict;
    private bool _positionInited = false;
    private Vector3 _initPosition;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        _cameraDict = new Dictionary<Camera, SubCameraInfo>();

        foreach (var info in _subCameraInfoList)
        {
            CreateSubCamera(info);
        }
    }

    public void CreateSubCamera(SubCameraInfo info)
    {
        if (!info.valid) return;

        // 创建子摄像机;
        var go = Instantiate(_subCameraPrefab, transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = transform.parent;
        go.name = "SubCamera - " + info.name;

        // 初始化值;
        var camera = go.GetComponent<Camera>();
        camera.orthographicSize = _camera.orthographicSize;
        camera.cullingMask = info.cullingMask;
        camera.depth = info.depth;

        // 加入dict;
        _cameraDict.Add(camera, info);

        // 主摄像机不渲染这部分layer;
        _camera.cullingMask &= ~info.cullingMask;
    }

    public void SetTarget(GameObject go)
    {
        _target = go;
    }

    public void SetBounds(Bounds bounds)
    {
        _hasBounds = true;
        _bounds = bounds;
    }

    void LateUpdate()
    {
        if (_target)
        {
            var half_height = _camera.orthographicSize;
            var half_width = half_height * Screen.width / Screen.height;

            float targetX;
            float targetY;
            if (!_positionInited)
            {
                // 第一次直接切镜头;
                targetX = _target.transform.position.x;
                targetY = _target.transform.position.y;
            }
            else
            {
                targetX = Mathf.Lerp(transform.position.x, _target.transform.position.x, _followLerpRate);
                targetY = Mathf.Lerp(transform.position.y, _target.transform.position.y, _followLerpRate);
            }
            var pos = new Vector3(targetX, targetY, transform.position.z);

            // 判定边界;
            if (_hasBounds)
            {
                if (pos.x + half_width > _bounds.max.x)
                {
                    pos.x = _bounds.max.x - half_width;
                }
                if (pos.x - half_width < _bounds.min.x)
                {
                    pos.x = _bounds.min.x + half_width;
                }
                if (pos.y + half_height > _bounds.max.y)
                {
                    pos.y = _bounds.max.y - half_height;
                }
                if (pos.y - half_height < _bounds.min.y)
                {
                    pos.y = _bounds.min.y + half_height;
                }
            }

            transform.position = pos;

            if (!_positionInited)
            {
                _initPosition = transform.position;
                _positionInited = true;
            }

            // 更新子摄像机;
            foreach (var kv in _cameraDict)
            {
                var camera = kv.Key;
                var d = transform.position - _initPosition;
                d.x *= kv.Value.followRateX;
                d.y *= kv.Value.followRateY;
                camera.transform.position = _initPosition + d;
            }
        }
    }
}
