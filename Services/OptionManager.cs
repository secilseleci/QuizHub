using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class OptionManager : IOptionService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public OptionManager(IRepositoryManager manager,
        IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        public void CreateOneOption(OptionDto optionDto)
        {
            Option option=_mapper.Map<Option>(optionDto);
            _manager.Option.Create(option);
            _manager.Save();
        }

        public void DeleteOneOption(int id)
        {
            Option option =GetOneOption(id,false);
            if (option is not null)
            {
                _manager.Option.DeleteOneOption(option);
                _manager.Save();
            }
        }

        public IEnumerable<Option> GetAllOptions(bool trackChanges)
        {
            return _manager.Option.GetAllOptions(trackChanges);
        }

       

        public Option? GetOneOption(int id, bool trackChanges)
        {
            var option = _manager.Option.GetOneOption(id, trackChanges);
            if (option is null)
                throw new Exception("option not found!");
            return option;
        }


        public void UpdateOneOption(OptionDto optionDto)
        {
            if (optionDto == null)
            {
                throw new ArgumentNullException(nameof(optionDto), "Option data cannot be null.");
            }

            var entity = _mapper.Map<Option>(optionDto);
            if (entity == null)
            {
                throw new Exception("Mapping failed. Option entity is null.");
            }

            _manager.Option.UpdateOneOption(entity);
            _manager.Save();
        }

        public IEnumerable<Option> GetOptionsByQuestionId(int questionId, bool trackChanges)
        {
            return _manager.Option.GetOptionsByQuestionId(questionId,trackChanges);        
        }

        public Option? GetCorrectOptionForQuestion(int questionId, bool trackChanges)
        {
            return _manager.Option.GetCorrectOptionForQuestion(questionId, trackChanges);
                
        }
       
    }
}
