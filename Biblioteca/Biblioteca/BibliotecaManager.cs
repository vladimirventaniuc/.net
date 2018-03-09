using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Biblioteca.API;

namespace Biblioteca.API
{
    public class BibliotecaManager
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
                    #region Meniu
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
                        //Show();
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Statistici
                    case "5":
                        Ui.ConsoleShow("Statistici:");
                        //Show();
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Adaugare cititor
                    case "6":
                        Ui.ConsoleShow("Adaugare cititor:");
                        //Show();
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Restituire carte 
                    case "7":
                        Ui.ConsoleShow("Restituire carte:");
                        //SetDates(out startDate, out endDate);

                        //Show();
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Query direct
                    case "8":
                        Ui.ConsoleShow("Query direct.");
                        //Show();
                        option = Ui.ConsoleRead();
                        break;
                    #endregion

                    #region Incheiere proces & defalut
                    case "0":
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

        private void ImprumutCarte(int bookId, int? idCitiror)
        {
            throw new NotImplementedException();
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
            if (autorId == 0)
               return false;

            int? genId = CheckIfGenreExistsAndAdd(gen);
            if (genId == 0)
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
                int? id = authors.FirstOrDefault(u => u.Nume == numeAutor && u.Prenume == prenumeAutor).AutorId;
               
                if (id == 0)
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
                if (id == 0)
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
        public IQueryable<object> SeeIfAllBooksWithATitle(string titlu)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;

            return borrows
                          .Join(books, bo => bo.CarteId, b => b.CarteId, (bo, b) => new {bo, b})
                          .Where(bob => bob.b.Titlu == titlu)
                          .Select(E => new
                          {
                          CarteId = E.bo.CarteId
                          ,DataRestituire = E.bo.DataRestituire
                          ,DataScadenta = E.bo.DataScadenta
                          });
        }
        public IQueryable<object> SeeIfAllBooksFromAnAuthor(string numeAutor,string prenumeAutor)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;
            DbSet<AUTOR> authors = dbContext.AUTOR;

            return borrows
                        .Join(books, bo => bo.CarteId, b => b.CarteId, (bo, b) => new {bo, b})
                        .Join(authors, bob => bob.b.AutorId, a => a.AutorId, (bob, a) => new {bob, a})
                        .Where(boba => boba.a.Nume == numeAutor && boba.a.Prenume == prenumeAutor)
                        .Select(E => new
                        {
                            CarteId = E.bob.b.CarteId
                            ,DataRestituire = E.bob.bo.DataRestituire
                            ,DataScadenta = E.bob.bo.DataScadenta
                        });
        }
        public IQueryable<object> SeeIfAllBooksFromAnAuthorAndTitle(string titlu, string numeAutor, string prenumeAutor)
        {
            DbSet<IMPRUMUT> borrows = dbContext.IMPRUMUT;
            DbSet<CARTE> books = dbContext.CARTE;
            DbSet<AUTOR> authors = dbContext.AUTOR;

            return borrows
                    .Join(books, bo => bo.CarteId, b => b.CarteId, (bo, b) => new {bo, b})
                    .Join(authors, bob => bob.b.AutorId, a => a.AutorId, (bob, a) => new {bob, a})
                    .Where(boba =>
                        boba.a.Nume == numeAutor && boba.a.Prenume == prenumeAutor && boba.bob.b.Titlu == titlu)
                    .Select(E => new
                    {
                        CarteId = E.bob.b.CarteId
                        ,DataRestituire = E.bob.bo.DataRestituire
                        ,DataScadenta = E.bob.bo.DataScadenta
                    });
        }
        public string GetMeAllTheBooksForBorrowing(string titlu, string nume, string prenume)
        {
            IQueryable<object> result;
            
            if (titlu != null && nume != null && prenume != null)
                result = SeeIfAllBooksFromAnAuthorAndTitle(titlu, nume, prenume);
            else if(titlu == null && nume != null && prenume != null)
            {
                result = SeeIfAllBooksFromAnAuthor(nume, prenume);
            }
            else if (titlu != null && nume == null && prenume == null)
            {
                result = SeeIfAllBooksWithATitle(titlu);
            }
            else
            {
                string message = "Nu ati tasat corect titlul, numele sau prenumele autorului. Reincercati!";

                return message;
            }

            int? id = result.SelectFirst(x => x.DataRestituire < DateTime.Now).CarteId;
            if (id == 0)
            {
                DateTime data = result.SelectFirst().DataScadenta;
                int nrZilDiferenta = (int)(DateTime.Now - data).TotalDays;
                return nrZilDiferenta + " zile";
            }

            return id.ToString();
        }
        public int? FindReaderIdByNameAndSurname(string nume, string prenume)
        {
            DbSet<CITITOR> readers = dbContext.CITITOR;
            int? cititorId = readers.FirstOrDefault(u => u.Nume == nume && u.Prenume == prenume).CititorId;
            if (cititorId == null)
                return 0;

            return cititorId;
        }
        public bool ImprumutCarte(int carteId, int cititorId)
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
        public bool RestituireCarte(string nume, string prenume)
        {
            //safisare toate cartile imprumutate si nerestituite ca lista
            //selectare si restituire din lista
            return true;
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
                if (id == 0)
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
    }
}
