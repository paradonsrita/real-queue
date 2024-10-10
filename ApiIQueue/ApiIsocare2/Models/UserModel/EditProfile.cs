using System.ComponentModel.DataAnnotations;

namespace ApiIsocare2.Models.UserModel
{
    public class EditProfile
    {
        public int user_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone_number { get; set; }
        public string citizen_id_number { get; set; }
        public string user_email { get; set; }
    }
}
