using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlockchain
{

    public static class FuncionalidadesDaBlockChain
    {
        public static byte[] GeradorDoHash(this IBloco bloco) //método gerador do hash 
        {
            using (SHA512 sha = new SHA512Managed())  //gerando o bloco
            using (MemoryStream st = new MemoryStream()) //criando um objeto do tipo Fluxo de Memória
            using (BinaryWriter bw = new BinaryWriter(st)) // passando o objeto Fluxo de Memória para o objeto Binary Writer
            {
                //escrevendo e atribuindo o fluxo de memória as propriedades do bloco
                bw.Write(bloco.Dado);
                bw.Write(bloco.Nonce);
                bw.Write(bloco.TimeStamp.ToBinary());
                bw.Write(bloco.PrevHash);
                var starr = st.ToArray();
                return sha.ComputeHash(starr); // criptografando o fluxo de memória
            }
        }
        public static byte[] MineHash(this IBloco bloco, byte[] dificuldade) //método para mineração do bloco 
        {
            if (dificuldade == null) throw new ArgumentNullException(nameof(dificuldade));

            byte[] hash = new byte[0];
            int d = dificuldade.Length;
            //preenchendo o hash 
            while (!hash.Take(2).SequenceEqual(dificuldade))
            {
                bloco.Nonce++;
                hash = bloco.GeradorDoHash();
            }
            return hash;
        } 
        public static bool ValidarBloco(this IBloco bloco) { }
        public static bool ValidarBlocoAnterior(this IBloco bloco, IBloco blocoAnterior) { }
        public static bool ValidarBloco(this IEnumerable<IBloco> itens) { }


    }

    public interface IBloco //interface compartilhada entre os blocos 
    {
        byte[] Dado { get; } //dado a ser enviado no bloco
        byte[] Hash { get; set; } //identificação e "chave" do bloco
        int Nonce { get; set; } //será um número aleatório para identificação usado uma só vez, google it
        byte[] PrevHash { get; set; } //hash do bloco anterior
        DateTime TimeStamp { get; } //marcação de data e hora no bloco

    }

    public class Bloco : IBloco
    {
        public byte[] Dado { get; }
        public byte[] Hash { get; set; }
        public int Nonce { get; set; }
        public byte[] PrevHash { get; set; }
        public DateTime TimeStamp { get; }

        public override string ToString()
        {
            //formatando o hash para imprimir na tela
            return $"{BitConverter.ToString(Hash).Replace("-", "")}:\n{BitConverter.ToString(PrevHash).Replace("-", "")}\n {Nonce} {TimeStamp}";
        }
    }


    public class BlockChain : IEnumerable<IBloco>
    {
        private List<IBloco> _itens = new List<IBloco>(); //referenciando um objeto do tipo Lista IBloco

        public BlockChain(byte[] dificuldade, IBloco genesis) //construtor do bloco
        {
            Dificuldade = dificuldade;  //medida de dificuldade para decodificação do bloco
            genesis.Hash = genesis.MineHash(dificuldade);
            Itens.Add(genesis);
        }


        public void Add(IBloco item) //adicionando item ao bloco
        {
            if (Itens.LastOrDefault() != null)
            {
                item.PrevHash = Itens.LastOrDefault()?.Hash; //retorna o último item do bloco (ou o padrão caso não haja item) ou o Hash
            }
            item.Hash = item.MineHash(Dificuldade);
            Itens.Add(item);
        }

        public int Count => Itens.Count; //contador para a alteração dos itens do bloco

        public IBloco this[int index]
        {
            get => Itens[index];
            set => Itens[index] = value;
        }


        //propriedades
        public List<IBloco> Itens //propriedade usada para alterar o valor de cada item (hash, hash prévio etc..)
        {
            get => _itens;
            set => _itens = value;
        }


        public byte[] Dificuldade { get; }

        public IEnumerator<IBloco> GetEnumerator()
        {
            return Itens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Itens.GetEnumerator();
        }
    }
    class Program
    {
        


        

        public static void Main(string[] args)
        {
        }
    }
}
