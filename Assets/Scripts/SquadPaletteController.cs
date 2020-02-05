using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SquadPaletteController : MonoBehaviour
{
    private List<SquadCardButton> cards;

    // Start is called before the first frame update

    private SquadCardButton currentCard;

    private SquadGhost ghost;


    public PlayerProfile player => GameManager.instance.user;

    void Awake()
    {
        ghost = FindObjectOfType<SquadGhost>();
        ghost.gameObject.SetActive(false);
        cards = new List<SquadCardButton>(GetComponentsInChildren<SquadCardButton>());

        foreach (SquadCardButton button in cards)
        {
            button.OnSelected += OnSelected;
            button.OnDeselected += OnDeselected;
            button.OnActivate += OnActivate;
        }
    }


    public void OnGameStart()
    {
        foreach (SquadCardButton button in cards)
        {
            button.OnGameStart();
            button.show(GameManager.instance.config.GenerateSquad());
        }
    }

    public void OnGameEnd()
    {
        currentCard = null;
        foreach (SquadCardButton button in cards)
        {
            button.OnGameEnd();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (currentCard)
        {
            ghost.gameObject.SetActive(currentCard.active);
            ghost.transform.position = new Vector3(currentCard.aimPoint.x, ghost.transform.position.y);

            ghost.cardAvailable = currentCard.Available;
            ghost.SetSquad(currentCard.squad);
            
        }
        else
        {
            ghost.Clear();
            ghost.gameObject.SetActive(false);
        }
    }


    private void OnSelected(SquadCardButton card)
    {
        if (currentCard)
        {
            OnDeselected(currentCard);
        }

        currentCard = card;
        if (currentCard)
        {
        }
    }

    private void OnDeselected(SquadCardButton card)
    {
        if (currentCard && currentCard == card)
        {
            if (currentCard.active && CanDrop(currentCard.squad) && ghost.dropPossible)
            {
                DropSquad();
                currentCard.show(GameManager.instance.config.GenerateSquad());
            }

            currentCard = null;
        }
    }


    private void DropSquad()
    {
        GameManager.instance.user.stamina -= currentCard.squad.items.Count;
        GameManager.instance.Spawn(currentCard.squad, player.id, ghost.position);
    }

    public bool CanDrop(Squad squad)
    {
        return player.stamina >= squad.items.Count;
    }

    private void OnActivate(SquadCardButton obj)
    {
        //drop squad
    }
}