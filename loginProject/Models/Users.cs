namespace loginProject.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string Email { get; set; } = string.Empty;
        public string NationalID { get; set; }
        public string City { get; set; }
        public string BloodBank { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }


        // verify
        public string? verificationToken { get; set; }
        public DateTime? verifiedAy { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }
}
