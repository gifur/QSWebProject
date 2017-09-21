using System;
using System.Collections.Generic;
using System.Linq;
using QS.Common;
using QS.Core.IRepository;
using QS.DTO.Module;

namespace QS.Service.Effection
{
    public class MyMessageService : IMyMessageService
    {
        private readonly IMyMessageRepository _myMessageRepository;

        public MyMessageService() { }

        public MyMessageService(IMyMessageRepository myMessageRepository)
        {
            _myMessageRepository = myMessageRepository;
        }
        public void AddMyMessage(MyMessageDto myMessageDto)
        {
            _myMessageRepository.Add(QsMapper.CreateMap<MyMessageDto, Core.Module.MyMessage>(myMessageDto));
            _myMessageRepository.UnitOfWork.Commit();
        }

        public void DeleteMyMessage(Int64 myMessageId)
        {
            var temp = _myMessageRepository.Get(myMessageId);
            if (temp == null) return;
            _myMessageRepository.Remove(temp);
            _myMessageRepository.UnitOfWork.Commit();
        }

        public MyMessageDto GetMyMessageById(Int64 myMessageId)
        {
            var temp = _myMessageRepository.Get(myMessageId);
            return temp == null ? new MyMessageDto() : (QsMapper.CreateMap<Core.Module.MyMessage, MyMessageDto>(temp));
        }

        public bool ChangeMyMessageDescription(Int64 myMessageId, MyMessageDto updatedMyMessageDto)
        {
   
            var original = _myMessageRepository.Get(myMessageId);
            var recent = QsMapper.CreateMap<MyMessageDto, Core.Module.MyMessage>(updatedMyMessageDto);
            if (original != null && recent != null)
            {
                _myMessageRepository.Merge(original, recent);
                _myMessageRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<MyMessageDto> GetAllMyMessages()
        {
            var allMyMessage = _myMessageRepository.GetAll().OrderByDescending(n => n.RecentTime).AsEnumerable();
            //var allMyMessage = _myMessageRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<Core.Module.MyMessage, MyMessageDto>(allMyMessage);
        }

        public IEnumerable<MyMessageDto> GetMyMessagePaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            var myMessageEnumrable = _myMessageRepository.GetPaged(pageIndex, pageCount, out count, n => n.RecentTime, false); 
            //var myMessageEnumrable = _myMessageRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<Core.Module.MyMessage, MyMessageDto>(myMessageEnumrable);
        }

        public IEnumerable<MyMessageDto> GetMyMessages(int userid)
        {
            var myMessageEnumrable = _myMessageRepository.GetFiltered(m => m.UserId == userid).OrderByDescending(n => n.RecentTime).AsEnumerable(); ;
            return QsMapper.CreateMapIEnume<Core.Module.MyMessage, MyMessageDto>(myMessageEnumrable);
        }

        public IEnumerable<MyMessageDto> GetMyMessagesWithStatus(int userid, bool read)
        {
            var myMessageEnumrable = _myMessageRepository.GetFiltered(m => m.UserId == userid && m.Status == read).OrderByDescending(n => n.RecentTime).AsEnumerable(); ;
            return QsMapper.CreateMapIEnume<Core.Module.MyMessage, MyMessageDto>(myMessageEnumrable);
        }

        public int GetUnreadMessage(int userid, bool read = false)
        {
            var num = _myMessageRepository.Count(m => m.Status == read && m.UserId == userid);
            return num;
        }
    }
}
