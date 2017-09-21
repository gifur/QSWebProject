using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.CommentModule;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface ISuggestionService
    {
        void AddSuggestion(SuggestionDto suggestionDto);
        void DeleteSuggestion(Int64 suggestionId);
        SuggestionDto GetSuggestionById(Int64 suggestionId);
        IEnumerable<SuggestionDto> GetAllSuggestions();
        IEnumerable<SuggestionDto> GetSuggestionPaged(int pageIndex, int pageCount, out int count);

    }
}
