using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.FeedbackAggregate;
using QS.DTO.FeedbackModule;

namespace QS.Service.Effection
{
    public class FbDocumentService : IFbDocumentService
    {
        private readonly IFbDocumentRepository _respository;

        public FbDocumentService(IFbDocumentRepository documentRepository)
        {
            _respository = documentRepository;
        }

        public void AddDocument(FbDocumentDto documentDto)
        {
            _respository.Add(QsMapper.CreateMap<FbDocumentDto, FbDocument>(documentDto));
            _respository.UnitOfWork.Commit();
        }

        public void DeleteDocument(Guid id)
        {
            var aimDocument = _respository.Get(id);
            if (aimDocument != null)
            {
                _respository.Remove(aimDocument);
                _respository.UnitOfWork.Commit();
            }
        }

        public IEnumerable<FbDocumentDto> GetDocumentsUnderFeedback(int index)
        {
            var temp = _respository.GetFiltered(filter => filter.FeedbackId == index).AsQueryable();
            var documents = temp.OrderByDescending(filter => filter.UploadDate);
            Mapper.CreateMap<FbDocument, FbDocumentDto>()
                .ForMember(dto => dto.UploaderName, opt => opt.MapFrom(entity => entity.Uploader.RealName));
            var lstDocumentDto = Mapper.Map<IEnumerable<FbDocument>, IEnumerable<FbDocumentDto>>(documents);

            //var lstDocumentDto = QsMapper.CreateMapIEnume<FbDocument, FbDocumentDto>(documents);
            return lstDocumentDto;
        }

        public List<FbDocumentDto> FindDocuments(int pageIndex, int pageCount)
        {
            throw new NotImplementedException();
        }

        public List<FbDocumentDto> GetAllDocument()
        {
            var documents = _respository.GetAllWithOrder(filter => filter.UploadDate).ToList();
            if (!documents.Any())
            {
                return new List<FbDocumentDto>();
            }

            var lstDocumentDto = QsMapper.CreateMapList<FbDocument, FbDocumentDto>(documents);
            return lstDocumentDto;
        }

        public FbDocumentDto GetDocumentById(Guid documentId)
        {
            var temp = _respository.Get(documentId);
            return temp == null ? null : QsMapper.CreateMap<FbDocument, FbDocumentDto>(temp);
        }
    }
}
