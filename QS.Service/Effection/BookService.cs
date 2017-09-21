using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.SharedAggregate;
using QS.DTO.SharedModule;

namespace QS.Service.Effection
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService() { }

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(BookDto bookDto)
        {
            bookDto.CreateTime = DateTime.Now;
            _bookRepository.Add(QsMapper.CreateMap<BookDto, Book>(bookDto));
            _bookRepository.UnitOfWork.Commit();
        }

        public void DeleteBook(long bookId)
        {
            var temp = _bookRepository.Get(bookId);
            if (temp != null)
            {
                _bookRepository.Remove(temp);
                _bookRepository.UnitOfWork.Commit();
            }
        }

        public BookDto GetBookById(long bookId)
        {
            var temp = _bookRepository.Get(bookId);
            return temp == null ? null : (QsMapper.CreateMap<Book, BookDto>(temp));
        }

        public bool ChangeBookDetail(long bookId, BookDto updatedBookDto)
        {
            var original = _bookRepository.Get(bookId);
            var recent = QsMapper.CreateMap<BookDto, Book>(updatedBookDto);
            if (original != null && recent != null)
            {
                _bookRepository.Merge(original, recent);
                _bookRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<BookDto> GetRandomBooks(int num = 6)
        {
            var sql = String.Format("SELECT TOP {0} * FROM Book ORDER BY NewID()", num);
            var results = _bookRepository.ExecuteQuery(sql);
            return QsMapper.CreateMapIEnume<Book, BookDto>(results);
        }

        public IEnumerable<BookDto> GetHighGradeBooks(int num = 3)
        {
            var sql = String.Format("SELECT Top {0} * FROM Book ORDER BY Grade DESC", num);
            var results = _bookRepository.ExecuteQuery(sql);
            return QsMapper.CreateMapIEnume<Book, BookDto>(results);
        }

        public IEnumerable<BookDto> GetMostItemBooks(string field, int num = 5)
        {
            var sql = String.Format("SELECT Top {0} * FROM Book ORDER BY {1} DESC", num, field);
            var results = _bookRepository.ExecuteQuery(sql);
            return QsMapper.CreateMapIEnume<Book, BookDto>(results);
        }

        public IEnumerable<BookDto> GetBooksWithCategory(string category, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookDto> GetBooksWithCategory(int categoryId, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookDto> GetBooksPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageCount <= 0 || pageIndex <= 0)
            {
                count = 0;
                return null;
            }

            var bookEnumrable = _bookRepository.GetPaged<DateTime>(pageIndex, pageCount, out count, p => p.CreateTime, false);
            return QsMapper.CreateMapIEnume<Book, BookDto>(bookEnumrable);
        }

        public int GetBooksNumWithCategory(string category)
        {
            return _bookRepository.Count(b => b.Category.Contains(category));
        }

        public IEnumerable<BookDto> GetBooksWithCategory(string category, out int count)
        {
            var results = _bookRepository.GetFiltered(b => b.Category.Contains(category), out count);
            return QsMapper.CreateMapIEnume<Book, BookDto>(results);
        }

        public IEnumerable<BookDto> GetAllBooks()
        {
            var allBooks = _bookRepository.GetAllWithOrder(p => p.CreateTime);
            return QsMapper.CreateMapIEnume<Book, BookDto>(allBooks);
        }

        public int IncreaseViewsOfBookOf(long bookId)
        {
            var sql = String.Format("UPDATE Book SET Hits = Hits + 1 WHERE BookId = {0}", bookId);
            return _bookRepository.ExecuteCommand(sql);
        }
    }
}
