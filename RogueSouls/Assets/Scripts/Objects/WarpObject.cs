using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpObject : MonoBehaviour
{
    Transform _warpTarget;

    Rigidbody2D _warpTargetRb;

    [SerializeField]
    Transform _warpPosition;

    GameObject _warpOverlay;

    [SerializeField]
    Vector3 _defaultWarpOverlaySize, _targetWarpOverlaySize;

    [SerializeField]
    float _warpInTransitionSpeed, _warpOutTransitionSpeed, _warpCooldownLength, _stairUseCooldownTime;

    private void Awake()
    {
        _warpOverlay = GameObject.FindGameObjectWithTag("WarpOverlay");
        _warpOverlay.transform.localScale = _defaultWarpOverlaySize;
    }

    private void Start()
    {
        _warpOverlay?.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            _warpTargetRb = other.gameObject.GetComponent<Rigidbody2D>();
            _warpTarget = other.transform;
            if(_warpPosition.GetComponent<BoxCollider2D>())
            {
                _warpPosition.GetComponent<BoxCollider2D>().enabled = false;
            }
            StartCoroutine(BeginWarpSequence());
            Invoke("TriggerReturnTransition", _warpCooldownLength);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Invoke("ResetStairs", _stairUseCooldownTime);
    }

    IEnumerator BeginWarpSequence()
    {
        _warpOverlay.transform.localScale = _defaultWarpOverlaySize;
        _warpOverlay.SetActive(true);
        _warpTargetRb.velocity = Vector3.zero;
        while (true)
        {
            _warpOverlay.transform.localScale = Vector3.Lerp(_warpOverlay.transform.localScale, _targetWarpOverlaySize, _warpInTransitionSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ReturnWarpSequence()
    {
        _warpOverlay.transform.localScale = _targetWarpOverlaySize;
        _warpTarget.position = _warpPosition.position;
        while (true)
        {
            _warpOverlay.transform.localScale = Vector3.Lerp(_warpOverlay.transform.localScale, _defaultWarpOverlaySize, _warpOutTransitionSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    void TriggerReturnTransition()
    {
        StartCoroutine(ReturnWarpSequence());
    }

    void ResetStairs()
    {
        _warpOverlay.SetActive(false);
        if (_warpPosition.GetComponent<BoxCollider2D>())
        {
            _warpPosition.GetComponent<BoxCollider2D>().enabled = true;
        }
        StopAllCoroutines();
    }
}