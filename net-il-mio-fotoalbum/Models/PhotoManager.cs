﻿using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Context;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoManager
    {
        //funzione per verificare che la tabella photo sia popolata
        public static int CountAllPhotos()
        {
            using (PhotoContext db = new PhotoContext())
            {
                return db.Photo.Count();
            }

        }

        //funzione per una lista di poto con annesse categorie e user
        public static List<Photo> GetAllPhotos(bool includeCategories = false)
        {
            SeedCat();

            Seed();

            using (PhotoContext db = new PhotoContext())
            {
                if (includeCategories) return db.Photo.Include(c => c.User).Include(i => i.Categories).ToList();

                else return db.Photo.ToList();
            }

        }

        //funzione popolamento tabella categoria foto
        public static void SeedCat()
        {
            using (PhotoContext db = new PhotoContext())
            {
                if (PhotoManager.CountAllPhotos() == 0)
                {
                    var categories = new List<Category>
                    {
                        new Category {Nome = "Still life"},
                        new Category {Nome = "Naturalistica"},
                        new Category {Nome = "Reportage"},
                        new Category {Nome = "Matrimonio"},
                        new Category {Nome = "Glamour"},
                        new Category {Nome = "Viaggio"}

                    };

                    // Verifica se l'ingrediente esiste già, altrimenti lo aggiunge
                    foreach (var categoria in categories)
                    {

                        db.Category.Add(categoria);
                    }

                    db.SaveChanges();

                }

            }
        }

        //funzione inserimento singole foto
        public static void Seed()
        {
            using (PhotoContext db = new PhotoContext())
            {
                if (PhotoManager.CountAllPhotos() == 0)
                {

                    // Recupera le categorie dal DB
                    var stilllife = db.Category.FirstOrDefault(i => i.Nome == "Still life")?.id.ToString();
                    var naturalistica = db.Category.FirstOrDefault(i => i.Nome == "Naturalistica")?.id.ToString();
                    var reportage = db.Category.FirstOrDefault(i => i.Nome == "Reportage")?.id.ToString();
                    var matrimonio = db.Category.FirstOrDefault(i => i.Nome == "Matrimonio")?.id.ToString();
                    var glamour = db.Category.FirstOrDefault(i => i.Nome == "Glamour")?.id.ToString();
                    var viaggio = db.Category.FirstOrDefault(i => i.Nome == "Viaggio")?.id.ToString();
              

                    // Crea una lista di stringhe con gli ID delle categorie

                    var primaFoto = new List<String>
                 {
                      stilllife,
                      viaggio,
                      reportage

                 };

                    var secondaFoto = new List<String>
                 {
                     matrimonio,
                     glamour,
                     stilllife
                 };

                    var terzaFoto = new List<String>
                 {
                     naturalistica,
                     reportage,
                     stilllife,
                     glamour
                 };

                   

                    PhotoManager.InsertPhoto(new Photo("TourModiale", "Esperienza fantastica", "~/img/Viaggio1.jpg", true, "73cce54a-42fd-4b12-a15f-98c7d2dde274"), primaFoto);
                    PhotoManager.InsertPhoto(new Photo("Matrimonio", "Bellissimo", "~/img/Matrimonio1.jpg", false, "73cce54a-42fd-4b12-a15f-98c7d2dde274"), secondaFoto);
                    PhotoManager.InsertPhoto(new Photo("TourModiale2", "Esperienza fantastica2", "~/img/Viaggio2.jpg", true, "73cce54a-42fd-4b12-a15f-98c7d2dde274"), terzaFoto);


                    db.SaveChanges();

                }

            }
        }

        //funzione inserimetno effettivo
        public static void InsertPhoto(Photo photo, List<string> SelectedCategories = null)
        {
            using (PhotoContext db = new PhotoContext())
            {
                if (SelectedCategories != null)
                {
                    photo.Categories = new List<Category>();

                    // Trasformiamo gli ID scelti in Ingredient da aggiungere tra i riferimenti in Pizza
                    foreach (var CategoriesId in SelectedCategories)
                    {
                        int id = int.Parse(CategoriesId);
                        var category = db.Category.FirstOrDefault(t => t.id == id); // PostManager.GetTagById(id); NON usiamo GetTagById() perché usa un db context diverso e ciò causerebbe errore in fase di salvataggio - usiamo lo stesso context all'interno della stessa operazione
                        photo.Categories.Add(category);
                    }
                }

                db.Photo.Add(photo);
                db.SaveChanges();
            }

        }

    }
}
