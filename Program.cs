using System;

namespace MeusFilmes
{
    class Program
    {
        static FilmeRepositorio repositorio = new FilmeRepositorio();
        static void Main(string[] args)
        {
            Console.WriteLine("##### API Meus Filmes a seu dispor!!! #####");
            string opcaoUsuario = ObterOpcaoUsuario();

            while (opcaoUsuario.ToUpper() != "X")
            {
                switch (opcaoUsuario)
                {
                    case "1":
                        ListarFilmes();
                        break;
                    case "2":
                        InserirFilme();
                        break;
                    case "3":
                        AtualizarFilme();
                        break;
                    case "4":
                        ExcluirFilme();
                        break;
                    case "5":
                        VisualizarFilme();
                        break;
                    case "C":
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("Comando inválido. Retornando ao Menu Inicial.");
                        break;
                }

                opcaoUsuario = ObterOpcaoUsuario();
            }

            Console.WriteLine("Obrigado por utilizar nossos serviços. Pressione qualquer tecla para finalizar a sessão.");
            Console.ReadLine();
            Console.WriteLine("Sessão finalizada com sucesso. Até breve.");

        }

        private static void ExcluirFilme()
        {
            Console.Write("Digite o id do filme: ");
            int indiceFilme;
            bool conversao = Int32.TryParse(Console.ReadLine(), out indiceFilme);
            var tamanho = repositorio.ProximoId(); // Aproveitamos o método pois ele retorna o numero de filmes já listados.

            Console.WriteLine();
            //Verificação da existência do filme
            if (conversao == false || indiceFilme < 0 || tamanho < indiceFilme + 1)
            {
                Console.WriteLine("Não existe nenhum filme cadastrado com o id informado. Retornando ao Menu Inicial.");
                return;
            }
            //Confirmação da exclusão do filme
            else
            {
                Console.WriteLine("Tem certeza que deseja excluir o filme? Digite 1 para Sim ou qualquer outra tecla para Não:");
                int confirmaExclusao;
                bool confirmar = Int32.TryParse(Console.ReadLine(), out confirmaExclusao);
                if (confirmar == false || confirmaExclusao != 1)
                {
                    Console.WriteLine("Exclusão não confirmada. Retornando ao Menu Inicial.");
                    Console.WriteLine();
                    return;
                }
            }
            repositorio.Exclui(indiceFilme);
            Console.WriteLine("Filme excluído com sucesso!");
        }

        private static void VisualizarFilme()
        {
            Console.Write("Digite o id do filme ou digite qualquer tecla para retornar ao Menu Inicial: ");
            int indiceFilme;
            bool conversao = Int32.TryParse(Console.ReadLine(), out indiceFilme);
            var tamanho = repositorio.ProximoId(); // Aproveitamos o método pois ele retorna o numero de filmes já listados.

            Console.WriteLine();

            if (conversao == false || indiceFilme < 0 || tamanho < indiceFilme + 1)
            {
                Console.WriteLine("Não existe nenhum filme cadastrado com o id informado. Retornando ao Menu Inicial.");
                return;
            }


            var filme = repositorio.RetornaPorId(indiceFilme);

            Console.WriteLine(filme);
        }

        private static void AtualizarFilme()
        {
            Console.Write("Digite o id do filme ou digite qualquer tecla para retornar ao Menu Inicial: ");
            int indiceFilme;
            bool conversao = Int32.TryParse(Console.ReadLine(), out indiceFilme);
            var tamanho = repositorio.ProximoId(); // Aproveitamos o método pois ele retorna o numero de filmes já listados.

            Console.WriteLine();
            //Confirmação da existência do filme na lista
            if (conversao == false || indiceFilme < 0 || tamanho < indiceFilme + 1)
            {
                Console.WriteLine("Não existe nenhum filme cadastrado com o id informado. Retornando ao Menu Inicial.");
                return;
            }

            // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
            // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
            foreach (int i in Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine("{0}-{1}", i, Enum.GetName(typeof(Genero), i));
            }
            //Confirmação da existência do gênero
            Console.Write("Digite o gênero entre as opções acima ou digite qualquer tecla para retornar ao Menu Inicial: ");
            int entradaGenero;
            bool entrada = Int32.TryParse(Console.ReadLine(), out entradaGenero);

            while (entradaGenero < 1 || entradaGenero > 13 || entrada == false)
            {
                if (entrada == false)
                {
                    Console.WriteLine("Gênero inexistente. Retornando ao Menu Inicial");
                    return;
                }
                Console.WriteLine("Gênero inexistente. Escolha um dos gêneros da lista ou digite qualquer outra tecla para retornar ao Menu Inicial: ");
                entrada = Int32.TryParse(Console.ReadLine(), out entradaGenero);
            }


            Console.Write("Digite o título do filme: ");
            string entradaTitulo = Console.ReadLine();
            //Verificação da escolha de um número inteiro para  indicar o ano do filme
            Console.Write("Digite o ano de início do filme: ");
            int entradaAno;
            bool conversaoAno = Int32.TryParse(Console.ReadLine(), out entradaAno);

            Console.WriteLine();

            if (conversaoAno == false)
            {
                Console.WriteLine("Não existe o ano especificado. Retornando ao Menu Inicial.");
                return;
            }

            Console.Write("Digite a descrição do filme: ");
            string entradaDescricao = Console.ReadLine();

            Filme atualizaFilme = new Filme(id: indiceFilme,
                                        genero: (Genero)entradaGenero,
                                        titulo: entradaTitulo,
                                        ano: entradaAno,
                                        descricao: entradaDescricao);

            repositorio.Atualiza(indiceFilme, atualizaFilme);
        }

        private static void ListarFilmes()
        {
            Console.WriteLine("Lista de filmes:");
            Console.WriteLine();

            var lista = repositorio.Lista();

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum filme cadastrado.");
                return;
            }

            foreach (var filme in lista)
            {
                var excluido = filme.retornaExcluido();

                Console.WriteLine("#ID {0}: - {1} {2}", filme.retornaId(), filme.retornaTitulo(), (excluido ? "*Excluído*" : ""));
            }
        }

        private static void InserirFilme()
        {
            Console.WriteLine("Inserir novo filme");

            // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
            // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
            foreach (int i in Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine("{0}-{1}", i, Enum.GetName(typeof(Genero), i));
            }
            //Confirmação da existência do gênero
            Console.Write("Digite o gênero entre as opções acima ou qualquer outra tecla para retornar ao Menu Inicial: ");
            int entradaGenero;
            bool entrada = Int32.TryParse(Console.ReadLine(), out entradaGenero);

            while (entradaGenero < 1 || entradaGenero > 13 || entrada == false)
            {
                if (entrada == false)
                {
                    Console.WriteLine("Gênero inexistente. Retornando ao Menu Inicial");
                    return;
                }
                Console.WriteLine("Gênero inexistente. Escolha um dos gêneros da lista ou qualquer outra tecla para retornar ao Menu Inicial: ");
                entrada = Int32.TryParse(Console.ReadLine(), out entradaGenero);
            }

            Console.Write("Digite o título do filme: ");
            string entradaTitulo = Console.ReadLine();
            //Verificação da escolha de um número inteiro para o ano do filme
            Console.Write("Digite o ano de início do filme: ");

            int entradaAno;
            bool conversaoAno = Int32.TryParse(Console.ReadLine(), out entradaAno);

            Console.WriteLine();

            if (conversaoAno == false)
            {
                Console.WriteLine("Não existe o ano especificado. Retornando ao Menu Inicial.");
                return;
            }

            Console.Write("Digite a descrição do filme: ");
            string entradaDescricao = Console.ReadLine();

            Filme novoFilme = new Filme(id: repositorio.ProximoId(),
                                        genero: (Genero)entradaGenero,
                                        titulo: entradaTitulo,
                                        ano: entradaAno,
                                        descricao: entradaDescricao);

            repositorio.Insere(novoFilme);
        }


        private static string ObterOpcaoUsuario()
        {
            Console.WriteLine();
            Console.WriteLine("### Menu Inicial ###");
            Console.WriteLine();
            Console.WriteLine("Informe a opção desejada:");
            Console.WriteLine();
            Console.WriteLine("1- Listar filmes");
            Console.WriteLine("2- Inserir novo filme");
            Console.WriteLine("3- Atualizar filme");
            Console.WriteLine("4- Excluir filme");
            Console.WriteLine("5- Visualizar filme");
            Console.WriteLine("C- Limpar Tela");
            Console.WriteLine("X- Sair");
            Console.WriteLine();

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }

    }
}
