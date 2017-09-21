using System;
using System.Collections.Generic;
using System.Linq;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.FeedbackAggregate;
using QS.DTO.FeedbackModule;

namespace QS.Service.Effection
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRespository)
        {
            _feedbackRepository = feedbackRespository;
        }
        public FeedbackDto GetFeedbackById(int id)
        {
            return QsMapper.CreateMap<Feedback, FeedbackDto>(_feedbackRepository.Get(id));
        }

        public void AddFeedback(FeedbackDto feedbackDto)
        {
            _feedbackRepository.Add(QsMapper.CreateMap<FeedbackDto, Feedback>(feedbackDto));
            _feedbackRepository.UnitOfWork.Commit();
        }

        public void DeleteFeedback(int feedbackId)
        {
            var aimFeedback = _feedbackRepository.Get(feedbackId);
            if (aimFeedback != null)
            {
                _feedbackRepository.Remove(aimFeedback);
                _feedbackRepository.UnitOfWork.Commit();
            }
        }

        public OutMsg UpdateFeedbackDetail(int id, FeedbackDto feedbackDto)
        {
            if (feedbackDto == null)
            {
                throw new ArgumentNullException();
            }

            var message = new OutMsg { Status = false, Msg = "修改信息失败" };

            var currentFeedback = _feedbackRepository.Get(id);
            var updateFeedback = QsMapper.CreateMap<FeedbackDto, Feedback>(feedbackDto);

            _feedbackRepository.Merge(currentFeedback, updateFeedback);
            _feedbackRepository.UnitOfWork.Commit();
            message.Msg = "成功修改信息";
            message.Status = true;
            return message;

        }

        public List<FeedbackDto> FindFeedbacks(int pageIndex, int pageCount)
        {
            throw new NotImplementedException();
        }

        public List<FeedbackDto> GetAllFeedback()
        {
            var feedbacks = _feedbackRepository.GetAllWithOrder(filter => filter.CreateTime).ToList();

            if (!feedbacks.Any()) return new List<FeedbackDto>();
            var lstFeedbackDto = QsMapper.CreateMapList<Feedback, FeedbackDto>(feedbacks);
            return lstFeedbackDto;
        }

        /// <summary>
        /// 获取最新的即将开始或正在进行的心理反馈信息
        /// </summary>
        /// <returns></returns>
        public FeedbackDto GetActiveItem()
        {
            var result = _feedbackRepository.FirstOrDefault(fb => fb.Status == (int)EnumFbStatus.Await || fb.Status == (int)EnumFbStatus.Underway);
            return FeedbackToDto(result);
        }

        public FeedbackDto FeedbackToDto(Feedback from)
        {
            if (from == null) return null;
            var aimFeedback = QsMapper.CreateMap<Feedback, FeedbackDto>(from);

            //Mapper.CreateMap<FbDocument, FbDocumentDto>()
            //    .ForMember(dto => dto.UploaderName, opt => opt.MapFrom(entity => entity.Uploader.RealName))
            //    .ForMember(dto => dto.FeedbackName, opt => opt.MapFrom(entity => entity.Feedback.FeedbackName));
            //aimFeedback.FbDocumentDtos = Mapper.Map<List<FbDocument>, List<FbDocumentDto>>(from.FbDocuments.ToList());
            return aimFeedback;
        }
    }
}
