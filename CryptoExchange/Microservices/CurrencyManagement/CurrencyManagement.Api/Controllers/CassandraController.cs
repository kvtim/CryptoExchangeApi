using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CurrencyManagement.Core.Services;
using CurrencyManagement.Core.Dtos.CurrencyDimension;
using CurrencyManagement.Core.Dtos.Analys;

namespace CurrencyManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CassandraController : ControllerBase
    {
        private readonly ICassandraService _cassandraService;
        private readonly IMapper _mapper;

        public CassandraController(ICassandraService cassandraService, IMapper mapper)
        {
            _cassandraService = cassandraService;
            _mapper = mapper;
        }

        [HttpPost("insert-data-to-cassandra")]
        public async Task<IActionResult> InsertData()
        {
            await _cassandraService.InsertAll();

            return Ok("Data inserted!");
        }      
        
        [HttpGet("get-all-dimensions")]
        public async Task<IActionResult> GetAllDimensions()
        {
            var dimensions = await _cassandraService.GetAll();

            return Ok(_mapper.Map<IEnumerable<CurrencyDimensionDto>>(dimensions));
        }
        
        [HttpGet("get-dimension-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dimension = await _cassandraService.GetById(id);

            return Ok(_mapper.Map<CurrencyDimensionDto>(dimension));
        }

        [HttpPut("get-dimensions-between-dates")]
        public async Task<IActionResult> GetDimensionsBetweenDates(
        [FromBody] TwoDatesDto datesDto)
        {
            var result = await _cassandraService.GetDimensionsBetweenDates(
                datesDto.FromDate,
                datesDto.EndDate
                );

            return Ok(result);
        }
    }
}
