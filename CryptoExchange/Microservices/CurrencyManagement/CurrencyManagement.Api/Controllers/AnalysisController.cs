using AutoMapper;
using CurrencyManagement.Core.Dtos.Analys;
using CurrencyManagement.Core.Services;
using CurrencyManagement.Core.Dtos.CurrencyDimension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CurrencyManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        IMapper _mapper;

        public AnalysisController(IAnalysisService analysisService, IMapper mapper)
        {
            _analysisService = analysisService;
            _mapper = mapper;
        }

        [HttpPost("insert-data-to-big-query")]
        public async Task<IActionResult> InsertData()
        {
            await _analysisService.InsertDataToBigQuery();

            return Ok("Data inserted!");
        }      
        
        [HttpGet("get-all-dimensions")]
        public async Task<IActionResult> GetAllDimensions()
        {
            var dimensions = await _analysisService.GetAllDimensions();

            return Ok(_mapper.Map<IEnumerable<CurrencyDimensionDto>>(dimensions));
        }

        [HttpGet("get-price-different-between-first-and-last-dates")]
        public async Task<IActionResult> GetDefferentBetweenFirstAndLastDates(
            [FromQuery] int currencyId)
        {
            var result = await _analysisService.GetDifferentBetweenFirstAndLastDate(currencyId);
            return Ok(result);
        }

        [HttpPut("get-price-different-between-different-dates")]
        public async Task<IActionResult> GetDefferentDifferentDates(
            [FromBody] FromAndEndDatesDto datesDto)
        {
            var result = await _analysisService.GetDifferentBetweenDifferentDates(
                datesDto.CurrencyId,
                datesDto.FromDate,
                datesDto.EndDate
                );
           
            return Ok(result);
        }
    }
}
