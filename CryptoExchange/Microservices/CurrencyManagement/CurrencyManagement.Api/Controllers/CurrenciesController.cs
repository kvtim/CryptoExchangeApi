using AutoMapper;
using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Repositories;
using CurrencyManagement.Data.Dtos.Currency;
using CurrencyManagement.Data.Services;
using Microsoft.AspNetCore.Mvc;


namespace CurrencyManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public CurrenciesController(ICurrencyService currencyService, IMapper mapper)
        {
            _currencyService = currencyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _currencyService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CurrencyDto>>(currencies));
        }
        
        [HttpGet("AllWithDimension")]
        public async Task<IActionResult> GetAllWithDimension()
        {
            var currencies = await _currencyService.GetAllWithDimensionAsync();
            return Ok(_mapper.Map<IEnumerable<CurrencyWithDimensionDto>>(currencies));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var currency = await _currencyService.GetByIdAsync(id);

            return currency == null ? NotFound() : Ok(_mapper.Map<CurrencyDto>(currency));
        }

        [HttpGet("CurrencyWithDimension/{id}")]
        public async Task<IActionResult> GetByIdlWithDimension(int id)
        {
            var currency = await _currencyService.GetByIdlWithDimensionAsync(id);

            return currency == null ? NotFound() 
                : Ok(_mapper.Map<CurrencyWithDimensionDto>(currency));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCurrencyDto createCurrencyDto)
        {
            var currency = _mapper.Map<Currency>(createCurrencyDto);

            var currencyResult = await _currencyService.AddAsync(currency);

            return currency == null ? BadRequest() : Ok(_mapper.Map<CurrencyDto>(currencyResult));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyDto updateCurrencyDto)
        {
            var currency = _mapper.Map<Currency>(updateCurrencyDto);
            currency.Id = id;

            await _currencyService.UpdateAsync(currency);

            return Ok(updateCurrencyDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currency = await _currencyService.GetByIdAsync(id);

            if (currency == null) return BadRequest("Currency not found");

            await _currencyService.RemoveAsync(currency);
            return Ok();
        }
    }
}
