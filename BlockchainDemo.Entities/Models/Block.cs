using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using BlockchainDemo.Helpers;

namespace BlockchainDemo.Entities.Models
{
    public class Block
    {

        public long Id { get; set; }
        public string Data { get; private set; }
        public string Hash { get; private set; }
        public string Prevhash { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int Difficulty { get; private set; }
        public long Nonce { get; private set; }
        public double Performance { get; private set; }

        public Block(string data, string prevhash, int difficulty)
        {
            this.Data = data;
            this.Prevhash = prevhash;
            this.Difficulty = difficulty;

            long nonce = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string result = string.Empty;

            using (SHA256 mySHA256 = SHA256.Create())
            {
                result = ConvertTo.Hex(mySHA256.ComputeHash(Encoding.UTF8.GetBytes(this.Id + this.Prevhash + this.Timestamp + this.Data + nonce)), true);
                string diff = new string('0', this.Difficulty);

                while (!result.StartsWith(diff, StringComparison.Ordinal))
                {
                    nonce += 1;
                    result = ConvertTo.Hex(mySHA256.ComputeHash(Encoding.UTF8.GetBytes(this.Id + this.Prevhash + this.Timestamp + this.Data + nonce)), true);
                }

                this.Hash = result;
            }

            stopWatch.Stop();

            this.Timestamp = DateTime.UtcNow;
            this.Nonce = nonce;
            this.Performance = stopWatch.Elapsed.TotalSeconds;
        }
    }
}
