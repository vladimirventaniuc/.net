using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;


namespace Biblioteca.API
{
    public class BibliotecaManager : IBibliotecaManager
    {

        private static BTESTEntities dbContext;
        UiHelp Ui = new UiHelp();
        private bool isTesting = false;
        public void Start()
        {
            isTesting = true;
            DateTime startDate, endDate;
            dbContext = new BTESTEntities();
            bool ok = true;
            Ui.ConsoleShow("***...Start...***");
            Ui.Options();
            string option = Ui.ConsoleRead();
            while (ok)
            {
                switch (option)
                {
                    #region Lista de optiuni
                    case "1":
                        Ui.Options();
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Achizitie carte
                    case "2":
                        Ui.ConsoleShow("Achizitie carte:");
                        Ui.ConsoleShow("Scrieri: Numele autorului, Prenumele autorului, Genul, Titlul si Numarul de exemplare.");
                        string nume = Ui.ConsoleRead();
                        string prenume = Ui.ConsoleRead();
                        string gen = Ui.ConsoleRead();
                        string titlu = Ui.ConsoleRead();
                        int nrExemplare = Ui.CheckIfIsNumber(Ui.ConsoleRead());
                        bool status = AchizitieCarte(nume, prenume, gen, titlu, nrExemplare);
                        if (status)
                        {
                            Ui.ConsoleShow("Comanda a fost executata cu succes!");
                        }
                        else
                        {
                            Ui.ConsoleShow("Eroare in executie!");
                        }
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Imprumutare carte
                    case "3":
                        Ui.ConsoleShow("Imprumutare carte:");
                        Ui.ConsoleShow("Tastati numele si prenumele");
                        string numeCititor = Ui.ConsoleRead();
                        string prenumeCititor = Ui.ConsoleRead();
                        bool readerExists = CheckIfReaderExsts(numeCititor, prenumeCititor);

                        if (!readerExists)
                        {
                            Ui.ConsoleShow("Scrieti adresa:");
                            string adresa = Ui.ConsoleRead();
                            Ui.ConsoleShow("Scrieti adresa de email:");
                            string email = Ui.ConsoleRead();
                            AdaugareCititor(numeCititor, prenumeCititor, adresa, email, true);
                        }

                        int? idCitiror = FindReaderIdByNameAndSurname(numeCititor, prenumeCititor);
                        Ui.ConsoleShow("Cititorul a fost ferificat si este serios");
                        Thread.Sleep(2000);
                        bool isOk = CheckReaderState(idCitiror);

                        if (!isOk)
                        {
                            Ui.ConsoleShow("Cititorul nu este in regula. Imprumutul este anulat!");
                            option = Ui.ConsoleRead();
                            break;
                        }

                        Ui.ConsoleShow("Tastati denumirea unui gen:");
                        string genCarte = Ui.ConsoleRead();
                        ShowGendersList(GetllBooksFromAGendres(genCarte));
                        Ui.ConsoleShow("Indicati titlul cartii sau dati enter pentru a continua!");
                        string titleToBorrow = Ui.ConsoleRead();
                        Ui.ConsoleShow("Indicati numele si prenumele autorului sau dati enter pentru a continua!");
                        string authorNameToBorrow = Ui.ConsoleRead();
                        string authorSurnameToBorrow = Ui.ConsoleRead();
                        string response = GetMeAllTheBooksForBorrowing(titleToBorrow, authorNameToBorrow, authorSurnameToBorrow);
                        if (response == "Nu ati tasat corect titlul, numele sau prenumele autorului. Reincercati!")
                        {
                            Ui.ConsoleShow(response);
                        }
                        else if (response.Contains("zile"))
                        {
                            response = "Cartea nu este disponibila momentan. Aceasta va putea fi imprumutata in " + response;
                            Ui.ConsoleShow(response);
                        }
                        else
                        {
                            int.TryParse(response, out int bookId);

                            ImprumutCarte(bookId, idCitiror);
                            Ui.ConsoleShow("Cartea a fost imprumutata cu succes!");
                        }
                        break;
                    #endregion

                    #region Restituire carte
                    case "4":
                        Ui.ConsoleShow("Restituire carte:");
                        Ui.ConsoleShow("Tastati id-ul imprumutului:");
                        int idImprumut = Ui.CheckIfIsNumber(Ui.ConsoleRead());
                        if (!RestituireCarte(idImprumut))
                        {
                            Ui.ConsoleShow("Eroare la restituirea cartii!");
                        }
                        else
                        {
                            Ui.ConsoleShow("Cartea a fost restituita.");
                            Ui.ConsoleShow("Scrieti va rog cateva detalii despre carte:");
                            string detalii = Ui.ConsoleRead();
                            AdaugaReview(idImprumut, detalii);
                        }
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Numarul de cititori si care sunt acestia intr-o perioada data
                    case "5":
                        Ui.ConsoleShow("Numarul de cititori si care sunt acestia intr-o perioada data:");
                        SetDates(out startDate, out endDate);
                        string message = "Numarul de cititori:" + GenNumberOfReadersBetweenDates(startDate, endDate);
                        Ui.ConsoleShow(message);
                        Ui.ConsoleShow("Cititorii activi:");
                        Show(GetAllTheReadersBetweenDates(startDate, endDate));
                        break;
                    #endregion

                    #region Cele mai solicitate carti (top 5)
                    case "6":
                        Ui.ConsoleShow("Cele mai solicitate carti:");
                        Show(Top5Books());
                        break;
                    #endregion

                    #region Autorii cei mai cautati (top 5)
                    case "7":
                        Ui.ConsoleShow("Autorii cei mai cautati:");
                        Show(Top5Authors());
                        break;
                    #endregion

                    #region Genurile cele mai solicitate (top 5)
                    case "8":
                        Ui.ConsoleShow("Genurile cele mai solicitate:");
                        Show(Top5Geres());
                        break;
                    #endregion
             
                    #region Review-urile pentru o anumita carte
                    case "9":
                        Ui.ConsoleShow("Review-urile pentru o anumita carte:");
                        Ui.ConsoleShow("scrieti titlul sau id-ul cartii.");
                        string request = Ui.ConsoleRead();
                        Show(GetMeReviews(request));
                    break;
                    #endregion
                   
                    #region Adaugare cititor
                    case "10":
                        Ui.ConsoleShow("Adaugare cititor:");
                        Ui.ConsoleShow("Tastati numele si prenumele");
                         numeCititor = Ui.ConsoleRead();
                         prenumeCititor = Ui.ConsoleRead();
                        Ui.ConsoleShow("Scrieti adresa:");
                        string adresaC = Ui.ConsoleRead();
                        Ui.ConsoleShow("Scrieti adresa de email:");
                        string emailC = Ui.ConsoleRead();
                        AdaugareCititor(numeCititor, prenumeCititor, adresaC, emailC, true);
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Query direct
                    case "11":
                        Ui.ConsoleShow("Query direct.");
                        Ui.ConsoleShow("Tastati comanda.");
                        string command = Ui.ConsoleRead();
                        Ui.ConsoleShow("Executie...");
                        Thread.Sleep(2000);
                        ShowList(DirectQuery(command));
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Incheiere proces & defalut
                    case "00":
                        Ui.ConsoleShow("***...Proces incheiat...***");
                        Thread.Sleep(500);
                        ok = false;
                        break;

                    default:
                        Ui.ConsoleShow("Nu ati tastat o optiune valida! Retastati.");
                        option = Ui.ConsoleRead();
                        break;
                    #endregion
                }
            }
        }
        public void SetDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = Ui.CheckIfIsDate(Ui.ConsoleRead());
            endDate = Ui.CheckIfIsDate(Ui.ConsoleRead());
        }
        public void Show(IQueryable<object> query)
        {
            foreach (var item in query)
            {
                Ui.ConsoleShow(item.ToString());
            }
        }
        public bool AchizitieCarte(string numeAutor, string prenumeAutor, string gen, string titlu, int numarExemplare)
        {
            DbSet<CARTE> books = dbContext.CARTE;

            int? autorId = CheckIfAutorExstsAndAdd(numeAutor,prenumeAutor);
            if (autorId == null || autorId == 0)
               return false;

            int? genId = CheckIfGenreExistsAndAdd(gen);
            if (genId == null || genId == 0)
                return false;

            try
            {
                for (int i = 0; i < numarExemplare; i++)
                {
                    books.Add(new CARTE() {AutorId = autorId, Titlu = titlu, GenId = genId});
                    dbContext.SaveChanges();
                }

                return true;
            }

            catch (Exception Ex)
            {
                var error = Ex;
                return false;
            }
        }
        public int? CheckIfAutorExstsAndAdd(string numeAutor, string prenumeAutor)
        {
            DbSet<AUTOR> authors = dbContext.AUTOR;       
            try
            {
                int id;
                id = authors.FirstOrDefault(u => u.Nume == numeAutor && u.Prenume == prenumeAutor).AutorId;

                if (id == null || id ==0)
                {
                    authors.Add(new AUTOR() {Nume = numeAutor, Prenume = prenumeAutor});
                    dbContext.SaveChanges();
                    id = authors.FirstOrDefault(u => u.Nume == numeAutor && u.Prenume == prenumeAutor).AutorId;
                }

                return id;
            }
            catch (Exception Ex)
            {
                var error = Ex;
                return 0;
            }
        }
        public int? CheckIfGenreExistsAndAdd(string gen)
        {
            DbSet<GEN> gendres = dbContext.GEN;
            try
            {
                int? id = gendres.FirstOrDefault(u => u.Descriere == gen).GenId;              
                if (id == null || id == 0)
                {
                    gendres.Add(new GEN() {Descriere = gen});
                    dbContext.SaveChanges();
                    id = gendres.FirstOrDefault(u => u.Descriere == gen).GenId;
                }

                return id;
            }

            catch(Exception Ex)
            {
                var error = Ex;
                return 0;
            }
        }
        public bool CheckIfReaderExsts(string numeCititor, string prenumeCititor)
        {
            DbSet<CITITOR> readers = dbContext.CITITOR;
            
                int? id = readers.FirstOrDefault(u => u.Nume == numeCititor && u.Prenume == prenumeCititor).CititorId;

                if (id == null)
                   return false;           

            return true;
        }
        public bool CheckReaderState(int? id)
        {
            DbSet<CITITOR> readers = dbContext.CITITOR;
            bool? state = readers.FirstOrDefault(r => r.CititorId == id).Stare;
            if (state == true)
            {
                return true;
            }

            return false;
        }
        public string GetMeAllTheBooksForBorrowing(string titlu, string nume, string prenume)
        {
            int? bookToBorrowId = null;
           
            if (titlu != null && nume != null && prenume != null)
            {
                bookToBorrowId = GetBookIdByAuthorAndTitle(nume, prenume, titlu);
                if (bookToBorrowId == null ||bookToBorrowId == 0)

                    return "Nu ati tasat corect titlul, numele sau prenumele autorului. Reincercati!";
            }
            else if(titlu == null && nume != null && prenume != null)
            {
                IQueryable <CARTE> booksFromAnAnAuthor = GetBooksByAuthor(nume, prenume);
                if (isTesting)
                    bookToBorrowId = booksFromAnAnAuthor.First().CarteId;
            }
            else if (titlu != null && nume == null && prenume == null)
            {
                bookToBorrowId = GetBookIdByTitle(titlu);
                if (bookToBorrowId == null || bookToBorrowId == 0)

                    return "Nu ati tasat corect titlul. Reincercati!";
            }
            else
            {
                string errorMessage = "Nu ati tasat corect titlul, numele sau prenumele autorului. Reincercati!";

                return errorMessage;
            }

            if (bookToBorrowId == null)
                return "Eroare!";

           return TryToBorrowABook(bookToBorrowId);
        }
        public string TryToBorrowABook(int? bookToBorrowId)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            int? id = bookToBorrowId;
            if (id == null || id == 0)
            {
                DateTime data;
                var o = borrows  
                                 .Where(x => x.CarteId == bookToBorrowId)
                                 .OrderByDescending(y=> y.DataScadenta);
                data = o.First().DataScadenta;

                int nrZilDiferenta = (int)(DateTime.Now - data).TotalDays;
                return nrZilDiferenta + " zile";
            }

            return id.ToString();
        }
        public int? GetBookIdByTitle(string title)
        {
            DbSet<CARTE> books = dbContext.CARTE;

            return books.FirstOrDefault(x => x.Titlu.ToLower() == title.ToLower()).CarteId;
        }
        public IQueryable<CARTE> GetBooksByAuthor(string nume, string prenume)
        {
            DbSet<CARTE> books = dbContext.CARTE;
            DbSet<AUTOR> authors = dbContext.AUTOR;

            int? id = authors.FirstOrDefault(x => x.Nume == nume && x.Prenume == prenume).AutorId;
            
            return books.Where(x => x.AutorId == id);

        }
        public int? GetBookIdByAuthorAndTitle(string nume, string prenume, string titlu)
        {
            DbSet<AUTOR> authors = dbContext.AUTOR;
            DbSet<CARTE> books = dbContext.CARTE;

            int? id = authors.FirstOrDefault(x => x.Nume == nume && x.Prenume == prenume).AutorId;
            if (id == null || id == 0)
            {
                return 0;
            }

            return books.FirstOrDefault(x => x.AutorId == id && x.Titlu.ToLower() == titlu).CarteId;
        }
        public int? FindReaderIdByNameAndSurname(string nume, string prenume)
        {
            DbSet<CITITOR> readers = dbContext.CITITOR;
            int? cititorId = readers.FirstOrDefault(u => u.Nume == nume && u.Prenume == prenume).CititorId;
            if (cititorId == null)
                return 0;

            return cititorId;
        }
        public bool ImprumutCarte(int carteId, int? cititorId)
        {
            DbSet <IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DateTime dataImprumut = DateTime.Now;
            DateTime dataScadenta = DateTime.Now.AddDays(14);

            try
            {
                borrows.Add(new IMPRUMUT()
                {
                    CarteId = carteId,
                    CititorId = cititorId,
                    DataImprumut = dataImprumut,
                    DataScadenta = dataScadenta
                });
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                var error = Ex;
                return false;
            }
        }
        public List<GEN> GetllBooksFromAGendres(string gen)
        {
            DbSet<GEN> gendres = dbContext.GEN;
            List<GEN> allBooksFromAGendres = gendres.Where(g => g.Descriere == gen).ToList();
            return allBooksFromAGendres;
        }
        public void ShowGendersList(List<GEN> list)
        {
            foreach (GEN item in list)
            {
                Ui.ConsoleShow(item.ToString());
            }
        }
        public bool RestituireCarte(int id)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;
            try
            {
                IMPRUMUT carteImprumutata = borrows.FirstOrDefault(x => x.ImprumutId == id);
                carteImprumutata.DataRestituire = DateTime.Now;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                var error = Ex;
                return false;
            }
        }
        public bool AdaugaReview(int imprumutId, string detalii)
        {
            DbSet<REVIEW> reviews = dbContext.REVIEW;
            try
            {
             
                    reviews.Add(new REVIEW() {ImprumutId = imprumutId, Text = detalii});
                    dbContext.SaveChanges();
              

                return true;
            }

            catch (Exception Ex)
            {
                var error = Ex;
                return false;
            }

        }
        public int? AdaugareCititor(string nume, string prenume, string adresa, string email, bool stare)
        {
            DbSet<CITITOR> readers = dbContext.CITITOR;
            if (adresa == null && email == null && isTesting)
            {
                Ui.ConsoleShow("Tastati adresa:");
                adresa = Ui.ConsoleRead();
                Ui.ConsoleShow("Tastati emailul:");
                email = Ui.ConsoleRead();
            }

            try
            {
                int? id = readers.FirstOrDefault(u => u.Nume == nume && u.Prenume == prenume).CititorId;              
                if (id == null || id == 0)
                {
                    readers.Add(new CITITOR() {Nume = nume, Prenume = prenume, Adresa = adresa, Email = email, Stare = stare});
                    dbContext.SaveChanges();
                    id = readers.FirstOrDefault(u => u.Nume == nume && u.Prenume == prenume).CititorId;
                }

                return id;
            }
            catch (Exception Ex)
            {
                var error = Ex;
                return 0;
            }

        }
        public int GenNumberOfReadersBetweenDates(DateTime startDate, DateTime endDate)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            int? numberOfReaders = borrows
                                          .Select(x =>
                                                     x.DataScadenta >= startDate 
                                                     && x.DataScadenta <= endDate 
                                                     && x.DataImprumut >= startDate
                                                     && x.DataImprumut <= endDate)
                                          .Distinct()
                                          .Count();
            if(numberOfReaders == 0)
                return 0;

            return (int)numberOfReaders;
        }
        public IQueryable<object> GetAllTheReadersBetweenDates(DateTime startDate, DateTime endDate)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CITITOR> readers = dbContext.CITITOR;

            return borrows
                .Join(readers, b => b.CititorId, r => r.CititorId, (b, r) => new {b, r})
                .Where(br =>
                    br.b.DataScadenta >= startDate
                    && br.b.DataScadenta <= endDate
                    && br.b.DataImprumut >= startDate
                    && br.b.DataImprumut <= endDate)
                .Select(e => new
                {
                    Nume = e.r.Nume
                    ,Prenume = e.r.Prenume
                })
                .Distinct();
        }
        public IQueryable<object> Top5Books()
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;

            return borrows
                        .Join(books, b => b.CarteId, br => br.CarteId, (b, br) => new {b, br})
                        .GroupBy(x => new {x.br.CarteId})
                        .Select(g => new
                        {
                             g.FirstOrDefault().br.Titlu
                            ,NumarImprumuturi = g.Count()
                        })
                        .OrderByDescending(q=> q.NumarImprumuturi)
                        .Take(5);             
        }
        public IQueryable<object> Top5Authors()
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<AUTOR> authors = dbContext.AUTOR;
            DbSet<CARTE> books = dbContext.CARTE;

            return borrows
                .Join(books, b => b.CarteId, br => br.CarteId, (b, br) => new { b, br })
                .Join(authors, a => a.br.AutorId, bbr => bbr.AutorId, (a, bbr) => new { a, bbr})
                .GroupBy(x => new { x.bbr.AutorId })
                .Select(g => new
                {
                    g.FirstOrDefault().bbr.AutorId
                    ,
                    NumarImprumuturi = g.Count()
                })
                .OrderByDescending(q => q.NumarImprumuturi)
                .Take(5);
        }
        public IQueryable<object> Top5Geres()
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<GEN> genres = dbContext.GEN;
            DbSet<CARTE> books = dbContext.CARTE;

            return borrows
                .Join(books, b => b.CarteId, br => br.CarteId, (b, br) => new { b, br })
                .Join(genres, g => g.br.GenId, bbr => bbr.GenId, (a, bbr) => new { a, bbr })
                .GroupBy(x => new { x.bbr.GenId })
                .Select(g => new
                {
                    g.FirstOrDefault().bbr.GenId
                    ,
                    NumarImprumuturi = g.Count()
                })
                .OrderByDescending(q => q.NumarImprumuturi)
                .Take(5);
        }
        public IQueryable<object> GetMeReviews(string request)
        {
            DbSet<REVIEW> reviews = dbContext.REVIEW;
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;

            if (Int32.TryParse(request, out int id))
            {
             return reviews
                    .Join(borrows, r => r.ImprumutId, br => br.ImprumutId, (r, br) => new {r, br})
                    .Where(o => o.br.CarteId == id)
                    .Select(o => new
                    {
                       o.r.Text
                    });
            }

            int? bId = GetBookIdByTitle(request);

                return reviews
                    .Join(borrows, r => r.ImprumutId, br => br.ImprumutId, (r, br) => new { r, br })
                    .Where(o => o.br.CarteId == bId)
                    .Select(o => new
                    {
                       o.r.Text
                    });
        }
        public List<object> DirectQuery(string query)
        {
            List<object> list = new List<object>();

            // TODO - se completeaza 

            string server = "";
            string user = "";
            string password = "";

            var connectionString = "Server= "+ server +";Database=master;User Id="+ user +";Password= "+ password +";Integrated Security=True";
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        if (query.ToLower().StartsWith("update") ||
                            query.ToLower().StartsWith("delete") ||
                            query.ToLower().StartsWith("insert"))
                        {
                            con.Open();
                            var rowsAffected = cmd.ExecuteNonQuery();
                            con.Close();
                            string rows = $"Rows affected {rowsAffected}";
                            list.Add(new {rows});
                            return list;
                        };

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(reader.GetString(0));
                            }
                        }

                        return list;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void ShowList(List<object> list)
        {
            foreach (object item in list)
            {
                Ui.ConsoleShow(item.ToString());
            }
        }

        // nu sunt necesare
        public IQueryable<object> SeeIfAllBooksWithATitle(string titlu)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;

            return borrows
                         .Join(books, bo => bo.CarteId, b => b.CarteId, (bo, b) => new { bo, b })
                         .Where(bob => bob.b.Titlu == titlu)
                         .Select(E => new
                         {
                             CarteId = E.b.CarteId
                            ,
                             DataRestituire = E.bo.DataRestituire
                            ,
                             DataScadenta = E.bo.DataScadenta
                         });

        }
        public IQueryable<object> SeeIfAllBooksFromAnAuthor(string numeAutor, string prenumeAutor)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;
            DbSet<AUTOR> authors = dbContext.AUTOR;

            return borrows
                        .Join(books, bo => bo.CarteId, b => b.CarteId, (bo, b) => new { bo, b })
                        .Join(authors, bob => bob.b.AutorId, a => a.AutorId, (bob, a) => new { bob, a })
                        .Where(boba => boba.a.Nume == numeAutor && boba.a.Prenume == prenumeAutor)
                        .Select(E => new
                        {
                            CarteId = E.bob.b.CarteId
                            ,
                            DataRestituire = E.bob.bo.DataRestituire
                            ,
                            DataScadenta = E.bob.bo.DataScadenta
                        });
        }
        public IQueryable<object> SeeIfAllBooksFromAnAuthorAndTitle(string titlu, string numeAutor, string prenumeAutor)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;
            DbSet<AUTOR> authors = dbContext.AUTOR;

            return borrows
                    .Join(books, bo => bo.CarteId, b => b.CarteId, (bo, b) => new { bo, b })
                    .Join(authors, bob => bob.b.AutorId, a => a.AutorId, (bob, a) => new { bob, a })
                    .Where(boba =>
                        boba.a.Nume == numeAutor && boba.a.Prenume == prenumeAutor && boba.bob.b.Titlu == titlu)
                    .Select(E => new
                    {
                        CarteId = E.bob.b.CarteId
                        ,
                        DataRestituire = E.bob.bo.DataRestituire
                        ,
                        DataScadenta = E.bob.bo.DataScadenta
                    });
        }
    }
}
