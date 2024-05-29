using Microsoft.AspNetCore.Mvc.Rendering;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }

        //public List<Category>? Categories { get; set; }


        //INGREDIENTI
        public List<SelectListItem>? Categories { get; set; } // Gli elementi delle categorie selezionate

        public List<string>? SelectedCategories { get; set; } // Gli ID degli elementi (categorie) effettivamente selezionati

        public PhotoFormModel() { }

        public PhotoFormModel(Photo p)
        {
            this.Photo = p;
            if (Photo.Categories != null)
            {
                this.SelectedCategories = this.Photo.Categories.Select(c => c.id.ToString()).ToList();
            }
            else
            {
                this.SelectedCategories = new List<string> { };
            }
        }

        public void CreateCategories(List<Category> InputCategory)
        {
            this.Categories = new List<SelectListItem>();

            if (this.SelectedCategories == null)
                this.SelectedCategories = new List<string>();
            

            foreach (var Categoria in InputCategory) // es. categoria1, categoria2, categoria3... categoria10
            {
                bool isSelected = this.SelectedCategories.Contains(Categoria.id.ToString());


                this.Categories.Add(new SelectListItem() // lista delle categorie selezionabili
                {
                    Text = $"{Categoria.Nome}",  // Nome categoria visualizzato 
                    Value = Categoria.id.ToString(), // SelectListItem vuole una generica stringa, non un int
                    Selected = isSelected // es. tag1, tag5, tag9
                });

                

            }
        }

    }
}
