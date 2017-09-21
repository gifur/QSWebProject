using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.CommentAggregate;
using QS.Core.Module.SharedAggregate;
using QS.DTO.CommentModule;
using QS.DTO.SharedModule;

namespace QS.Service.Effection
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ISuggestionRepository _suggestionRepository;

        public SuggestionService() { }

        public SuggestionService(ISuggestionRepository suggestionRepository)
        {
            _suggestionRepository = suggestionRepository;
        }
        public void AddSuggestion(SuggestionDto suggestionDto)
        {
            suggestionDto.CreateTime = DateTime.Now;
            _suggestionRepository.Add(QsMapper.CreateMap<SuggestionDto, Suggestion>(suggestionDto));
            _suggestionRepository.UnitOfWork.Commit();
        }

        public void DeleteSuggestion(Int64 suggestionId)
        {
            var temp = _suggestionRepository.Get(suggestionId);
            if (temp != null)
            {
                _suggestionRepository.Remove(temp);
                _suggestionRepository.UnitOfWork.Commit();
            }
        }

        public SuggestionDto GetSuggestionById(Int64 suggestionId)
        {
            var temp = _suggestionRepository.Get(suggestionId);
            return temp == null ? null : (QsMapper.CreateMap<Suggestion, SuggestionDto>(temp));
        }

        public IEnumerable<SuggestionDto> GetAllSuggestions()
        {
            var allSuggestion = _suggestionRepository.GetAll().OrderByDescending(n => n.CreateTime).AsEnumerable();
            //var allSuggestion = _suggestionRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<Suggestion, SuggestionDto>(allSuggestion);
        }

        public IEnumerable<SuggestionDto> GetSuggestionPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            var suggestionEnumrable = _suggestionRepository.GetPaged(pageIndex, pageCount, out count, n => n.CreateTime, false); 
            //var suggestionEnumrable = _suggestionRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<Suggestion, SuggestionDto>(suggestionEnumrable);
        }
    }
}
