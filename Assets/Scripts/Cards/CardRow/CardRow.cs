using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LNE.Cards
{
  public class CardRow : MonoBehaviour
  {
    [SerializeField]
    private CardBase _cardBasePrefab;

    [SerializeField]
    private CardVisualObject _cardVisualObjectPrefab;

    [SerializeField]
    private Transform _cardBasesContainer;

    [SerializeField]
    private Transform _cardVisualObjectsContainer;

    private readonly List<CardBase> _cardBaseList = new();

    private CardVisualObject _movingCardVisualObject;

    private void Start()
    {
      // TODO: Replace this with deck initialization
      for (int i = 0; i < 5; i++)
      {
        CardBase cardBase = Instantiate(
          _cardBasePrefab,
          transform,
          _cardBasesContainer
        );
        _cardBaseList.Add(cardBase);

        cardBase.name = $"Card Base {i}";
        cardBase.Index = i;
        cardBase.transform.SetParent(_cardBasesContainer);
        cardBase.transform.localScale = Vector3.one;

        CardVisualObject cardVisualObject = Instantiate(
          _cardVisualObjectPrefab,
          transform,
          _cardVisualObjectsContainer
        );

        cardVisualObject.name = $"Card Visual Object {i}";
        cardVisualObject.transform.SetParent(_cardVisualObjectsContainer);
        cardVisualObject.transform.localScale = Vector3.one;
        cardVisualObject.CardBase = cardBase;

        cardVisualObject.CardBeginDrag += OnCardBeginDrag;
      }

      LayoutRebuilder.ForceRebuildLayoutImmediate(
        GetComponent<RectTransform>()
      );
    }

    private void Update()
    {
      if (_movingCardVisualObject)
      {
        SortCards();
      }
    }

    private void OnCardBeginDrag(CardVisualObject cardVisualObject)
    {
      _movingCardVisualObject = cardVisualObject;
    }

    private void SortCards()
    {
      for (int i = 0; i < _cardBaseList.Count; i++)
      {
        if (_cardBaseList[i] == _movingCardVisualObject.CardBase)
        {
          continue;
        }

        if (
          _movingCardVisualObject.TargetPosition.x
            > _cardBaseList[i].transform.position.x
          && _movingCardVisualObject.CardBase.Index < _cardBaseList[i].Index
        )
        {
          SwapCards(_movingCardVisualObject.CardBase, _cardBaseList[i]);
        }

        if (
          _movingCardVisualObject.TargetPosition.x
            < _cardBaseList[i].transform.position.x
          && _movingCardVisualObject.CardBase.Index > _cardBaseList[i].Index
        )
        {
          SwapCards(_movingCardVisualObject.CardBase, _cardBaseList[i]);
        }
      }
    }

    private static void SwapCards(CardBase cardBase1, CardBase cardBase2)
    {
      (cardBase2.Index, cardBase1.Index) = (cardBase1.Index, cardBase2.Index);
      cardBase1.transform.SetSiblingIndex(cardBase1.Index);
      cardBase2.transform.SetSiblingIndex(cardBase2.Index);
    }
  }
}
