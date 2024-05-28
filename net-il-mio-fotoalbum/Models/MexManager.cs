using net_il_mio_fotoalbum.Context;

namespace net_il_mio_fotoalbum.Models
{
    public class MexManager
    {
        //funzione inserimetno effettivo messaggio
        public static void InsertMessage(Message mex)
        {
            using (PhotoContext db = new PhotoContext())
            {
             
                db.Message.Add(mex);

                db.SaveChanges();
            }

        }
    }
}
