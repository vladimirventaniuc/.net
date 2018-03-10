using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteca
{
    interface IBibliotecaManager
    {
        void Start();
        bool AchizitieCarte(string numeAutor, string prenumeAutor, string gen, string titlu, int numarExemplare);
        int? CheckIfAutorExstsAndAdd(string numeAutor, string prenumeAutor);
        int? CheckIfGenreExistsAndAdd(string gen);
        bool CheckIfReaderExsts(string numeCititor, string prenumeCititor);
        bool CheckReaderState(int? id);
        string GetMeAllTheBooksForBorrowing(string titlu, string nume, string prenume);
        string TryToBorrowABook(int? bookToBorrowId);
        int? GetBookIdByTitle(string title);
        IQueryable<CARTE> GetBooksByAuthor(string nume, string prenume);
        int? GetBookIdByAuthorAndTitle(string nume, string prenume, string titlu);
        int? FindReaderIdByNameAndSurname(string nume, string prenume);
        bool ImprumutCarte(int carteId, int? cititorId);
        List<GEN> GetllBooksFromAGendres(string gen);
        bool RestituireCarte(int id);
        bool AdaugaReview(int imprumutId, string detalii);
        int? AdaugareCititor(string nume, string prenume, string adresa, string email, bool stare);
        int GenNumberOfReadersBetweenDates(DateTime startDate, DateTime endDate);
        IQueryable<object> GetAllTheReadersBetweenDates(DateTime startDate, DateTime endDate);
        IQueryable<object> Top5Books();
        IQueryable<object> Top5Authors();
        IQueryable<object> Top5Geres();
        IQueryable<object> GetMeReviews(string request);
        List<object> DirectQuery(string query);
    }
}