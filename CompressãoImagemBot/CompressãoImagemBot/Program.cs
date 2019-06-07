using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Telegram.Bot.Args;
using File = System.IO.File;

namespace CompressãoImagemBot
{
    class Program 
    {
        // Cria o objeto do telegram
        private static Telegram.Bot.ITelegramBotClient bot = new Telegram.Bot.TelegramBotClient("Coloque o token aqui");
        static void Main(string[] args)
        {
            try
            {
                //Cria o evendo de receber a mensagem
                bot.OnMessage += bot_OnMessage;
                //Cria o evendo de receber a mensagem e possa editar a mensagem
                bot.OnMessageEdited += bot_OnMessage;
                //conecta o bot
                bot.StartReceiving();
                Console.WriteLine("Iniciado bot");
                //faz do tela do console não fechar sozinha, so fez caso aperte ENTER
                Console.ReadKey();
                //Fecha a conexão do bot
                bot.StopReceiving();
            }catch(Exception ex)
            {
                //Execeção de erro caso o bot der algum problema
                Console.WriteLine(ex.Message);
            }
            
        }

        private static void bot_OnMessage(Object sende, MessageEventArgs e)
        {
            //Identifica que o usuario mando uma mensagem de texto
            if(e.Message.Type ==Telegram.Bot.Types.Enums.MessageType.Text)
            {
                //Comandos
                if(e.Message.Text=="/start")
                {
                    //Mostra os outros comando
                    bot.SendTextMessageAsync(e.Message.Chat.Id, @"Comandos de Lista de Imagens Comprimidas:
/Mar
/Floresta
/Montanha
/Vida_Marinha");
                    Console.WriteLine("Comanado mostrado para "+e.Message.Chat.FirstName);
                }
                else if (e.Message.Text == "/Mar")
                {   //função que envia uma foto
                    enviarFoto(e, @"Bot/Mar");
                }
                else if (e.Message.Text == "/Floresta")
                {
                    enviarFoto(e, @"Bot/Floresta");
                    
                }
                else if (e.Message.Text == "/Montanha")
                {
                    enviarFoto(e, @"Bot/Montanha");
                    
                }
                else if (e.Message.Text == "/Vida_Marinha")
                {
                    enviarFoto(e, @"Bot/Vida Marinha");
                }
            }
        }
        private static void enviarFoto(MessageEventArgs e, String dir)
        {
            //Envia uma foto para o usuario
            bot.SendPhotoAsync(e.Message.Chat.Id, Compressao(e, dir));
            //mostra a mensagem no console
            Console.WriteLine("Enviado a foto");
        }
        //Busca todos todos os caminhos de arquivo dentro do diretorio especifico
        private static List<String> BuscaArquivos(String dir)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            List<String> l = new List<string>();
            // lista arquivos do diretorio corrente
            foreach (FileInfo file in d.GetFiles())
            {
                // aqui no caso estou guardando o nome completo do arquivo em em controle ListBox
                // voce faz como quiser
                l.Add(file.FullName);
            }
            return l;
        }
        //Escolher um caminho aleatorio de arquivo
        private static String EscolhaDiretorio(String d)
        {
            List<String> l = BuscaArquivos(d);
            Random r = new Random();
            return l[r.Next(l.Count-1)];
        }
        //Faz a Compressão da imagem
        private static FileStream Compressao(MessageEventArgs e, String dir)
        {

            //Tranforma a imagem em bitmap
            Bitmap bmp = new Bitmap(EscolhaDiretorio(dir));
            //Inverte a imagem a contrario
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //Cria um repositorio de memoria
            MemoryStream ms = new MemoryStream();
            //sala os dados do bitmap na memoria
            bmp.Save(ms, ImageFormat.Bmp);
            //pega todos os bytes da memoria e colocar no vetor de bytes
            byte[] b = ms.ToArray();
            //ele copia os ultimos 54 bytes e deleta os 54 primeiros, apos isto empura a lista e adiciona a copia
            Array.Copy(b, 54, b, 0, b.Length - 54);
            //metodo de comprissão da imagem
            byte[] jpegBytes = ConvertBytestoJpegBytes(b, bmp.Width, bmp.Height);
            //salva a imagem no computador
            File.WriteAllBytes(@"Compression\TestForest.jpeg", jpegBytes);
            return new FileStream(@"Compression\TestForest.jpeg", FileMode.Open, FileAccess.Read);
        }
        private static byte[] ConvertBytestoJpegBytes(byte[] pixels24bpp, int W, int H)
        {
            //GChandle- permite acessr um objeto dentro de uma memoria não gerenciada
            //A Class GChandle utiliza-se ponteiro para alocar

            
            GCHandle gch = GCHandle.Alloc(pixels24bpp, GCHandleType.Pinned);//GCHandleType.Pinned - faz que o coletor de lixo perda eficiencia
            int stride = 4 * ((24 * W + 31) / 32);//calculo de deslocamento de uma linha de bytes para a proxima

            //Cria um bitimap com dimessão igual a foto anterior, o descolocamento de bytes, formato de cores.
            //AddrOfPinnedObject() - Recupera o endereço de um objeto em um identificador Fixado
            Bitmap bmp = new Bitmap(W, H, stride, PixelFormat.Format24bppRgb, gch.AddrOfPinnedObject());
            MemoryStream ms = new MemoryStream();//cria um local para salvar o bitmap
            bmp.Save(ms, ImageFormat.Jpeg);//salva o bitmap
            gch.Free();//libera o coletor para coletar o lixo
            return ms.ToArray();// retorna o vetor de memoria
        }

    }
}
