using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LNE.Cards
{
  public class CardVisualObject
    : MonoBehaviour,
      IDragHandler,
      IBeginDragHandler,
      IEndDragHandler
  {
    public event Action<Vector3> CardDragging;
    public event Action<CardVisualObject> CardBeginDrag;
    public CardBase CardBase { get; set; }
    public Vector3 TargetPosition { get; private set; }

    [SerializeField]
    private float _speed = 12f;

    [SerializeField]
    private float _tiltSpeed = 60f;

    [SerializeField]
    private float _tiltDistanceThreshold = 0.1f;

    [SerializeField]
    private float _tiltMaxDistance = 160f;

    private bool _isDragging;

    private void Start()
    {
      TargetPosition = transform.position;
    }

    private void Update()
    {
      LerpTowardTargetPosition(_isDragging ? TargetPosition : CardBase.transform.position, _speed);
    }

    private void LerpTowardTargetPosition(Vector3 targetPosition, float speed)
    {
      // * Lerp position
      transform.position = Vector3.Lerp(
        transform.position,
        targetPosition,
        speed * Time.deltaTime
      );

      Vector3 direction = targetPosition - transform.position;

      // * Lerp rotation to tilt card based on distance
      if (direction.magnitude > _tiltDistanceThreshold)
      {
        float angle = Vector3.SignedAngle(
          Vector3.up,
          direction,
          Vector3.forward
        );

        angle = angle * direction.magnitude / _tiltMaxDistance;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(
          transform.rotation,
          rotation,
          _tiltSpeed * Time.deltaTime
        );
      }
      else
      {
        transform.rotation = Quaternion.identity;
      }
    }

    public void OnDrag(PointerEventData eventData)
    {
      TargetPosition = eventData.position;
      CardDragging?.Invoke(TargetPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      _isDragging = true;
      CardBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      _isDragging = false;
    }
  }
}