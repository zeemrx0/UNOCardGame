using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public Vector3 TargetPosition { get; set; }

    [SerializeField]
    private float _speed = 12f;

    private bool _isDragging;

    private void Start()
    {
      TargetPosition = transform.position;
    }

    private void Update()
    {
      if (_isDragging)
      {
        LerpToTargetPosition(TargetPosition, _speed);
      }
      else
      {
        LerpToTargetPosition(CardBase.transform.position, _speed);
      }
    }

    public void LerpToTargetPosition(Vector3 targetPosition, float speed)
    {
      transform.position = Vector3.Lerp(
        transform.position,
        targetPosition,
        speed * Time.deltaTime
      );
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
