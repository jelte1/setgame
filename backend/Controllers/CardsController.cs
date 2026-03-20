// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using AutoMapper;
// using backend.Interfaces;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using backend.DTOs.Card;
// using backend.Entities;
// using backend.Repositories;
// using Microsoft.AspNetCore.Authorization;
//
// namespace backend.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class CardsController : ControllerBase
//     {
//         // test
//         private readonly IMapper _mapper;
//         private readonly ICardsRepository _cardsRepository;
//
//         public CardsController(ICardsRepository cardsRepository, IMapper mapper)
//         {
//             _mapper = mapper;
//             _cardsRepository = cardsRepository;
//         }
//
//         // GET: api/Cards
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<GetCardDto>>> GetCards()
//         {
//             var cards = await _cardsRepository.GetAllAsync();
//             return Ok(_mapper.Map<List<GetCardDto>>(cards));
//         }
//
//         // GET: api/Card/1
//         [HttpGet("{id}")]
//         public async Task<ActionResult<GetCardDto>> GetCard(int id)
//         {
//             var card = await _cardsRepository.GetAsync(id);
//
//             if (card == null)
//             {
//                 return NotFound();
//             }
//
//             return Ok(_mapper.Map<GetCardDto>(card));
//         }
//
//
//         // POST: api/Card
//         [HttpPost]
//         [Authorize]
//         public async Task<ActionResult<Card>> PostCard(CreateCardDto createCard)
//         {
//             var card = _mapper.Map<Card>(createCard);
//
//             await _cardsRepository.AddAsync(card);
//
//             return CreatedAtAction("GetCard", new { id = card.Id }, card);
//         }
//
//         // DELETE: api/Card/5
//         [HttpDelete("{id}")]
//         [Authorize]
//         public async Task<IActionResult> DeleteCard(int id)
//         {
//             var card = await _cardsRepository.GetAsync(id);
//             if (card == null)
//             {
//                 return NotFound();
//             }
//
//             await _cardsRepository.DeleteAsync(id);
//
//             return NoContent();
//         }
//         // // GET: api/Card/ids/1,2,3
//         // [HttpGet("ids/{ids}")]
//         // public async Task<IActionResult> GetCardsByIds(List<int> ids)
//         // {
//         //     var cards = await _cardsRepository.GetCardsByIds(ids);
//         //     if (cards == null || cards.Count == 0)
//         //     {
//         //         return NotFound();
//         //     }
//         //
//         //     return Ok(_mapper.Map<List<GetCardDto>>(cards));
//         // }
//
//         private async Task<bool> CardExists(int id)
//         {
//             return await _cardsRepository.Exists(id);
//         }
//     }
// }