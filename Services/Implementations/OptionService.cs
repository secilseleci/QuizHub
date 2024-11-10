using AutoMapper;
using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Implementations
{
    public class OptionService : IOptionService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public OptionService(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<ResultGeneric<Option>> CreateOneOption(OptionDto optionDto)
        {
            var option = _mapper.Map<Option>(optionDto);
            await _manager.Option.CreateOneOptionAsync(option);

            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Option>.Fail("Seçenek kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }

            return ResultGeneric<Option>.Ok(option);
        }

        public async Task<Result> DeleteOneOption(int id)
        {
            var option = await _manager.Option.GetOneOptionAsync(id, trackChanges: false);
            if (option == null)
            {
                return Result.Fail("Seçenek bulunamadı.", "Silmek istediğiniz seçenek mevcut değil.");
            }

            await _manager.Option.DeleteOneOptionAsync(option);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Seçenek silinemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }

            return Result.Ok();
        }

        public async Task<ResultGeneric<IEnumerable<Option>>> GetAllOptions(bool trackChanges)
        {
            var options = await _manager.Option.GetAllOptionsAsync(trackChanges);
            if (!options.Any())
            {
                return ResultGeneric<IEnumerable<Option>>.Fail("Seçenek bulunamadı.", "Henüz kayıtlı seçenek yok.");
            }

            return ResultGeneric<IEnumerable<Option>>.Ok(options);
        }

        public async Task<ResultGeneric<Option>> GetOneOption(int id, bool trackChanges)
        {
            var option = await _manager.Option.GetOneOptionAsync(id, trackChanges);
            if (option == null)
            {
                return ResultGeneric<Option>.Fail("Seçenek bulunamadı.", "Aradığınız seçenek mevcut değil.");
            }

            return ResultGeneric<Option>.Ok(option);
        }

        public async Task<ResultGeneric<Option>> UpdateOneOption(OptionDto optionDto)
        {
            var option = await _manager.Option.GetOneOptionAsync(optionDto.OptionId, trackChanges: true);
            if (option == null)
            {
                return ResultGeneric<Option>.Fail("Seçenek bulunamadı.", "Güncellemek istediğiniz seçenek mevcut değil.");
            }

            _mapper.Map(optionDto, option);

            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Option>.Fail("Seçenek güncellenemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }

            return ResultGeneric<Option>.Ok(option);
        }

        public async Task<ResultGeneric<IEnumerable<Option>>> GetOptionsByQuestionId(int questionId, bool trackChanges)
        {
            var options = await _manager.Option.GetOptionsByQuestionIdAsync(questionId, trackChanges);
            if (!options.Any())
            {
                return ResultGeneric<IEnumerable<Option>>.Fail("Seçenek bulunamadı.", "Bu soru ile ilişkili seçenek mevcut değil.");
            }

            return ResultGeneric<IEnumerable<Option>>.Ok(options);
        }

        public async Task<ResultGeneric<Option>> GetCorrectOptionForQuestion(int questionId, bool trackChanges)
        {
            var option = await _manager.Option.GetCorrectOptionForQuestionAsync(questionId, trackChanges);
            if (option == null)
            {
                return ResultGeneric<Option>.Fail("Doğru seçenek bulunamadı.", "Bu soru için doğru seçenek mevcut değil.");
            }

            return ResultGeneric<Option>.Ok(option);
        }
    }
}
