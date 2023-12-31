﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.DTO;
using motel.Repositories;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ITierRepositories _tierRepository;

        public TiersController(AppDbContext dbContext, ITierRepositories
tierRepository)
        {
            _dbContext = dbContext;
            _tierRepository = tierRepository;
        }
        [HttpGet("get-all-tier")]
        public IActionResult GetAllTier([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var allAuthors = _tierRepository.GetlAllTier();
            return Ok(allAuthors);
        }
        [HttpGet("get-tier-id")]
        public IActionResult GetTierById(int id)
        {
            var tierWithId = _tierRepository.GetTierById(id);
            return Ok(tierWithId);
        }
        [HttpPost("add-tier")]
        public IActionResult AddTier([FromBody] AddTiersDTO
       addTierDTO)
        {
            var tierAdd = _tierRepository.AddTier(addTierDTO);
            return Ok();
        }
        [HttpPut("update-tier-id")]
        public IActionResult UpdateTierById(int id, [FromBody] AddTiersDTO TierDTO)
        {
            var tierUpdate = _tierRepository.UpdateTierById(id, TierDTO);
            return Ok(tierUpdate);
        }
        [HttpDelete("delete-tier-id")]
        public IActionResult DeleteBookById(int id)
        {
            var tierDelete = _tierRepository.DeleteTierById(id);
            return Ok();
        }

    }
}
