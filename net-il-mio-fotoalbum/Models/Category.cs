namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        public int id { get; set; }
        public string Nome { get; set; }

        //relazione tra la foto e le categorie.(many-to-many)
        public List<Photo> Photos { get; set; }

        //METODI
        public Category() { }

        public Category(string nome) : this()
        {
            this.Nome = nome;

        }
    }
}
