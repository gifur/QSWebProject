using System;
using System.Collections.Generic;
using System.Linq;
using QS.Common;
using QS.Core.IRepository;
using QS.DTO.Module;

namespace QS.Service.Effection
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService() { }

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public void AddMessage(MessageDto messageDto)
        {
            messageDto.CreateTime = DateTime.Now;
            _messageRepository.Add(QsMapper.CreateMap<MessageDto, Core.Module.Message>(messageDto));
            _messageRepository.UnitOfWork.Commit();
        }

        public void DeleteMessage(Int64 messageId)
        {
            var temp = _messageRepository.Get(messageId);
            if (temp == null) return;
            _messageRepository.Remove(temp);
            _messageRepository.UnitOfWork.Commit();
        }

        public MessageDto GetMessageById(Int64 messageId)
        {
            var temp = _messageRepository.Get(messageId);
            return temp == null ? new MessageDto() : (QsMapper.CreateMap<Core.Module.Message, MessageDto>(temp));
        }

        public bool ChangeMessageDescription(Int64 messageId, MessageDto updatedMessageDto)
        {
   
            var original = _messageRepository.Get(messageId);
            var recent = QsMapper.CreateMap<MessageDto, Core.Module.Message>(updatedMessageDto);
            if (original != null && recent != null)
            {
                _messageRepository.Merge(original, recent);
                _messageRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<MessageDto> GetAllMessages()
        {
            var allMessage = _messageRepository.GetAll().OrderByDescending(n => n.CreateTime).AsEnumerable();
            //var allMessage = _messageRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<Core.Module.Message, MessageDto>(allMessage);
        }

        public IEnumerable<MessageDto> GetMessagePaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            var messageEnumrable = _messageRepository.GetPaged(pageIndex, pageCount, out count, n => n.CreateTime, false); 
            //var messageEnumrable = _messageRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<Core.Module.Message, MessageDto>(messageEnumrable);
        }
    }
}
