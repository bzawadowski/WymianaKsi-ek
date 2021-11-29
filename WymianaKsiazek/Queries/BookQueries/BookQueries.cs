using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Models.MapperModels;
using Microsoft.EntityFrameworkCore;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Queries.BookQueries
{
    public class BookQueries : IBookQueries
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        public BookQueries(IMapper mapper, Context context)
        {
            _mapper = mapper;
            _context = context;
        }
        public List<BookReviewsList> GetBooks()
        {
            var books = _context.Book.Include(x => x.Opinion).ToList();
            return _mapper.Map<List<BookReviewsList>>(books);
        }
        public List<BookReviewsList> GetBooksByCategory(long id)
        {
            var books = _context.Book.Include(x => x.Opinion).Include(x => x.Category).Where(x => x.Category_Id == id).ToList();
            return _mapper.Map<List<BookReviewsList>>(books);
        }
        public List<BookReviewsList> GetBooksByAuthor(string author)
        {
            var books = _context.Book.Include(x => x.Opinion).Include(x => x.Category).Where(x => x.Author == author).ToList();
            return _mapper.Map<List<BookReviewsList>>(books);
        }
        public List<BookReviewsList> GetConcretBook(string title, string author)
        {
            var books = _context.Book.Include(x => x.Opinion).Include(x => x.Category).Where(x => x.Author == author && x.Title == title).ToList();
            return _mapper.Map<List<BookReviewsList>>(books);
        }
        public List<BookReviewsList> GetSearchedBooks(string title, string author)
        {
            var books = GetBooks();
            if((title == null || title == "") && (author == null || author == ""))
            {
                return books;
            }
            else if(title == null || title == "")
            {
                author = author.ToUpper();
                books = (from i in books where i.Author.ToUpper().Contains(author) select i).ToList();
            }
            else if(author == null || author == "")
            {
                title = title.ToUpper();
                books = (from i in books where i.Title.ToUpper().Contains(title) select i).ToList();
            }
            else
            {
                title = title.ToUpper();
                author = author.ToUpper();
                books = (from i in books where i.Title.ToUpper().Contains(title) && i.Author.ToUpper().Contains(author) select i).ToList();
            }
            return books;
        }
        public List<BookReviewsList> GetSearchedBooksByFilter(List<BookReviewsList> books, long categoryid, int sorttype)
        {
            if(categoryid > 0)
            {
                books = (from i in books where i.Category_Id == categoryid select i).ToList();
            }
            if(sorttype != 2)
            {
                if(sorttype == 1)
                {
                    books = (from i in books orderby i.Opinion.TotalOpinions() descending select i).ToList();
                }
                else if(sorttype == 0)
                {
                    books = (from i in books orderby i.Opinion.AverageOpinion() descending select i).ToList();
                }
            }
            return books;
        }
        public BookOpinionMP GetBookById(long id)
        {
            var book = _context.Book.Include(x => x.Opinion).Include(x => x.Comments).ThenInclude(x => x.User)
                .Where(x => x.Book_Id == id).FirstOrDefault();
            return _mapper.Map<BookOpinionMP>(book);
        }
        public BookOpinionMP GetBookOpinions(long bookid)
        {
            var bookopinion = _context.Book.Include(x => x.Category).Include(x => x.Offers).Include(x => x.Comments)
                .Where(x => x.Book_Id == bookid).FirstOrDefault();
            return _mapper.Map<BookOpinionMP>(bookopinion);
        }
        public string GetCategoryName(long id)
        {
            var category = _context.Category.Where(x => x.Category_Id == id).Select(x => x.Category_Name).FirstOrDefault();
            return category;
        }
    }
}
