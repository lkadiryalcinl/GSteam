using AutoMapper;
using GameService.Base;
using GameService.Data;
using GameService.DTOs;
using GameService.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameService.Repositories.ForCategory
{
    public class CategoryRepository(GameDbContext context, IMapper mapper,BaseResponseModel baseResponseModel) : ICategoryRepository
    {
        private readonly GameDbContext _context = context;
        private IMapper _mapper = mapper;
        private BaseResponseModel _responseModel = baseResponseModel;

        public async Task<BaseResponseModel> CreateCategory(CategoryDTO category)
        {
            var objDTO = _mapper.Map<Category>(category);
            await _context.Categories.AddAsync(objDTO);

            if (await _context.SaveChangesAsync() > 0)
            {
                _responseModel.IsSuccess = true;
                _responseModel.Message = "SuccessFull";
                _responseModel.Data = objDTO;
                return _responseModel;
            }

            _responseModel.IsSuccess = false;
            return _responseModel;

        }

        public async Task<BaseResponseModel> GetAllCategories()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            if (categories is not null)
            {
                _responseModel.IsSuccess = true;
                _responseModel.Data = categories;
                return _responseModel;
            }
            _responseModel.IsSuccess = false;
            return _responseModel;
        }

        public async Task<bool> RemoveCategory(Guid categoryId)
        {
            Category category = await _context.Categories.FindAsync(categoryId);
            if (category is not null)
            {
                _context.Categories.Remove(category);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<BaseResponseModel> UpdateCategory(CategoryDTO category, Guid categoryId)
        {
            Category? obj = await _context.Categories.FindAsync(categoryId);
            if (obj is not null)
            {
                obj.Description = category.Description;
                obj.Name = category.Name;
                if (await _context.SaveChangesAsync() > 0)
                {
                    _responseModel.Data = category;
                    _responseModel.IsSuccess = true;
                    _responseModel.Message = "Success";
                    return _responseModel;
                }

            }

            _responseModel.IsSuccess = false;
            return _responseModel;
        }
    }
}
