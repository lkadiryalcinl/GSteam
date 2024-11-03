using AutoMapper;
using BasketService.Base;
using BasketService.Models;
using Contracts;
using MassTransit;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Security.Claims;

namespace BasketService.Repository
{
    public class BasketRepository : IBasketRepository
    {
        readonly IConfiguration _configuration;
        private static string? connectionString;
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string UserId;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketRepository(IConfiguration configuration, IHttpContextAccessor contextAccessor, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _configuration = configuration;
            connectionString = _configuration.GetValue<string>("RedisDatabase")??"localhost";
            ConnectionMultiplexer _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            _database = _connectionMultiplexer.GetDatabase();
            _contextAccessor = contextAccessor;
            UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async Task<ResponseModel<bool>> AddBasket(BasketModel model)
        {
            ResponseModel<bool> result = new();
            if(model is not null)
            {
                var converType = JsonConvert.SerializeObject(model);
                await _database.ListRightPushAsync(UserId, converType);
                result.IsSuccess = true;
                return result;
            }
            return result;
        }

        public async Task<ResponseModel<BasketModel>> GetBasketItem(long index)
        {
            ResponseModel<BasketModel> model = new();
            var res = await _database.ListGetByIndexAsync(UserId, index);
            var objRes = JsonConvert.DeserializeObject<BasketModel>(res);
            model.Data = objRes;
            model.IsSuccess = true;
            return model;
        }

        public async Task<ResponseModel<List<BasketModel>>> GetBasketItems()
        {
            ResponseModel<List<BasketModel>> model = new();
            var res = await _database.ListRangeAsync(UserId);
            List<BasketModel> basketModels = [];

            foreach (var item in res) 
            {
                var objRes = JsonConvert.DeserializeObject<BasketModel>(item);
                basketModels.Add(objRes);
            }

            if(basketModels.Count > 0)
            {
                model.Data = basketModels;
                model.IsSuccess = true;
                return model;
            }

            model.IsSuccess = false;
            return model;
        }

        public async Task<ResponseModel<bool>> RemoveBasketItem(long index)
        {
            ResponseModel<bool> model = new();
            var item = await _database.ListGetByIndexAsync(UserId, index);
            await _database.ListRemoveAsync(UserId, item);
            model.IsSuccess = true;
            return model;
        }

        public async Task<ResponseModel<BasketModel>> UpdateBasketItem(BasketModel basketmodel, long index)
        {
            ResponseModel<BasketModel> model = new();
            var objRes = JsonConvert.SerializeObject(basketmodel);
            await _database.ListSetByIndexAsync(UserId, index,objRes);
            model.IsSuccess=true;
            return model;
        }

        public async Task<ResponseModel<bool>> Checkout()
        {
            List<CheckoutModel> checkouts = [];
            ResponseModel<bool> responseModel = new();
            var response = await _database.ListRangeAsync(UserId);
            foreach (var item in response)
            {
                CheckoutModel _checkout = new();
                var objResult = JsonConvert.DeserializeObject<BasketModel>(item);
                _mapper.Map(objResult, _checkout);
                _checkout.UserId = Guid.Parse(UserId);
                checkouts.Add(_checkout);
            }
            if (checkouts.Count > 0)
            {
                responseModel.IsSuccess = true;
                foreach (var item in checkouts)
                {
                    try
                    {
                        await _publishEndpoint.Publish(_mapper.Map<CheckoutBasketModel>(item));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                await _database.KeyDeleteAsync(UserId);

                return responseModel;
            }
            responseModel.IsSuccess = false;
            return responseModel;
        }

    }
}
