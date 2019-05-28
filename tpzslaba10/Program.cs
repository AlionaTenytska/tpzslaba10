using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace tpzslaba10
{
    class Program
    {
        class Block
        {
            public int index;
            public string previousHash;
            public string timestamp;
            public string data;
            public string hash;

            public Block(int index, string previousHash, string timestamp, string data, string hash)
            {
                this.index = index;
                this.previousHash = previousHash;
                this.timestamp = timestamp;
                this.data = data;
                this.hash = hash;
            }
        }
        static void Main(string[] args)
        {
            List<Block> blockchain = new List<Block>();
            blockchain.Add(GetGenesisBlock());
        }

        static string CalculateHash(int index, string previousHash, string timestamp, string data)
        {
            string hashResult = "";
            SHA256 hash = SHA256.Create();
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] combined = encoder.GetBytes(index.ToString() + previousHash + timestamp.ToString() + data);
            hash.ComputeHash(combined);
            foreach (var b in hash.Hash)
            {
                hashResult += b.ToString("X2");
            }

            return hashResult;
        }

        static Block GenerateNextBlock(string blockData)
        {
            Block previousBlock = getLatestBlock();
            var nextIndex = previousBlock.index + 1;
            var nextTimeStamp = DateTime.Now.ToString();
            var nextHash = CalculateHash(nextIndex, previousBlock.hash, nextTimeStamp, blockData);
            return new Block(nextIndex, previousBlock.hash, nextTimeStamp, blockData, nextHash);
        }

        static Block GetGenesisBlock()
        {
            return new Block(0, "0", "1465154705", "my genesis block!!", "816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7");
        }

        public static bool IsValidNewBlock(Block newBlock, Block previousBlock)
        {
            if (previousBlock.index + 1 != newBlock.index)
            {
                Console.WriteLine("неверный индекс");
                return false;
            }
            else if (previousBlock.hash != newBlock.previousHash)
            {
                Console.WriteLine("неверный хеш предыдущего блока");
                return false;
            }
            else if (CalculateHashForBlock(newBlock) != newBlock.hash)
            {
                Console.WriteLine("неверный хеш: " + CalculateHashForBlock(newBlock) + " " + newBlock.hash);
                return false;
            }
            return true;
        }

        public static void ReplaceChain(List<Block> newBlocks, List<Block> blockchain)
        {
            if (IsValidChain(newBlock) && newBlocks.Count > blockchain.Count)
            {
                Console.WriteLine("Принятый блокчейн является валидным. Происходит замена текущего блокчейна на принятый");
                blockchain = newBlocks;
                broadcast(responseLatesMsg());
            }
            else
            {
                Console.WriteLine("Принятый блокчейн не является валидным");
            }
        }

    }
}
