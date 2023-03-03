using System;
using System.Collections.Generic;
using CatCards.DAO;
using CatCards.Models;
using CatCards.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatCards.Controllers
{
    [Route("/api/cards/")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatCardDao cardDao;
        private readonly ICatFactService catFactService;
        private readonly ICatPicService catPicService;

        public CatController(ICatCardDao _cardDao, ICatFactService _catFact, ICatPicService _catPic)
        {
            catFactService = _catFact;
            catPicService = _catPic;
            cardDao = _cardDao;
        }
        [HttpGet]

        public ActionResult<List<CatCard>> GetAllCards()
        {
            List<CatCard> cards = cardDao.GetAllCards();
            if(cards != null)
            {
                return cards;
            }
            else
            {
                return NotFound();
            }

        }


        [HttpGet("{id}")]
        public ActionResult<CatCard> GetCard(int id)
        {
            CatCard card = cardDao.GetCard(id);
            if (card != null)
            {
                return Ok(card);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("random")]

        public ActionResult<CatCard> NewCard()
        {
            CatCard card = new CatCard();
            CatFact fact = catFactService.GetFact();
            card.CatFact = fact.Text;
            CatPic pic = catPicService.GetPic();
            card.ImgUrl = pic.File;
            //card.ImgUrl = Convert.ToString(catPicService.GetPic());

            return card;
        }
        [HttpPost]
        public ActionResult<CatCard> SaveCard(CatCard card)
        {
            CatCard saved = cardDao.SaveCard(card);

            return Created($"/api/cards/{saved.CatCardId}", saved);
        }

        [HttpPut("{id}")]
        public ActionResult<CatCard> UpdateCard(int id, CatCard card)
        {

            bool updated = cardDao.UpdateCard(card);

            if (!updated)
            {
                return NotFound();
            } else
            {
                return NoContent();
            }
           
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCard(int id)
        {
            bool deleted = cardDao.RemoveCard(id);

            if (!deleted)
            {
                return NotFound();
            } else if (deleted)
            {
                return NoContent();
            } else
            {
                return StatusCode(500);
            }
           
            
        }
        

    }
}
