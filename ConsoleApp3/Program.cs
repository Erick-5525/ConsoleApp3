using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System;
using System.Threading.Tasks;

public class YouTubeTranscription
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Por favor, insira a URL do vídeo do YouTube:");
        string videoUrl = Console.ReadLine();  // Recebe a URL do vídeo

        // Extrair o ID do vídeo da URL
        string videoId = ExtractVideoId(videoUrl);

        if (string.IsNullOrEmpty(videoId))
        {
            Console.WriteLine("URL inválida. Não foi possível extrair o ID do vídeo.");
            return;
        }

        Console.WriteLine("Por favor, insira a chave da API do YouTube:");
        string apiKey = Console.ReadLine();  // Recebe a chave da API digitada pelo usuário

        // Verificar se a chave foi fornecida
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("A chave da API não foi fornecida corretamente.");
            return;
        }

        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "YouTubeAPIExample"
        });

        var captionsRequest = youtubeService.Captions.List("snippet");
        captionsRequest.VideoId = videoId;

        try
        {
            var captionsResponse = await captionsRequest.ExecuteAsync();

            if (captionsResponse.Items.Count > 0)
            {
                Console.WriteLine("Legendas disponíveis para este vídeo:");
                foreach (var caption in captionsResponse.Items)
                {
                    Console.WriteLine($"Idioma: {caption.Snippet.Language}");
                    Console.WriteLine($"ID da legenda: {caption.Id}");

                }
            }
            else
            {
                Console.WriteLine("Este vídeo não possui legendas disponíveis.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao acessar as legendas: " + ex.Message);
        }
    }

    // Função para extrair o ID do vídeo da URL
    private static string ExtractVideoId(string url)
    {
        var match = System.Text.RegularExpressions.Regex.Match(url, @"(?:youtube\.com\/(?:[^\/\n\s]*\/\S*\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})");
        return match.Success ? match.Groups[1].Value : null;
    }
}
