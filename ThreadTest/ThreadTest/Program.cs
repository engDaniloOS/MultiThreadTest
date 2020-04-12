using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class Program
    {
        private const int ITERACOES = 100;
        private const string _url = "https://www.google.com/";

        static async Task Main(string[] args)
        {
            try
            {
                IEnumerable<bool> resultados = null;

                Console.WriteLine($"Escolha a opção: {Environment.NewLine}" +
                                  $"1: Multi-Thread com conexão Assincrona;{Environment.NewLine}" +
                                  $"2: Mono-Thread com conexão Assincrona; {Environment.NewLine}" +
                                  $"3: Multi-Thread com conexão Sincrona;  {Environment.NewLine}" +
                                  $"4: Mono-Thread com conexão Sincrona;   {Environment.NewLine}");

                var opcao = Console.ReadLine();

                DateTime start = DateTime.Now;

                var lista = MontaLista();

                resultados = opcao switch
                {
                    "1" => await ProcessarMultThreadConexaoAsync(lista),
                    "2" => await ProcessarMonoThreadConexaoAsync(lista),
                    "3" => await ProcessarMultThreadConexaoSync(lista),
                    "4" => ProcessarMonoThreadConexaoSync(lista),
                    _ => new List<bool>().ToArray()
                };

                foreach (var item in resultados)
                    Console.WriteLine(item ? "Sucesso" : "Falhou");

                Console.WriteLine($"Tempo para processamento: {(start - DateTime.Now).ToString()}");

                Console.ReadLine();
            }
            catch (Exception)
            {
                return;
            }
        }

        private static async Task<bool[]> ProcessarMonoThreadConexaoAsync(List<string> lista)
        {
            var resultados = new List<bool>();

            foreach (var item in lista)
            {
                Console.WriteLine($"Conectanto {item}...");

                var consulta = new ConsultaWeb();

                resultados.Add(await consulta.AcessoAsync(_url));
            }

            return resultados.ToArray();
        }

        private static bool[] ProcessarMonoThreadConexaoSync(List<string> lista)
        {
            var resultados = new List<bool>();

            foreach (var item in lista)
            {
                Console.WriteLine($"Conectanto {item}...");

                var consulta = new ConsultaWeb();

                resultados.Add(consulta.Acesso(_url));
            }

            return resultados.ToArray();
        }

        private static async Task<bool[]> ProcessarMultThreadConexaoAsync(List<string> lista)
        {
            IEnumerable<Task<Task<bool>>> tarefas = lista.Select(item =>
                Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine($"Conectanto {item}...");

                    var consulta = new ConsultaWeb();

                    return await consulta.AcessoAsync(_url);
                }));

            Task<bool>[] tarefasPrincipais = await Task.WhenAll(tarefas);

            return await Task.WhenAll(tarefasPrincipais);
        }

        private static async Task<bool[]> ProcessarMultThreadConexaoSync(List<string> lista)
        {
            IEnumerable<Task<bool>> tarefas = lista.Select(item =>
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Conectanto {item}...");

                    var consulta = new ConsultaWeb();

                    return consulta.Acesso(_url);
                }));

            return await Task.WhenAll(tarefas);
        }

        private static List<string> MontaLista()
        {
            var lista = new List<string>();

            for (int x = 0; x < ITERACOES; x++)
                lista.Add(x.ToString());

            return lista;
        }
    }
}
