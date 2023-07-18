using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.UnitOfWork;
using CurrencyManagement.Core.Services;
using CurrencyManagement.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyManagement.Data.Exceptions;
using CurrencyManagement.Core.BiqQuery;
using MassTransit;
using EventBus.Messages.Events;
using Google.Apis.Bigquery.v2.Data;
using System.Text.Json;
using EventBus.Messages.Common;

namespace CurrencyManagement.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBigQuery _bigQuery;
        private readonly IKafkaProducerService _kafkaProducerService;

        public CurrencyService(
            IUnitOfWork unitOfWork,
            IBigQuery bigQuery,
            IKafkaProducerService kafkaProducerService)
        {
            _unitOfWork = unitOfWork;
            _bigQuery = bigQuery;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Currency> AddAsync(Currency entity)
        {
            var currency = await _unitOfWork.CurrencyRepository.GetByIdAsync(entity.Id);

            if (currency != null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.CurrencyLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Currency",
                        LogType = "Exception",
                        Message = $"Currency {entity.Id} already exists.",
                        LogTime = DateTime.Now
                    }));

                throw new KeyNotFoundException($"Currency {entity.Id} already exists.");
            }

            entity.CurrencyDimensions = new List<CurrencyDimension>
            {
                new CurrencyDimension()
                {
                    PriceInUSD = entity.CurrentPriceInUSD,
                }
            };
            await _unitOfWork.CurrencyRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();


            var message = JsonSerializer.Serialize(new CreateNewLogEvent()
            {
                Microservice = "Currency",
                LogType = "Addition",
                Message = $"Currency {entity.Id} created.",
                LogTime = DateTime.Now
            });

            await _kafkaProducerService.SendMessage(
                TopicNamesConstants.CurrencyLogsTopic,
                JsonSerializer.Serialize(new CreateNewLogEvent()
                {
                    Microservice = "Currency",
                    LogType = "Addition",
                    Message = $"Currency {entity.Id} created.",
                    LogTime = DateTime.Now
                }));

            //await _bigQuery.InsertCurrencyDimension(entity.CurrencyDimensions.First());
            return entity;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();

            if (!currencies.Any())
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.CurrencyLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Currency",
                        LogType = "Exception",
                        Message = $"Currencies not found.",
                        LogTime = DateTime.Now
                    }));

                throw new KeyNotFoundException("Currencies not found");
            }

            return currencies;
        }

        public async Task<IEnumerable<Currency>> GetAllWithDimensionAsync()
        {
            var currencies = await _unitOfWork.CurrencyRepository.GetAllWithDimensionAsync();

            if (!currencies.Any())
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.CurrencyLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Currency",
                        LogType = "Exception",
                        Message = $"Currencies not found.",
                        LogTime = DateTime.Now
                    }));

                throw new KeyNotFoundException("Currencies not found");
            }

            return currencies;
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            var currency = await _unitOfWork.CurrencyRepository.GetByIdAsync(id);

            if (currency == null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.CurrencyLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Currency",
                        LogType = "Exception",
                        Message = $"Currency {id} not found.",
                        LogTime = DateTime.Now
                    }));

                throw new KeyNotFoundException($"Currency {id} not found.");
            }

            return currency;
        }

        public async Task<Currency> GetByIdlWithDimensionAsync(int id)
        {
            var currency = await _unitOfWork.CurrencyRepository.GetByIdWithDimensionAsync(id);

            if (currency == null)
            {
                await _kafkaProducerService.SendMessage(
                   TopicNamesConstants.CurrencyLogsTopic,
                   JsonSerializer.Serialize(new CreateNewLogEvent()
                   {
                       Microservice = "Currency",
                       LogType = "Exception",
                       Message = $"Currency {id} not found.",
                       LogTime = DateTime.Now
                   }));

                throw new KeyNotFoundException($"Currency {id} not found");
            }

            return currency;
        }

        public async Task RemoveAsync(int id)
        {
            var currency = await GetByIdAsync(id);

            await _unitOfWork.CurrencyRepository.RemoveAsync(currency);
            await _unitOfWork.CommitAsync();

            await _kafkaProducerService.SendMessage(
                TopicNamesConstants.CurrencyLogsTopic,
                JsonSerializer.Serialize(new CreateNewLogEvent()
                {
                    Microservice = "Currency",
                    LogType = "Deletion",
                    Message = $"Currency {currency.Id} deleted.",
                    LogTime = DateTime.Now
                }));
        }

        public async Task<Currency> UpdateAsync(Currency entity)
        {
            await _unitOfWork.CurrencyRepository.UpdateAsync(entity);

            await _AddDimension(entity);

            await _unitOfWork.CommitAsync();

            await _kafkaProducerService.SendMessage(
                   TopicNamesConstants.CurrencyLogsTopic,
                   JsonSerializer.Serialize(new CreateNewLogEvent()
                   {
                       Microservice = "Currency",
                       LogType = "Updation",
                       Message = $"Currency {entity.Id} updated.",
                       LogTime = DateTime.Now
                   }));

            return entity;
        }

        public async Task<Currency> UpdatePriceAsync(int id, decimal increasePrice)
        {
            var currency = await GetByIdAsync(id);

            if (currency == null)
            {
                await _kafkaProducerService.SendMessage(
                   TopicNamesConstants.CurrencyLogsTopic,
                   JsonSerializer.Serialize(new CreateNewLogEvent()
                   {
                       Microservice = "Currency",
                       LogType = "Exception",
                       Message = $"Currency {id} not found.",
                       LogTime = DateTime.Now
                   }));

                throw new KeyNotFoundException("Currency not found");
            }

            if (currency.CurrentPriceInUSD <= -increasePrice)
            {
                await _kafkaProducerService.SendMessage(
                   TopicNamesConstants.CurrencyLogsTopic,
                   JsonSerializer.Serialize(new CreateNewLogEvent()
                   {
                       Microservice = "Currency",
                       LogType = "Exception",
                       Message = $"Currency {id} price less than zero.",
                       LogTime = DateTime.Now
                   }));

                throw new CurrencyPriceLessThanZeroException(currency.Name);
            }

            currency.CurrentPriceInUSD += increasePrice;

            return await UpdateAsync(currency);
        }

        public async Task FillingData(int id)
        {
            Random random = new Random();

            for (int i = 0; i < 10000; i++)
            {
                await UpdatePriceAsync(
                    id,
                    (decimal)random.Next(-100, 100) / 100
                    );
            }
        }

        private async Task _AddDimension(Currency entity)
        {
            var updatedEntity = await _unitOfWork.CurrencyRepository
                .GetByIdWithDimensionAsync(entity.Id);

            if (updatedEntity == null)
            {
                await _kafkaProducerService.SendMessage(
                   TopicNamesConstants.CurrencyLogsTopic,
                   JsonSerializer.Serialize(new CreateNewLogEvent()
                   {
                       Microservice = "Currency",
                       LogType = "Exception",
                       Message = $"Currency {entity.Id} not found.",
                       LogTime = DateTime.Now
                   }));

                throw new KeyNotFoundException("Currency not found");
            }

            var lastDim = updatedEntity.CurrencyDimensions.First(c => c.IsCurrent == true);
            lastDim.EndDate = DateTime.Now;
            lastDim.IsCurrent = false;

            updatedEntity.CurrencyDimensions.Add(
                new CurrencyDimension()
                {
                    FromDate = DateTime.Now,
                    PriceInUSD = entity.CurrentPriceInUSD
                });
        }
    }
}
