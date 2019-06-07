using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Dropbox.Api;
using Telegram.Bot.Args;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Dropbox.Api.Files;
using System.Globalization;
using System.Threading;

namespace CompressãoImagemBoy
{
    public partial class frmTelaBot : Form
    {

        private TelegramBotClient bot;
        private String tokenDropBox;
        public frmTelaBot()
        {
            InitializeComponent();
            bot = new TelegramBotClient("");
            tokenDropBox = "";
            bot.OnMessage += OnMessage;
            CriarPasta();
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                EnviarImagemComprimidaAsync(e);
            }
        }

        private void CriarPasta()
        {
            if (!Directory.Exists(@"C:\BotCompressor"))
            {
                Directory.CreateDirectory(@"C:\BotCompressor");
                Directory.CreateDirectory(@"C:\BotCompressor\Imagens");
                Directory.CreateDirectory(@"C:\BotCompressor\Compressão");
            }
        }
        private async Task EnviarImagemComprimidaAsync(MessageEventArgs e)
        {
            String caminho= await DownloadFoto(e);
            FileStream imagem= Compressao(e, caminho);
            String url = await Upload(@""+caminho);
            String tamanho = tamanhoArquivo(caminho);
            if (caminho != "" && url != "")
                bot.SendPhotoAsync(e.Message.Chat.Id, imagem, "Tamanho da imagem: "+tamanho+" KB\nLink para DownLoad: " + url);
            else
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Não foi posivel enviar a mensagem");
        }
        private String tamanhoArquivo(String caminho)
        {
            String arq = caminho.Split('\\').Last();
            FileInfo arquivo = new FileInfo(@"C:\BotCompressor\Compressão\"+arq);
            return ((float)arquivo.Length / 1024).ToString("N1", new CultureInfo("en-US", false));
            
                
        }
        private async Task<String> DownloadFoto(MessageEventArgs e)
        {
            try
            {
                var file = bot.GetFileAsync(e.Message.Document.FileId);
                String extension = file.Result.FilePath.Split('.').Last();

                if (extension == "png" || extension == "jpg" || extension == "jpeg")
                {
                    using (var salvar = File.Open(@"C:\BotCompressor\Imagens\" + file.Result.FileId + "." + extension, FileMode.Create))
                    {
                        await bot.DownloadFileAsync(file.Result.FilePath, salvar);
                    }

                    if (extension == "png")
                    {
                        using (var bit = new Bitmap(@"C:\BotCompressor\Imagens\" + file.Result.FileId + "." + extension))
                        {
                            Image im = (Image)bit;
                            im.Save(@"C:\BotCompressor\Imagens\" + file.Result.FileId + ".jpg", ImageFormat.Jpeg);
                            extension = "jpg";
                        }

                    }
                    return @"C:\BotCompressor\Imagens\" + file.Result.FileId + "." + extension;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no DownLoad: {0}",ex.Message);
            }
            return "";
        }
        private async Task<String> Upload(String dir)
        {
            try
            {
                using (var drop = new DropboxClient(tokenDropBox))
                {
                    String url;
                    var imagem = dir.Split('\\').Last();
                    using (var up = new MemoryStream(File.ReadAllBytes(dir)))
                    {
                        var update = await drop.Files.UploadAsync("/ImagensB/" + imagem, WriteMode.Overwrite.Instance, body: up);
                        var link = await drop.Sharing.CreateSharedLinkAsync("/ImagensB/" + imagem);
                        url = link.Url;
                    }
                    return url;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no Upload: {0} ",ex.Message);
            }
            return "";
        }
        private FileStream Compressao(Telegram.Bot.Args.MessageEventArgs e, String dir)
        {
            
            String ultimo = dir.Split('\\').Last();
            //Tranforma a imagem em bitmap
            Bitmap bmp = new Bitmap(dir);

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
            File.WriteAllBytes(@"C:\BotCompressor\Compressão\" + ultimo, jpegBytes);

            return new FileStream(@"C:\BotCompressor\Compressão\" + ultimo, FileMode.Open, FileAccess.Read);
        }
        private byte[] ConvertBytestoJpegBytes(byte[] pixels24bpp, int W, int H)
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

        private void btnEstadoBot_Click(object sender, EventArgs e)
        {
            if (btnEstadoBot.Text == "Desconectado")
            {
                btnEstadoBot.Text = "Conectado";
                bot.StartReceiving();
            }
            else
            {
                btnEstadoBot.Text = "Desconectado";
                bot.StopReceiving();
            }
        }

        private void frmTelaBot_Load(object sender, EventArgs e)
        {

        }
    }
}
