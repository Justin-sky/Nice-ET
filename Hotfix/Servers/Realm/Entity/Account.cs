using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
    public sealed class Account : Entity
    {
        public string AccountName { get; set; }
        public string Password { get; set; }

        public void Awake(string accountName, string password)
        {
            this.AccountName = accountName;
            this.Password = password;
        }
    }
}
