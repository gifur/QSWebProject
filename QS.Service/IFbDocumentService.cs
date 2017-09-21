using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.FeedbackModule;

namespace QS.Service
{
    public interface IFbDocumentService
    {
        void AddDocument(FbDocumentDto documentDto);

        void DeleteDocument(Guid id);

        IEnumerable<FbDocumentDto> GetDocumentsUnderFeedback(int index);

        List<FbDocumentDto> FindDocuments(int pageIndex, int pageCount);

        List<FbDocumentDto> GetAllDocument();
        FbDocumentDto GetDocumentById(Guid documentId);
    }
}
