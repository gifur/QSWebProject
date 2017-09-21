using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface IBookService
    {
        void AddBook(BookDto bookDto);
        void DeleteBook(Int64 bookId);
        BookDto GetBookById(Int64 bookId);
        bool ChangeBookDetail(Int64 bookId, BookDto updatedBookDto);
        IEnumerable<BookDto> GetRandomBooks(int num = 6);
        IEnumerable<BookDto> GetHighGradeBooks(int num = 3);
        IEnumerable<BookDto> GetMostItemBooks(string field, int num = 5);
        IEnumerable<BookDto> GetBooksWithCategory(string category, int pageIndex, int pageCount, out int count);
        IEnumerable<BookDto> GetBooksWithCategory(int categoryId, int pageIndex, int pageCount, out int count);
        IEnumerable<BookDto> GetBooksPaged(int pageIndex, int pageCount, out int count);
        int GetBooksNumWithCategory(string category);
        IEnumerable<BookDto> GetBooksWithCategory(string category, out int count);
        IEnumerable<BookDto> GetAllBooks();
        int IncreaseViewsOfBookOf(Int64 bookId);
    }
}
